using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        private List<Figure> _figures = new List<Figure>();
        private Figure _selectedFigure;
        private Figure _copyBuffer; // Буфер обмена для Копировать/Вставить
        private Stroke _currentStroke = new Stroke();
        private StackMemory _history = new StackMemory(20);
        private StackMemory _redoHistory = new StackMemory(20);
        private bool _isDrawing = false;
        private bool _isResizing = false; // Флаг режима изменения размера
        private SelectionFrame _selectionFrame = new SelectionFrame(); // Экземпляр помощника
        private Point _lastMousePos;      // Для вычисления дельты перемещения
        private MarkerPosition _activeMarker = MarkerPosition.None;

        public MainForm()
        {
            InitializeComponent();
            this.KeyPreview = true;

            // Настройки по умолчанию
            if (toolStripComboBox1.Items.Count > 0) toolStripComboBox1.SelectedIndex = 0;
            if (toolStripComboBox2.Items.Count > 0) toolStripComboBox2.SelectedIndex = 0;
            _currentStroke.Color = Color.Black;
        }

        // --- 1, 2, 3: РАБОТА С ФАЙЛАМИ ---

        private void создать_Click(object sender, EventArgs e)
        {
            _figures.Clear();
            _history.Clear();
            _redoHistory.Clear();
            _selectedFigure = null;
            pictureBox1.Invalidate();
        }

        private void открыть_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Vector Drawing|*.bin" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                {
                    _figures = (List<Figure>)new BinaryFormatter().Deserialize(fs);
                    _history.Clear();
                    _redoHistory.Clear();
                    pictureBox1.Invalidate();
                }
            }
        }

        private void сохранить_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "Vector Drawing|*.bin" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (FileStream fs = new FileStream(sfd.FileName, FileMode.Create))
                    new BinaryFormatter().Serialize(fs, _figures);
            }
        }

        // --- 4, 5, 6: БУФЕР ОБМЕНА (Копировать, Вставить, Вырезать) ---

        private void копировать_Click(object sender, EventArgs e)
        {
            if (_selectedFigure != null) _copyBuffer = CloneFigure(_selectedFigure);
        }

        private void вставить_Click(object sender, EventArgs e)
        {
            if (_copyBuffer != null)
            {
                SaveState(_history);
                Figure newFig = CloneFigure(_copyBuffer);
                newFig.Move(10, 10); // Смещение, чтобы видеть копию
                _figures.Add(newFig);
                _selectedFigure = newFig;
                pictureBox1.Invalidate();
            }
        }

        private void вырезать_Click(object sender, EventArgs e)
        {
            if (_selectedFigure != null)
            {
                SaveState(_history);
                _copyBuffer = CloneFigure(_selectedFigure);
                _figures.Remove(_selectedFigure);
                _selectedFigure = null;
                pictureBox1.Invalidate();
            }
        }

        // --- 7, 8: ОТМЕНА И ПОВТОР (Undo/Redo) ---

        private void отменить_Click(object sender, EventArgs e)
        {
            if (_history.Count > 0)
            {
                SaveState(_redoHistory); // Сохраняем текущее в Redo
                RestoreState(_history);
            }
        }

        private void повторить_Click(object sender, EventArgs e)
        {
            if (_redoHistory.Count > 0)
            {
                SaveState(_history); // Сохраняем текущее в Undo
                RestoreState(_redoHistory);
            }
        }

        // --- 9, 10, 11: ИНСТРУМЕНТЫ И ПАРАМЕТРЫ ---

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // Если мы выключаем режим выделения, сбрасываем выбранную фигуру
            if (!toolStripButton3.Checked)
            {
                _selectedFigure = null;
                pictureBox1.Invalidate();
            }
        }

        private void выбратьЦвет_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                _currentStroke.Color = colorDialog1.Color;
                if (_selectedFigure != null)
                {
                    _selectedFigure.StrokeConfig.Color = colorDialog1.Color;
                    pictureBox1.Invalidate();
                }
            }
        }

        private void toolStripComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (float.TryParse(toolStripComboBox2.Text, out float width))
            {
                _currentStroke.Width = width;
                if (_selectedFigure != null)
                {
                    _selectedFigure.StrokeConfig.Width = width;
                    pictureBox1.Invalidate();
                }
            }
        }

        // --- ВСПОМОГАТЕЛЬНЫЕ МЕТОДЫ (ЛОГИКА) ---

        private void SaveState(StackMemory stack)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                new BinaryFormatter().Serialize(ms, _figures);
                stack.Push(ms);
            }
        }

        private void RestoreState(StackMemory stack)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stack.Pop(ms);
                ms.Position = 0;
                _figures = (List<Figure>)new BinaryFormatter().Deserialize(ms);
                _selectedFigure = null;
                pictureBox1.Invalidate();
            }
        }

        private Figure CloneFigure(Figure source)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, source);
                ms.Position = 0;
                return (Figure)bf.Deserialize(ms);
            }
        }

        // Обработка рисования в PictureBox
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            _lastMousePos = e.Location;

            if (toolStripButton3.Checked) // Режим "Указатель"
            {
                if (_selectedFigure != null)
                {
                    // Проверяем, попал ли курсор в какой-либо из 4-х маркеров
                    _activeMarker = _selectionFrame.GetHitMarker(e.Location, _selectedFigure.Bounds);

                    if (_activeMarker != MarkerPosition.None)
                    {
                        _isResizing = true;
                        return; // Начинаем трансформацию, не меняя выделение
                    }
                }

                // Если в маркер не попали, ищем фигуру под курсором для выделения
                _selectedFigure = _figures.FindLast(f => f.Bounds.Contains(e.Location));
                _isResizing = false;
            }
            else // Режим рисования новой фигуры
            {
                SaveState(_history);
                _redoHistory.Clear();
                _selectedFigure = CreateFigureFromSelection();
                _selectedFigure.Bounds = new Rectangle(e.Location, new Size(0, 0));
                _selectedFigure.StrokeConfig = new Stroke { Color = _currentStroke.Color, Width = _currentStroke.Width };
                _figures.Add(_selectedFigure);
                _isDrawing = true;
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_selectedFigure == null) return;

            if (_isResizing) // ТРАНСФОРМАЦИЯ ЗА МАРКЕРЫ
            {
                Rectangle b = _selectedFigure.Bounds;
                int x1 = b.Left;
                int y1 = b.Top;
                int x2 = b.Right;
                int y2 = b.Bottom;

                // В зависимости от маркера меняем соответствующую координату
                switch (_activeMarker)
                {
                    case MarkerPosition.TopLeft:
                        x1 = e.X; y1 = e.Y;
                        break;
                    case MarkerPosition.TopRight:
                        x2 = e.X; y1 = e.Y;
                        break;
                    case MarkerPosition.BottomLeft:
                        x1 = e.X; y2 = e.Y;
                        break;
                    case MarkerPosition.BottomRight:
                        x2 = e.X; y2 = e.Y;
                        break;
                }

                // Нормализация: позволяем тянуть углы в любом направлении без ошибок
                int newX = Math.Min(x1, x2);
                int newY = Math.Min(y1, y2);
                int newWidth = Math.Max(5, Math.Abs(x1 - x2));
                int newHeight = Math.Max(5, Math.Abs(y1 - y2));

                _selectedFigure.Bounds = new Rectangle(newX, newY, newWidth, newHeight);
                pictureBox1.Invalidate();
            }
            else if (_isDrawing) // РИСОВАНИЕ НОВОЙ ФИГУРЫ
            {
                int x = Math.Min(_lastMousePos.X, e.X);
                int y = Math.Min(_lastMousePos.Y, e.Y);
                int w = Math.Abs(_lastMousePos.X - e.X);
                int h = Math.Abs(_lastMousePos.Y - e.Y);
                _selectedFigure.Bounds = new Rectangle(x, y, w, h);
                pictureBox1.Invalidate();
            }
            else if (toolStripButton3.Checked && e.Button == MouseButtons.Left) // ПЕРЕМЕЩЕНИЕ
            {
                int dx = e.X - _lastMousePos.X;
                int dy = e.Y - _lastMousePos.Y;
                _selectedFigure.Move(dx, dy);
                _lastMousePos = e.Location;
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
            _isResizing = false;
            _activeMarker = MarkerPosition.None;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Рисуем все фигуры
            foreach (var f in _figures)
                f.Draw(e.Graphics);

            // Рисуем рамку и маркеры вокруг выделенной фигуры
            if (_selectedFigure != null && toolStripButton3.Checked)
            {
                _selectionFrame.Draw(e.Graphics, _selectedFigure.Bounds);
            }
        }



        private Figure CreateFigureFromSelection()
        {
            switch (toolStripComboBox1.Text)
            {
                case "Треугольник": return new RegularPolygon(3);
                case "5-угольник": return new RegularPolygon(5);
                case "6-угольник": return new RegularPolygon(6);
                default: return new SquareFigure();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_selectedFigure != null && toolStripButton3.Checked)
            {
                // Если зажат Shift — шаг 1, иначе — 5
                int step = (keyData.HasFlag(Keys.Shift)) ? 1 : 5;

                switch (keyData & Keys.KeyCode)
                {
                    case Keys.Left:
                        _selectedFigure.Move(-step, 0);
                        pictureBox1.Invalidate();
                        return true;
                    case Keys.Right:
                        _selectedFigure.Move(step, 0);
                        pictureBox1.Invalidate();
                        return true;
                    case Keys.Up:
                        _selectedFigure.Move(0, -step);
                        pictureBox1.Invalidate();
                        return true;
                    case Keys.Down:
                        _selectedFigure.Move(0, step);
                        pictureBox1.Invalidate();
                        return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

    }
}