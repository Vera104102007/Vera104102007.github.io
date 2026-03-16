using System;
using System.Windows.Forms;

namespace tema3
{
    public partial class Form2 : Form
    {
        Form1 mainForm; 

        public Form2(Form1 f)
        {
            InitializeComponent();
            mainForm = f;

            if (trackBar1 != null)
                trackBar1.Value = mainForm.speed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                mainForm.circleColor = colorDialog1.Color;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            mainForm.speed = trackBar1.Value;
        }
    }
}