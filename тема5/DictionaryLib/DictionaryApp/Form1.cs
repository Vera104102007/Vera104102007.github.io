using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DictionaryLib;

namespace DictionaryApp
{
    public partial class Form1 : Form
    {
        private Slovar mySlovar = new Slovar();

        public Form1()
        {
            InitializeComponent();
            RefreshUI(); // При старте пусто
            textBox1.TextChanged += (s, e) => { if (string.IsNullOrEmpty(textBox1.Text)) RefreshUI(); };
        }

        private void RefreshUI(List<string> dataToShow = null)
        {
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            var source = dataToShow ?? mySlovar.GetAll();
            listBox1.Items.AddRange(source.Take(1000).ToArray());
            listBox1.EndUpdate();
            toolStripStatusLabel1.Text = $"В словаре: {mySlovar.Count} | Найдено: {source.Count}"; //
        }

        // --- МЕНЮ "СЛОВАРЬ" ---

        private void openToolStripMenuItem_Click(object sender, EventArgs e) // Открыть
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Text Files (*.txt)|*.txt" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                mySlovar.LoadFromFile(ofd.FileName);
                RefreshUI();
            }
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e) // Создать новый
        {
            mySlovar.ClearAll();
            RefreshUI();
        }

        private void deleteCurrentToolStripMenuItem_Click(object sender, EventArgs e) // Удалить текущий
        {
            if (MessageBox.Show("Очистить текущий словарь?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                mySlovar.ClearAll();
                RefreshUI();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) // Сохранить как
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "Text Files (*.txt)|*.txt" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                mySlovar.SaveToFile(sfd.FileName);
            }
        }

        // --- ПОИСК И КНОПКИ ---

        // Кнопка "Добавить" (Button1)
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)) return;
            mySlovar.AddWord(textBox1.Text);
            RefreshUI(); // Показываем обновленный список
            textBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e) // Найти (Точный)
        {
            numericLength.Enabled = false; // Блокируем длину
            RefreshUI(mySlovar.ExactSearch(textBox1.Text));
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e) // Левенштейн
        {
            numericLength.Enabled = false; // Блокируем длину
            RefreshUI(mySlovar.LevenshteinSearch(textBox1.Text, 3));
        }

        private void fuzzySearchToolStripMenuItem_Click(object sender, EventArgs e) // Вариант 4
        {
            numericLength.Enabled = true; // РАЗБЛОКИРУЕМ для нечеткого поиска
            RefreshUI(mySlovar.SearchVariant4((int)numericLength.Value, textBox1.Text));
        }

        private void button2_Click(object sender, EventArgs e) // Удалить выделенное
        {
            if (listBox1.SelectedItem != null)
            {
                mySlovar.DeleteWord(listBox1.SelectedItem.ToString());
                RefreshUI();
            }
        }
    }
}