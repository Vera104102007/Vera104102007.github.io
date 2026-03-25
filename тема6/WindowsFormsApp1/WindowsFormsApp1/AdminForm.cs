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

        private void button2_Click(object sender, EventArgs e) // Сохранить
        {
            if (!File.Exists("quiz.xml")) return;

            XDocument doc = XDocument.Load("quiz.xml");
            var themeNode = doc.Root.Elements("theme").FirstOrDefault(x => x.Attribute("name").Value == comboBox1.Text);
            var levelNode = themeNode?.Elements("level").FirstOrDefault(x => x.Attribute("id").Value == numericUpDown1.Value.ToString());

            if (levelNode != null)
            {
                XElement newQ = new XElement("q",
                    new XAttribute("text", textBox1.Text),
                    new XAttribute("src", textBox2.Text),
                    new XElement("a", new XAttribute("right", radioButton1.Checked ? "yes" : "no"), radioButton1.Text),
                    new XElement("a", new XAttribute("right", radioButton2.Checked ? "yes" : "no"), radioButton2.Text),
                    new XElement("a", new XAttribute("right", radioButton3.Checked ? "yes" : "no"), radioButton3.Text)
                );

                levelNode.Add(newQ);
                doc.Save("quiz.xml");
                MessageBox.Show("Вопрос добавлен!");
            }
        }
    }
}