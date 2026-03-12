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
        /// Обработчик KeyPress для textBox1 (ввод x) - разрешен минус
        /// </summary>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Разрешаем: цифры, Backspace, запятую, точку и минус
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != ',' && e.KeyChar != '.' && e.KeyChar != '-')
            {
                e.Handled = true;
            }

            // Замена точки на запятую для корректного Double.Parse
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            // Запрет второй запятой
            if (e.KeyChar == ',' && tb.Text.Contains(','))
            {
                e.Handled = true;
            }

            // Логика для минуса: только в начале и только один
            if (e.KeyChar == '-')
            {
                if (tb.SelectionStart != 0 || tb.Text.Contains('-'))
                {
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Обработчик KeyPress для textBox2 (ввод точности) - минус запрещен
        /// </summary>
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                e.KeyChar != ',' && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            if (e.KeyChar == ',' && tb.Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !string.IsNullOrWhiteSpace(textBox1.Text) &&
                              !string.IsNullOrWhiteSpace(textBox2.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = !string.IsNullOrWhiteSpace(textBox1.Text) &&
                              !string.IsNullOrWhiteSpace(textBox2.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label6.Text = "";

            try
            {
                double x = Convert.ToDouble(textBox1.Text);
                double eps = Convert.ToDouble(textBox2.Text);

                //Проверка диапазона вводимых данных
                if (x < -100 || x > 100)
                {
                    label6.Text = "Ошибка: Введите X в диапазоне от -100 до 100";
                    return;
                }

                if (eps <= 0 || eps >= 1)
                {
                    label6.Text = "Ошибка: Точность должна быть в интервале (0; 1)";
                    return;
                }

                double mathResult = Math.Sin(x);

                // Сумма ряда Тейлора для sin(x)
                double sumInBrackets = 0.0;
                double term = 1.0;
                int n = 0;
                int sign = 1;
                int count = 0;

                do
                {
                    sumInBrackets += sign * term;
                    count++;

                    // Рекуррентная формула для следующего члена
                    double nextTerm = term * (x * x) / ((2 * n + 2) * (2 * n + 3));

                    term = nextTerm;
                    n++;
                    sign = -sign;

                } while (Math.Abs(term) > eps);

                double seriesResult = x * sumInBrackets;

                // Вывод результатов
                label3.Text = $"Math.Sin({x:F3}) = {mathResult:F8}";
                label4.Text = $"Сумма ряда = {seriesResult:F8}";
                label5.Text = $"Количество членов = {count}";
            }
            catch (FormatException)
            {
                label6.Text = "Ошибка: Некорректный формат числа";
            }
            catch (Exception ex)
            {
                label6.Text = $"Ошибка: {ex.Message}";
            }
        }
    }
}