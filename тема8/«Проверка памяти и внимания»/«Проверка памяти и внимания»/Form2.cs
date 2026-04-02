using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq; 
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Name
{
    public partial class Form2 : Form
    {
        int step = 0; 

        public Form2()
        {
            InitializeComponent();
            this.BackColor = GameState.BackgroundColor;
        }


        private void Form2_Load(object sender, EventArgs e)
        {
            List<int> allIndices = Enumerable.Range(0, 15).ToList();

            Random rnd = new Random();
            GameState.Sequence = allIndices.OrderBy(x => rnd.Next()).ToList();

            timer1.Interval = GameState.Interval;
            timer1.Start();

            ShowNextImage();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            step++;
            if (step < GameState.Sequence.Count)
            {
                ShowNextImage();
            }
            else
            {
                timer1.Stop();
                Form3 f3 = new Form3();
                f3.Show();
                this.Hide();
            }
        }

        private void ShowNextImage()
        {
            int currentId = GameState.Sequence[step];

            label1.Text = GameState.Items[currentId];

            string fileName = (currentId + 1).ToString() + ".jpeg";

            string projectPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            string path = Path.Combine(projectPath, "image", fileName);

            if (File.Exists(path))
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    pictureBox1.Image = Image.FromStream(stream);
                }
            }
            else
            {
                MessageBox.Show($"Картинка не найдена! Ищу тут: {path}");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        { //привет 
        }

    }
    }

   
