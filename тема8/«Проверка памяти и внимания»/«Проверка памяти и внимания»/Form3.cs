using Name;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Name
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            dataGridView1.RowCount = 15;
            this.BackColor = GameState.BackgroundColor;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            int score = 0;    
            int seqScore = 0; 

            int columnIndex = dataGridView1.Columns.Count > 0 ? dataGridView1.Columns.Count - 1 : 0;

            for (int i = 0; i < GameState.Sequence.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[columnIndex].Value == null) continue;

                string userWord = dataGridView1.Rows[i].Cells[columnIndex].Value.ToString().Trim().ToLower();
                if (string.IsNullOrEmpty(userWord)) continue;

                int correctId = GameState.Sequence[i];
                string correctWord = GameState.Items[correctId].Trim().ToLower();

                if (GameState.Items.Any(item => item.Trim().ToLower() == userWord))
                {
                    score++;
                }

                if (userWord == correctWord)
                {
                    seqScore++;
                }
            }

            string resultData = $"{DateTime.Now:dd.MM HH:mm} — Верно: {score}, Порядок: {seqScore}";

            GameState.SessionHistory.Add($"{GameState.PlayerName}|{resultData}");

            try
            {
                using (BinaryWriter writer = new BinaryWriter(File.Open("results.dat", FileMode.Append)))
                {
                    writer.Write(GameState.PlayerName);
                    writer.Write(resultData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Не удалось сохранить в файл: " + ex.Message);
            }

            MessageBox.Show($"Игра окончена!\nВерно: {score}\nПорядок: {seqScore}");

            this.Close();
            if (Application.OpenForms["Form1"] != null)
            {
                Application.OpenForms["Form1"].Show();
            }
        }
    }
}