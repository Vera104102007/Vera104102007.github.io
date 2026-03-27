using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApp1
{
    public partial class GameForm : Form
    {
        // Укажите здесь правильное имя вашего XML файла
        private const string PathToQuiz = "quiz.xml";

        string theme;
        int level;
        List<Question> allQuestions = new List<Question>();
        List<Question> sessionQuestions;
        int currentQ = 0;
        int correctAnswers = 0;
        int seconds = 60;

        public GameForm(string theme, int level)
        {
            InitializeComponent();
            this.theme = theme;
            this.level = level;

            LoadXmlData();

            // Выбираем 5 случайных вопросов
            if (allQuestions.Count >= 5)
                sessionQuestions = allQuestions.OrderBy(x => Guid.NewGuid()).Take(5).ToList();
            else
                sessionQuestions = allQuestions;

            if (sessionQuestions.Count > 0)
            {
                ShowQuestion();
                timer1.Interval = 1000;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Вопросы для этого уровня не найдены в XML!");
                // Используем BeginInvoke, чтобы закрыть форму после её полной загрузки
                this.Load += (s, e) => this.Close();
            }
        }

        private void LoadXmlData()
        {
            // Путь к XML в папке запуска
            var path = Path.Combine(Application.StartupPath, PathToQuiz);
            if (!File.Exists(path)) return;

            using (XmlReader xr = XmlReader.Create(path))
            {
                while (xr.Read())
                {
                    if (xr.Name == "theme" && xr.GetAttribute("name") == theme)
                    {
                        while (xr.Read() && !(xr.Name == "theme" && xr.NodeType == XmlNodeType.EndElement))
                        {
                            if (xr.Name == "level" && xr.GetAttribute("id") == level.ToString())
                            {
                                while (xr.Read() && !(xr.Name == "level" && xr.NodeType == XmlNodeType.EndElement))
                                {
                                    if (xr.Name == "q")
                                    {
                                        Question q = new Question();
                                        q.Text = xr.GetAttribute("text");
                                        q.Image = xr.GetAttribute("src");
                                        q.Answers = new List<string>();

                                        int ansIdx = 0;
                                        // Читаем ответы внутри вопроса
                                        using (XmlReader inner = xr.ReadSubtree())
                                        {
                                            while (inner.Read())
                                            {
                                                if (inner.Name == "a" && inner.NodeType == XmlNodeType.Element)
                                                {
                                                    if (inner.GetAttribute("right") == "yes")
                                                        q.RightIndex = ansIdx;

                                                    q.Answers.Add(inner.ReadElementContentAsString());
                                                    ansIdx++;
                                                }
                                            }
                                        }
                                        allQuestions.Add(q);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ShowQuestion()
        {
            // 1. Получаем текущий вопрос из списка сессии
            var q = sessionQuestions[currentQ];

            // 2. Настройка текстовых меток (Текст вопроса и счетчик)
            label1.Text = q.Text;
            label2.Text = $"Вопрос {currentQ + 1} из {sessionQuestions.Count}";

            // 3. Работа с изображением (Безопасный режим)
            // Сначала очищаем старое изображение из памяти
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }

            // Проверяем, указано ли имя файла в данных вопроса
            if (!string.IsNullOrWhiteSpace(q.Image))
            {
                // Формируем пути для поиска (3 шага назад для .NET 9 и папка рядом с .exe)
                string projectImagesPath = Path.Combine(Application.StartupPath, @"..\..\..\images", q.Image);
                string localImagesPath = Path.Combine(Application.StartupPath, "images", q.Image);

                if (File.Exists(projectImagesPath))
                {
                    pictureBox1.Image = Image.FromFile(projectImagesPath);
                    pictureBox1.Visible = true;
                }
                else if (File.Exists(localImagesPath))
                {
                    pictureBox1.Image = Image.FromFile(localImagesPath);
                    pictureBox1.Visible = true;
                }
                else
                {
                    // Если файл прописан в XML, но физически отсутствует — просто скрываем PictureBox
                    pictureBox1.Visible = false;
                }
            }
            else
            {
                // Если в XML поле src пустое (как в твоих новых вопросах) — скрываем PictureBox
                pictureBox1.Visible = false;
            }

            // 4. Настройка вариантов ответов (RadioButtons)
            // Берем текст ответов напрямую из списка q.Answers, который ты заполнила в админке
            radioButton1.Text = q.Answers.Count > 0 ? q.Answers[0] : "Вариант не загружен";
            radioButton2.Text = q.Answers.Count > 1 ? q.Answers[1] : "Вариант не загружен";
            radioButton3.Text = q.Answers.Count > 2 ? q.Answers[2] : "Вариант не загружен";

            // Сбрасываем выделение с кнопок для нового вопроса
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e) // Кнопка "Готово"
        {
            int selected = -1;
            if (radioButton1.Checked) selected = 0;
            else if (radioButton2.Checked) selected = 1;
            else if (radioButton3.Checked) selected = 2;

            if (selected == -1)
            {
                MessageBox.Show("Выберите вариант ответа!");
                return;
            }

            if (selected == sessionQuestions[currentQ].RightIndex) correctAnswers++;

            currentQ++;
            if (currentQ < sessionQuestions.Count)
                ShowQuestion();
            else
                EndGame();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            seconds--;
            label3.Text = $"Осталось: {seconds} сек";
            if (seconds <= 0) EndGame();
        }

        private void EndGame()
        {
            timer1.Stop();
            int finalScore = (sessionQuestions.Count > 0) ? (correctAnswers * 100) / sessionQuestions.Count : 0;
            MessageBox.Show($"Игра окончена! Баллы: {finalScore}");

            // Запись результатов (убедитесь, что эти статические свойства есть в MainForm)
            if (level == 1) MainForm.ScoreLevel1 = finalScore;
            if (level == 2) MainForm.ScoreLevel2 = finalScore;

            this.Close();
        }
    }

    // Класс вопроса (если он у вас в отдельном файле, этот блок можно удалить)
    public class Question
    {
        public string Text { get; set; }
        public string Image { get; set; }
        public List<string> Answers { get; set; } = new List<string>();
        public int RightIndex { get; set; }
    }
}