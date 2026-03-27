using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            comboBox1.Items.AddRange(new string[] { "Европа", "Азия", "Америка" });
        }

        private void button1_Click(object sender, EventArgs e) // Выбрать фото
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = Path.GetFileName(openFileDialog1.FileName);
                string imgDir = Path.Combine(Application.StartupPath, "images");
                if (!Directory.Exists(imgDir)) Directory.CreateDirectory(imgDir);

                string dest = Path.Combine(imgDir, textBox2.Text);
                if (!File.Exists(dest)) File.Copy(openFileDialog1.FileName, dest);
            }
        }

        private void button2_Click(object sender, EventArgs e) // Сохранить/Добавить
        {
            // 1. Проверяем наличие файла XML
            if (!File.Exists("quiz.xml"))
            {
                MessageBox.Show("Файл quiz.xml не найден!");
                return;
            }

            // 2. Загружаем документ и ищем нужную тему и уровень
            XDocument doc = XDocument.Load("quiz.xml");
            var themeNode = doc.Root.Elements("theme")
                .FirstOrDefault(x => x.Attribute("name")?.Value == comboBox1.Text);

            var levelNode = themeNode?.Elements("level")
                .FirstOrDefault(x => x.Attribute("id")?.Value == numericUpDown1.Value.ToString());

            if (levelNode != null)
            {
                // 3. Создаем новый элемент вопроса <q>
                // Текст вопроса берем из textBox1, имя картинки из textBox2
                XElement newQ = new XElement("q",
                    new XAttribute("text", textBox1.Text),
                    new XAttribute("src", textBox2.Text)
                );

                // 4. Добавляем варианты ответов <a>
                // Текст берем из текстовых полей (txtAnswer1, txtAnswer2, txtAnswer3), 
                // а пометку 'right' — из состояния соответствующих RadioButton
                newQ.Add(new XElement("a",
                    new XAttribute("right", radioButton1.Checked ? "yes" : "no"),
                    textBox3.Text)); // Используем текст из поля ввода

                newQ.Add(new XElement("a",
                    new XAttribute("right", radioButton2.Checked ? "yes" : "no"),
                    textBox4.Text)); // Используем текст из поля ввода

                newQ.Add(new XElement("a",
                    new XAttribute("right", radioButton3.Checked ? "yes" : "no"),
                    textBox5.Text)); // Используем текст из поля ввода

                // 5. Сохраняем изменения
                levelNode.Add(newQ);
                doc.Save("quiz.xml");

                MessageBox.Show("Вопрос успешно добавлен в XML!");

                // Опционально: очищаем поля после добавления
                ClearAdminFields();
            }
            else
            {
                MessageBox.Show("Не удалось найти указанную тему или уровень в XML.");
            }
        }

        // Вспомогательный метод для очистки полей (чтобы не стирать всё вручную)
        private void ClearAdminFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;
        }
    }
}