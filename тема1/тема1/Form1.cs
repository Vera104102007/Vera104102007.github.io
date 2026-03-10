using System;
using System.Linq;
using System.Windows.Forms;

namespace project1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обработчик KeyPress для textBox1 (ввод x)
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обработчик KeyPress для textBox2 (ввод точности)
        /// </summary>
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',' && (sender as TextBox).Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Обработчик TextChanged для textBox1
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !string.IsNullOrWhiteSpace(textBox1.Text) &&
                              !string.IsNullOrWhiteSpace(textBox2.Text);
        }

        /// <summary>
        /// Обработчик TextChanged для textBox2
        /// </summary>
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !string.IsNullOrWhiteSpace(textBox1.Text) &&
                              !string.IsNullOrWhiteSpace(textBox2.Text);
        }

        /// <summary>
        /// Обработчик Click для button1 - вычисление sin(x) через ряд
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = ""; // Очистка сообщения об ошибке

            try
            {
                // Ввод данных
                double x = Convert.ToDouble(textBox1.Text);
                double eps = Convert.ToDouble(textBox2.Text);

                // Проверка точности
                if (eps <= 0)
                {
                    label6.Text = "Ошибка: Точность должна быть положительным числом";
                    return;
                }

                // Эталонное значение для проверки
                double mathResult = Math.Sin(x);

                // Вычисляем сумму в скобках S = 1 - x²/3! + x⁴/5! - x⁶/7! + ...
                double sumInBrackets = 0.0;    // Сумма в скобках
                double term = 1.0;              // Первый член в скобках (1)
                int n = 0;                       // Индекс члена (n=0 для первого члена 1)
                int sign = 1;                     // Знак (+1 или -1)
                int count = 0;                     // Счетчик членов в скобках

                // Вычисляем члены ряда в скобках, пока текущий член больше точности
                do
                {
                    // Добавляем текущий член с соответствующим знаком
                    sumInBrackets += sign * term;
                    count++;

                    // Вычисляем следующий член: term(n+1) = term(n) * x² / ((2n+2)*(2n+3))
                    // Для n=0: term1 = 1 * x² / (2*3) = x²/6 (это x²/3! = x²/6)
                    // Для n=1: term2 = (x²/6) * x² / (4*5) = x⁴/120 (это x⁴/5!)
                    double nextTerm = term * (x * x) / ((2 * n + 2) * (2 * n + 3));

                    term = nextTerm;
                    n++;
                    sign = -sign; // Меняем знак для следующего члена

                } while (Math.Abs(term) > eps);

                // Умножаем сумму в скобках на x
                double seriesResult = x * sumInBrackets;

                // Вывод результатов
                label3.Text = $"Math.Sin({x:F3}) = {mathResult:F8}";
                label4.Text = $"Сумма ряда = {seriesResult:F8}";
                label5.Text = $"Количество членов = {count}";
            }
            catch (FormatException)
            {
                label6.Text = "Ошибка: Используйте запятую для дробных чисел";
            }
            catch (Exception ex)
            {
                label6.Text = $"Ошибка: {ex.Message}";
            }
        }
    }
}