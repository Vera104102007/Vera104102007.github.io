using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Name
{
    public partial class FormSettings : Form
    {
        // В конструктор формы или при загрузке передаем текущее значение
        public FormSettings(int currentInterval)
        {
            InitializeComponent();
            numericUpDown1.Value = currentInterval;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сохраняем значение в наш общий класс данных
            GameState.Interval = (int)numericUpDown1.Value;
            this.Close();
        }
    }
}