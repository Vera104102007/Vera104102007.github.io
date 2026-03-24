using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DictionaryLib;

namespace DictionaryApp
{
    public partial class Form1 : Form
    {
        private Slovar mySlovar;
        private string defaultPath = Path.Combine(Application.StartupPath, "russian.txt");

        public Form1()
        {
            InitializeComponent();
            LoadDictionary(defaultPath);
            // Авто-сброс при очистке текстового поля
            textBox1.TextChanged += (s, e) => { if (string.IsNullOrEmpty(textBox1.Text)) RefreshUI(); };
        }

        private void LoadDictionary(string path)
        {
            if (File.Exists(path)) mySlovar = new Slovar(path);
            RefreshUI();
        }

        private void RefreshUI(List<string> dataToShow = null)
        {
            if (mySlovar == null) return;
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            var source = dataToShow ?? mySlovar.GetAll();
            // Показываем только первые 1000 для скорости
            var limited = source.Take(1000).ToArray();
            listBox1.Items.AddRange(limited);
            listBox1.EndUpdate();
            toolStripStatusLabel1.Text = $"В словаре: {mySlovar.Count} | Показано: {limited.Length}";
        }

        // --- КНОПКИ ---

        // Button 3: ОБЫЧНЫЙ ПОИСК (Один в один)
        private void button3_Click(object sender, EventArgs e)
        {
            if (mySlovar == null) return;
            numericLength.Enabled = false; // Блокируем длину
            RefreshUI(mySlovar.ExactSearch(textBox1.Text));
        }

        // Button 1: ДОБАВИТЬ
        private void button1_Click(object sender, EventArgs e)
        {
            if (mySlovar == null || string.IsNullOrWhiteSpace(textBox1.Text)) return;
            mySlovar.AddWord(textBox1.Text);
            RefreshUI(mySlovar.ExactSearch(textBox1.Text));
        }

        // Button 2: УДАЛИТЬ
        private void button2_Click(object sender, EventArgs e)
        {
            if (mySlovar == null || listBox1.SelectedItem == null) return;
            mySlovar.DeleteWord(listBox1.SelectedItem.ToString());
            RefreshUI();
        }

        // --- МЕНЮ ---

        // Меню "Поиск": ЛЕВЕНШТЕЙН (Расстояние <= 3)
        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mySlovar == null) return;
            numericLength.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            var results = mySlovar.LevenshteinSearch(textBox1.Text, 3);
            RefreshUI(results);
            this.Cursor = Cursors.Default;
        }

        // Меню "Нечетный поиск": ВАРИАНТ 4 (Длина + Начало/Конец)
        private void fuzzySearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mySlovar == null) return;
            numericLength.Enabled = true; // РАЗБЛОКИРУЕМ ПОЛЕ ДЛИНЫ
            var results = mySlovar.SearchVariant4((int)numericLength.Value, textBox1.Text);
            RefreshUI(results);
        }
    }
}