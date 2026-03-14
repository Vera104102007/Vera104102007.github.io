using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int a = int.Parse(textBox1.Text);
                int b = int.Parse(textBox2.Text);

                List<int> allMaxSumNumbers = NaturalNumbersLogic.GetMaxSumDivisorsList(a, b);
                var result = NaturalNumbersLogic.FindTwoClosest(allMaxSumNumbers);

                if (result.HasValue)
                    label3.Text = $"Ближайшие с макс. суммой: {result.Value.Item1} и {result.Value.Item2}";
                else if (allMaxSumNumbers.Count == 1)
                    label3.Text = $"Найдено только одно число: {allMaxSumNumbers[0]}";
                else
                    label3.Text = "Числа не найдены.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int a = int.Parse(textBox1.Text);
                int b = int.Parse(textBox2.Text);
                List<int> perfects = new List<int>();

                for (int i = a; i <= b; i++)
                {
                    if (NaturalNumbersLogic.IsPerfect(i)) perfects.Add(i);
                }

                var result = NaturalNumbersLogic.FindTwoClosest(perfects);

                if (result.HasValue)
                    label3.Text = $"Ближайшие совершенные: {result.Value.Item1} и {result.Value.Item2}";
                else
                    label3.Text = "Пара совершенных чисел не найдена.";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}