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
        private List<string> lastResults = null; // Храним результаты последнего поиска

        public Form1()
        {
            InitializeComponent();
            RefreshUI();
            textBox1.TextChanged += (s, e) => { if (string.IsNullOrEmpty(textBox1.Text)) RefreshUI(); };
        }

        private void RefreshUI(List<string> dataToShow = null)
        {
            lastResults = dataToShow; // Обновляем текущий результат
            listBox1.BeginUpdate();
            listBox1.Items.Clear();
            var source = dataToShow ?? mySlovar.GetAll();
            listBox1.Items.AddRange(source.Take(1000).ToArray());
            listBox1.EndUpdate();
            toolStripStatusLabel1.Text = $"В словаре: {mySlovar.Count} | Найдено: {source.Count}"; //
        }


        private void button1_Click(object sender, EventArgs e) // Добавить
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)) return;
            mySlovar.AddWord(textBox1.Text);
            RefreshUI();
            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e) // Удалить
        {
            if (listBox1.SelectedItem == null) return;
            mySlovar.DeleteWord(listBox1.SelectedItem.ToString());
            RefreshUI();
        }

        private void button3_Click(object sender, EventArgs e) // Найти
        {
            numericLength.Enabled = false;
            RefreshUI(mySlovar.ExactSearch(textBox1.Text));
        }

        // --- МЕНЮ: СЛОВАРЬ ---

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Text Files (*.txt)|*.txt" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                mySlovar.LoadFromFile(ofd.FileName);
                RefreshUI();
            }
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mySlovar.ClearAll();
            RefreshUI();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "Text Files (*.txt)|*.txt" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                mySlovar.SaveToFile(sfd.FileName);
            }
        }

        private void deleteCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mySlovar.ClearAll();
            RefreshUI();
        }

        // --- МЕНЮ: РАБОТА СО СЛОВАРЕМ ---

        private void searchToolStripMenuItem_Click(object sender, EventArgs e) // Поиск Левенштейна
        {
            numericLength.Enabled = false;
            RefreshUI(mySlovar.LevenshteinSearch(textBox1.Text, 3));
        }

        private void fuzzySearchToolStripMenuItem_Click(object sender, EventArgs e) // Нечеткий поиск
        {
            numericLength.Enabled = true;
            RefreshUI(mySlovar.SearchVariant4((int)numericLength.Value, textBox1.Text));
        }

        // ИСПРАВЛЕННЫЙ МЕТОД: СОХРАНИТЬ РЕЗУЛЬТАТ ПОИСКА
        private void saveSearchResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastResults == null || lastResults.Count == 0)
            {
                MessageBox.Show("Нет результатов поиска для сохранения.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog { Filter = "Text Files (*.txt)|*.txt", Title = "Сохранить результаты поиска" };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                mySlovar.SaveResults(sfd.FileName, lastResults);
                MessageBox.Show($"Успешно сохранено слов: {lastResults.Count}");
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Создаем экземпляр формы AboutBox1
            AboutBox1 aboutWindow = new AboutBox1();

            // Открываем её как модальное окно (пока оно открыто, главная форма недоступна)
            aboutWindow.ShowDialog();
        }
    }
}