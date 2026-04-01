using System.Drawing;

public enum MarkerPosition { None, TopLeft, TopRight, BottomLeft, BottomRight }

public class SelectionFrame
{
    private const int MarkerSize = 6;

    public void Draw(Graphics g, Rectangle bounds)
    {
        using (Pen p = new Pen(Color.Gray))
        {
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            g.DrawRectangle(p, bounds);
        }

        // Отрисовка всех 4 угловых маркеров
        DrawMarker(g, bounds.Left, bounds.Top);      // Верхний левый
        DrawMarker(g, bounds.Right, bounds.Top);     // Верхний правый
        DrawMarker(g, bounds.Left, bounds.Bottom);   // Нижний левый
        DrawMarker(g, bounds.Right, bounds.Bottom);  // Нижний правый
    }

    private void DrawMarker(Graphics g, int x, int y)
    {
        g.FillRectangle(Brushes.White, x - MarkerSize / 2, y - MarkerSize / 2, MarkerSize, MarkerSize);
        g.DrawRectangle(Pens.Black, x - MarkerSize / 2, y - MarkerSize / 2, MarkerSize, MarkerSize);
    }

    // Проверка, в какой маркер попал пользователь
    public MarkerPosition GetHitMarker(Point p, Rectangle bounds)
    {
        if (IsPointInMarker(p, bounds.Left, bounds.Top)) return MarkerPosition.TopLeft;
        if (IsPointInMarker(p, bounds.Right, bounds.Top)) return MarkerPosition.TopRight;
        if (IsPointInMarker(p, bounds.Left, bounds.Bottom)) return MarkerPosition.BottomLeft;
        if (IsPointInMarker(p, bounds.Right, bounds.Bottom)) return MarkerPosition.BottomRight;
        return MarkerPosition.None;
    }

    private bool IsPointInMarker(Point p, int x, int y)
    {
        Rectangle rect = new Rectangle(x - 5, y - 5, 10, 10);
        return rect.Contains(p);
    }
}