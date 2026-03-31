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
            if (toolStripButton3.Checked) // Режим Указателя
            {
                // 1. Проверяем, не нажали ли мы на маркер (например, нижний правый) текущей выделенной фигуры
                if (_selectedFigure != null && _selectionFrame.IsBottomRightMarker(e.Location, _selectedFigure.Bounds))
                {
                    _isResizing = true;
                }
                else
                {
                    // 2. Если не по маркеру, ищем фигуру под курсором для выделения
                    _selectedFigure = _figures.FindLast(f => f.Bounds.Contains(e.Location));
                    _isResizing = false;
                }
            }
            else // Режим рисования
            {
                SaveState(_history);
                _redoHistory.Clear();
                _selectedFigure = CreateFigureFromSelection();
                _selectedFigure.Bounds = new Rectangle(e.X, e.Y, 0, 0);
                _selectedFigure.StrokeConfig = new Stroke { Color = _currentStroke.Color, Width = _currentStroke.Width };
                _figures.Add(_selectedFigure);
                _isDrawing = true;
            }
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isResizing && _selectedFigure != null)
            {
                // Растягивание: меняем ширину и высоту относительно верхнего левого угла
                int newWidth = e.X - _selectedFigure.Bounds.X;
                int newHeight = e.Y - _selectedFigure.Bounds.Y;

                // Ограничение, чтобы фигура не схлопнулась в 0
                if (newWidth < 5) newWidth = 5;
                if (newHeight < 5) newHeight = 5;

                _selectedFigure.Bounds = new Rectangle(
                    _selectedFigure.Bounds.X,
                    _selectedFigure.Bounds.Y,
                    newWidth,
                    newHeight
                );
                pictureBox1.Invalidate();
            }
            else if (_isDrawing && _selectedFigure != null)
            {
                // Обычная логика рисования новой фигуры
                _selectedFigure.Bounds = new Rectangle(
                    _selectedFigure.Bounds.X, _selectedFigure.Bounds.Y,
                    e.X - _selectedFigure.Bounds.X, e.Y - _selectedFigure.Bounds.Y
                );
                pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            _isDrawing = false;
            _isResizing = false;
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

        
    }
}