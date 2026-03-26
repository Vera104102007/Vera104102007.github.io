using System;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        public static int ScoreLevel1 = 0;
        public static int ScoreLevel2 = 0;

        public MainForm()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e) => OpenGame(1);
        private void button2_Click(object sender, EventArgs e) => OpenGame(2);
        private void button3_Click(object sender, EventArgs e) => OpenGame(3);

        private void OpenGame(int level)
        {
            GameForm gf = new GameForm(comboBox1.Text, level);
            gf.ShowDialog();

            // Обновляем доступ к уровням после закрытия игры
            if (ScoreLevel1 >= 80) button2.Enabled = true;
            if (ScoreLevel2 >= 80) button3.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdminForm af = new AdminForm();
            af.ShowDialog();
        }
    }
}