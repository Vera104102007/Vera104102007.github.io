using System;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        ArrayBrower myData = new ArrayBrower();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                string name = comboBox1.SelectedItem.ToString();
                bool found = false;
                for (int i = 0; i < myData.Count; i++)
                {
                    if (myData[i].Name == name)
                    {
                        myData[i].Vote++;
                        found = true;
                        break;
                    }
                }
                if (!found) myData.Add(new Brower(name, 1));
                MessageBox.Show("Ваш голос за " + name + " учтен!");
            }
            else
            {
                MessageBox.Show("Выберите браузер из списка!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;

                myData.OpenFile(path);

                UpdateTable();
                MessageBox.Show("Данные загружены из: " + path);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt";
            saveFileDialog1.DefaultExt = "txt";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = saveFileDialog1.FileName;

                myData.SaveToFile(path);

                MessageBox.Show("Данные успешно сохранены в: " + path);
            }
        }

        private void UpdateTable()
        {
            if (dataGridView1.Columns.Count == 0) return; 

            dataGridView1.Rows.Clear();
            myData.AllProcent();
            for (int i = 0; i < myData.Count; i++)
            {
                double pr = myData.Procent[i] * 100;
                dataGridView1.Rows.Add(myData[i].Name, myData[i].Vote, pr.ToString("F2") + "%");
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2) UpdateTable();
            if (tabControl1.SelectedTab == tabPage3) myData.Diagram(chart1);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            //привет
        }
    }
}