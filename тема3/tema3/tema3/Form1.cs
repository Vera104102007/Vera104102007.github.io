using System;
using System.Drawing;
using System.Windows.Forms;

namespace tema3
{
    public partial class Form1 : Form
    {
        public Color circleColor = Color.Red;
        public int speed = 5;

        private double x, y; 
        private int dx, dy;
        private int radius = 15;
        private int squareSize = 250;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.KeyPreview = true;

            InitMovement();
        }

        private void InitMovement()
        {
            dx = speed;
            dy = speed;

            int left = (ClientSize.Width - squareSize) / 2;
            x = left + radius;
            y = ClientSize.Height / 2;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int left = (ClientSize.Width - squareSize) / 2;
            int right = left + squareSize;
            int top = (ClientSize.Height - squareSize) / 2;
            int bottom = top + squareSize;
            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;

            x += (dx > 0) ? speed : -speed;
            y += (dy > 0) ? speed : -speed;


            if (x + radius >= right)
            {
                dx = -speed;
                x = right - radius;
                y = centerY; 
            }
            else if (x - radius <= left)
            {
                dx = speed;
                x = left + radius;
                y = centerY; 
            }

            if (y + radius >= bottom)
            {
                dy = -speed;
                y = bottom - radius;
                x = centerX; 
            }
            else if (y - radius <= top)
            {
                dy = speed;
                y = top + radius;
                x = centerX; 
            }

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            int left = (ClientSize.Width - squareSize) / 2;
            int top = (ClientSize.Height - squareSize) / 2;

            g.DrawRectangle(Pens.Black, left, top, squareSize, squareSize);

            using (Brush brush = new SolidBrush(circleColor))
            {
                g.FillEllipse(brush, (float)(x - radius), (float)(y - radius), radius * 2, radius * 2);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 settings = new Form2(this);
            settings.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Application.Exit(); 
        }
    }
}