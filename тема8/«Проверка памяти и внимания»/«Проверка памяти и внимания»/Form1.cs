using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Name
{
    public partial class Form1 : Form
    {

        private string binPath = "results.dat";
        public Form1()
        {
            InitializeComponent();
            this.BackColor = GameState.BackgroundColor;
            LoadHistoryFromFile();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Введите логин!");
                return;
            }
            GameState.PlayerName = textBox1.Text;
            new Form2().Show();
            this.Hide();
        }

        private void LoadHistoryFromFile()
        {
            if (!File.Exists(binPath)) return;

            try
            {
                GameState.SessionHistory.Clear();
                using (BinaryReader reader = new BinaryReader(File.Open(binPath, FileMode.Open)))
                {
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        string name = reader.ReadString();
                        string data = reader.ReadString();
                        GameState.SessionHistory.Add($"{name}|{data}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка загрузки истории: " + ex.Message);
            }
        }

        

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string currentPlayer = textBox1.Text;
            if (string.IsNullOrWhiteSpace(currentPlayer))
            {
                MessageBox.Show("Введите имя для поиска");
                return;
            }

            string historyText = $"Вся история для {currentPlayer}:\n";
            bool found = false;

            foreach (string item in GameState.SessionHistory)
            {
                string[] parts = item.Split('|');
                if (parts[0].Trim().ToLower() == currentPlayer.Trim().ToLower())
                {
                    historyText += parts[1] + "\n";
                    found = true;
                }
            }
            MessageBox.Show(found ? historyText : "История не найдена.");
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Бинарные файлы (*.dat)|*.dat";
            saveFileDialog.Title = "Сохранить результаты как...";
            saveFileDialog.FileName = "results.dat"; 

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string chosenPath = saveFileDialog.FileName;

                try
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(chosenPath, FileMode.Create)))
                    {
                        foreach (string item in GameState.SessionHistory)
                        {
                            string[] parts = item.Split('|');
                            if (parts.Length >= 2)
                            {
                                writer.Write(parts[0]); // Логин
                                writer.Write(parts[1]); // Результат
                            }
                        }
                    }
                    MessageBox.Show("Файл успешно сохранен!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении: " + ex.Message);
                }
            }
        }

        private void интервалПоказаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSettings fs = new FormSettings(GameState.Interval);
            fs.ShowDialog();
        }

        private void цветФонаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                GameState.BackgroundColor = colorDialog1.Color;
                foreach (Form f in Application.OpenForms)
                {
                    f.BackColor = GameState.BackgroundColor;
                }
            }
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox1 about = new AboutBox1())
            {
                about.BackColor = GameState.BackgroundColor;

                about.ShowDialog();
            }
        }


    }
}