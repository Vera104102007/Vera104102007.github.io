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
            // Возврат словаря при очистке поля
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
            var limited = source.Take(1000).ToArray(); //
            listBox1.Items.AddRange(limited);
            listBox1.EndUpdate();
            toolStripStatusLabel1.Text = $"В словаре: {mySlovar.Count} | Найдено: {source.Count}";
        }

        // --- КНОПКИ ---

        private void button3_Click(object sender, EventArgs e) // Найти (Точный)
        {
            if (mySlovar == null) return;
            numericLength.Enabled = false; // Блокируем длину
            RefreshUI(mySlovar.ExactSearch(textBox1.Text));
        }

        private void button2_Click(object sender, EventArgs e) // Удалить (Исправлено)
        {
            if (mySlovar == null || listBox1.SelectedItem == null) return;
            mySlovar.DeleteWord(listBox1.SelectedItem.ToString());
            RefreshUI();
        }

        private void button1_Click(object sender, EventArgs e) // Добавить
        {
            if (mySlovar == null || string.IsNullOrWhiteSpace(textBox1.Text)) return;
            mySlovar.AddWord(textBox1.Text);
            RefreshUI(mySlovar.ExactSearch(textBox1.Text));
        }

        // --- МЕНЮ ---

        // Работа со словарем -> Поиск (Левенштейн)
        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mySlovar == null || string.IsNullOrWhiteSpace(textBox1.Text)) return;
            numericLength.Enabled = false; // Блокируем длину
            this.Cursor = Cursors.WaitCursor;
            RefreshUI(mySlovar.LevenshteinSearch(textBox1.Text, 3));
            this.Cursor = Cursors.Default;
        }

        // Работа со словарем -> Нечеткий поиск (Длина + Часть)
        private void fuzzySearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (mySlovar == null) return;
            numericLength.Enabled = true; // РАЗБЛОКИРУЕМ длину
            RefreshUI(mySlovar.SearchVariant4((int)numericLength.Value, textBox1.Text));
        }
    }
}