using System.Drawing;
using System.Drawing.Drawing2D;

public class SelectionFrame
{
    private const int MarkerSize = 6;

    public void Draw(Graphics g, Rectangle bounds)
    {
        using (Pen p = new Pen(Color.Gray))
        {
            p.DashStyle = DashStyle.Dash;
            g.DrawRectangle(p, bounds); // Пунктирная рамка
        }

        int[] xs = { bounds.Left, bounds.Left + bounds.Width / 2, bounds.Right };
        int[] ys = { bounds.Top, bounds.Top + bounds.Height / 2, bounds.Bottom };

        foreach (int x in xs)
            foreach (int y in ys)
                if (!(x == xs[1] && y == ys[1])) // Не рисуем в центре
                    g.FillRectangle(Brushes.White, x - MarkerSize / 2, y - MarkerSize / 2, MarkerSize, MarkerSize);
    }

    // Метод для определения, попал ли клик в маркер (упрощенно для нижнего правого)
    public bool IsBottomRightMarker(Point p, Rectangle bounds)
    {
        Rectangle rect = new Rectangle(bounds.Right - 5, bounds.Bottom - 5, 10, 10);
        return rect.Contains(p);
    }
}