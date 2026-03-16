using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace тема1
{
    public partial class Form2 : Form
    {
        private Supabase.Client supabase;
        private string base64Avatar = ""; // Здесь будет храниться строка фото

        public Form2()
        {
            InitializeComponent();
            InitSupabase();
        }

        private async void InitSupabase()
        {
            var url = "https://tvgsepihkilwockknlmu.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InR2Z3NlcGloa2lsd29ja2tubG11Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NzM2NDQ2NTAsImV4cCI6MjA4OTIyMDY1MH0.N0_IdxnIMhec9eExRjdae7jUVjEUQgcVtorWloUE3WE";

            try
            {
                var options = new Supabase.SupabaseOptions { AutoConnectRealtime = false };
                supabase = new Supabase.Client(url, key, options);
                await supabase.InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
        }

        // --- КНОПКА: ВЫБРАТЬ ФОТО (button4) ---
        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);

                    // Переводим картинку в строку (Base64), чтобы сохранить в БД
                    byte[] imageBytes = File.ReadAllBytes(ofd.FileName);
                    base64Avatar = Convert.ToBase64String(imageBytes);

                    MessageBox.Show("Фото успешно загружено в программу!");
                }
            }
        }

        // --- КНОПКА: РЕГИСТРАЦИЯ (button1) ---
        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            try
            {
                var newUser = new User
                {
                    Name = textBox1.Text,
                    Password = textBox2.Text,
                    Avatar = base64Avatar
                };

                await supabase.From<User>().Insert(newUser);
                MessageBox.Show($"Пользователь {textBox1.Text} успешно зарегистрирован!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }

        // --- КНОПКА: ВОЙТИ (button2) ---
        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Ищем в базе по логину и паролю
                var result = await supabase
                    .From<User>()
                    .Where(x => x.Name == textBox1.Text)
                    .Where(x => x.Password == textBox2.Text)
                    .Get();

                var foundUser = result.Models.FirstOrDefault();

                if (foundUser != null)
                {
                    MessageBox.Show($"Успешный вход! Рады видеть вас, {foundUser.Name}");

                    // Если у пользователя есть аватарка в базе, выводим её
                    if (!string.IsNullOrEmpty(foundUser.Avatar))
                    {
                        byte[] imageBytes = Convert.FromBase64String(foundUser.Avatar);
                        using (var ms = new MemoryStream(imageBytes))
                        {
                            pictureBox1.Image = Image.FromStream(ms);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка: Пользователь не найден или пароль неверный.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка входа: {ex.Message}");
            }
        }

        // --- КНОПКА: ОЧИСТИТЬ (button3) ---
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            pictureBox1.Image = null;
            base64Avatar = "";
            MessageBox.Show("Поля формы очищены.");
        }

        [Table("users")]
        class User : BaseModel
        {
            [PrimaryKey("id", false)]
            public int Id { get; set; }

            [Column("username")]
            public string Name { get; set; }

            [Column("password")]
            public string Password { get; set; }

            [Column("avatar")] // Тот самый столбец, который нужно добавить в БД
            public string Avatar { get; set; }
        }
    }
}