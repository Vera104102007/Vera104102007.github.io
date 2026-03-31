using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace WindowsFormsApp1
{
    [Serializable]
    public class Stroke // Класс настроек линии
    {
        public Color Color { get; set; } = Color.Black;
        public float Width { get; set; } = 1f;
        public DashStyle DashStyle { get; set; } = DashStyle.Solid;
    }

    [Serializable]
    public class StackMemory // Класс для Undo (отмены)
    {
        private readonly int _depth;
        private readonly List<byte[]> _items = new List<byte[]>();

        public StackMemory(int depth) { _depth = depth; }

        public void Push(MemoryStream ms)
        {
            if (_items.Count >= _depth) _items.RemoveAt(0);
            _items.Add(ms.ToArray());
        }

        public void Clear()
        {
            _items.Clear();
        }

        public void Pop(MemoryStream ms)
        {
            if (_items.Count == 0) return;
            byte[] buffer = _items[_items.Count - 1];
            ms.Write(buffer, 0, buffer.Length);
            _items.RemoveAt(_items.Count - 1);
        }

        public int Count => _items.Count;
    }
}