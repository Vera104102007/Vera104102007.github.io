using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApp1
{
    public partial class GameForm : Form
    {
        private const string PathToQuiz = "../../../quiz.xml";

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
                this.Close();
            }
        }

        private void LoadXmlData()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), PathToQuiz);
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

                                        int ansIdx = 0;
                                        while (xr.Read() && !(xr.Name == "q" && xr.NodeType == XmlNodeType.EndElement))
                                        {
                                            if (xr.Name == "a")
                                            {
                                                if (xr.GetAttribute("right") == "yes") q.RightIndex = ansIdx;
                                                q.Answers.Add(xr.ReadElementContentAsString());
                                                ansIdx++;
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
            var q = sessionQuestions[currentQ];
            label1.Text = q.Text;
            label2.Text = $"Вопрос {currentQ + 1} из {sessionQuestions.Count}";

            string imgPath = System.IO.Path.Combine(Application.StartupPath, "images", q.Image);
            if (System.IO.File.Exists(imgPath))
                pictureBox1.Image = Image.FromFile(imgPath);

            radioButton1.Text = q.Answers.Count > 0 ? q.Answers[0] : "";
            radioButton2.Text = q.Answers.Count > 1 ? q.Answers[1] : "";
            radioButton3.Text = q.Answers.Count > 2 ? q.Answers[2] : "";

            radioButton1.Checked = radioButton2.Checked = radioButton3.Checked = false;
        }

        private void button1_Click(object sender, EventArgs e) // Кнопка "Готово"
        {
            int selected = -1;
            if (radioButton1.Checked) selected = 0;
            if (radioButton2.Checked) selected = 1;
            if (radioButton3.Checked) selected = 2;

            if (selected == sessionQuestions[currentQ].RightIndex) correctAnswers++;

            currentQ++;
            if (currentQ < sessionQuestions.Count) ShowQuestion();
            else EndGame();
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
            int finalScore = (correctAnswers * 100) / sessionQuestions.Count;
            MessageBox.Show($"Игра окончена! Баллы: {finalScore}");

            if (level == 1) MainForm.ScoreLevel1 = finalScore;
            if (level == 2) MainForm.ScoreLevel2 = finalScore;

            this.Close();
        }
    }
}