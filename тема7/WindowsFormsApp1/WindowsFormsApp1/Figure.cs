using System;
using System.Drawing;

namespace WindowsFormsApp1
{
    [Serializable]
    public abstract class Figure
    {
        public Rectangle Bounds { get; set; }
        public Stroke StrokeConfig { get; set; } = new Stroke();
        public abstract void Draw(Graphics g);

        public void Move(int dx, int dy)
        {
            this.Bounds = new Rectangle(Bounds.X + dx, Bounds.Y + dy, Bounds.Width, Bounds.Height);
        }

        public void DrawMarkers(Graphics g)
        {
            int s = 6;
            // Рисуем маркеры по углам для 4 варианта
            Rectangle[] markers = {
                new Rectangle(Bounds.Left - s/2, Bounds.Top - s/2, s, s),
                new Rectangle(Bounds.Right - s/2, Bounds.Top - s/2, s, s),
                new Rectangle(Bounds.Left - s/2, Bounds.Bottom - s/2, s, s),
                new Rectangle(Bounds.Right - s/2, Bounds.Bottom - s/2, s, s)
            };
            foreach (var m in markers)
            {
                g.FillRectangle(Brushes.White, m);
                g.DrawRectangle(Pens.Black, m);
            }
        }
    }

    [Serializable]
    public class SquareFigure : Figure
    {
        public override void Draw(Graphics g)
        {
            using (Pen p = new Pen(StrokeConfig.Color, StrokeConfig.Width))
                g.DrawRectangle(p, Bounds);
        }
    }

    [Serializable]
    public class RegularPolygon : Figure
    {
        private int _sides;
        public RegularPolygon(int sides) { _sides = sides; }
        public override void Draw(Graphics g)
        {
            if (Bounds.Width < 5 || Bounds.Height < 5) return;
            PointF[] pts = new PointF[_sides];
            float rx = Bounds.Width / 2f, ry = Bounds.Height / 2f;
            float cx = Bounds.X + rx, cy = Bounds.Y + ry;
            for (int i = 0; i < _sides; i++)
            {
                double a = 2 * Math.PI * i / _sides - Math.PI / 2;
                pts[i] = new PointF(cx + rx * (float)Math.Cos(a), cy + ry * (float)Math.Sin(a));
            }
            using (Pen p = new Pen(StrokeConfig.Color, StrokeConfig.Width))
                g.DrawPolygon(p, pts);
        }
    }
}