using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DictionaryApp
{
    partial class AboutBox1 : Form
    {
        public AboutBox1()
        {
            InitializeComponent();

            // Прямое назначение текста (игнорируем AssemblyInfo)
            this.Text = "О программе DictionaryApp"; // Заголовок окна
            this.labelProductName.Text = "Словарь «Слововед»";
            this.labelVersion.Text = "Версия уже не помню какая...";
            this.labelCopyright.Text = "Copyright © 2026 Смелянская Вера";
            this.labelCompanyName.Text = "ООО тмыв денег";

            // Вставляем подробное описание с сохранением форматирования
            this.textBoxDescription.Text = @"Общее описание:
Программа представляет собой высокопроизводительный инструмент для работы с большими текстовыми базами данных (словарями). Приложение позволяет загружать, редактировать и сохранять файлы слов, а также выполнять расширенный поиск с использованием современных алгоритмов обработки текста.   

Основные возможности:
— Работа с огромными данными: Оптимизированная загрузка словарей объемом более 1.5 миллионов слов.
— Управление файлами: Полный цикл работы через проводник — создание новых списков, открытие существующих файлов (.txt) и сохранение изменений.
— Интеллектуальный поиск: Поддержка трех различных режимов фильтрации данных.

Подробная инструкция по использованию:
1. Начало работы (Меню «Словарь»)
Открыть: Используйте этот пункт для выбора текстового файла. После загрузки в строке состояния отобразится общее количество слов.
Создать новый: Очищает базу данных для создания списка «с нуля».
Сохранить как: Экспорт текущего списка в новый файл.

2. Управление словами (Главная панель)
Добавить: Введите слово и нажмите кнопку «Добавить».
Удалить: Выделите слово в списке и нажмите «Удалить».
Найти (Кнопка): Мгновенный точный поиск слова.

3. Режимы расширенного поиска (Меню «Работа со словарем»)
Поиск (Левенштейна): Находит слова с опечатками (например, «малако» -> «молоко»).
Нечеткий поиск (Вариант 4): Фильтр по длине и части слова.
Сохранить результат поиска: Выгрузка найденных слов в отдельный файл.

Технические особенности для защиты:
— Алгоритм Левенштейна: поиск с пределом в 3 правки.
— Оптимизация UI: отображение не более 1000 первых результатов для скорости.

Автор: Студентка Смелянская Вера.";
        }

        #region Методы доступа к атрибутам сборки

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void tableLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
