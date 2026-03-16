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
        private string base64Avatar = ""; // Переменная для хранения фото в виде строки

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

        // --- 1. ВЫБРАТЬ ФОТО (button4) ---
        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Изображения|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Отображаем на форме
                    pictureBox1.Image = Image.FromFile(ofd.FileName);

                    // Превращаем в строку для базы данных
                    byte[] imageBytes = File.ReadAllBytes(ofd.FileName);
                    base64Avatar = Convert.ToBase64String(imageBytes);

                    MessageBox.Show("Фото успешно выбрано!");
                }
            }
        }

        // --- 2. РЕГИСТРАЦИЯ (button1) ---
        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Введите логин и пароль для регистрации.");
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
                MessageBox.Show($"Пользователь {textBox1.Text} успешно добавлен в базу!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка регистрации: {ex.Message}");
            }
        }

        // --- 3. ВОЙТИ (button2) ---
        private async void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Введите логин и пароль для входа.");
                return;
            }

            try
            {
                // Ищем пользователя, у которого совпадают и логин, и пароль
                var result = await supabase
                    .From<User>()
                    .Where(u => u.Name == textBox1.Text)
                    .Where(u => u.Password == textBox2.Text)
                    .Get();

                var user = result.Models.FirstOrDefault();

                if (user != null)
                {
                    MessageBox.Show($"Авторизация успешна! Здравствуйте, {user.Name}.");

                    // Если в базе есть фото — загружаем его в pictureBox1
                    if (!string.IsNullOrEmpty(user.Avatar))
                    {
                        byte[] imageBytes = Convert.FromBase64String(user.Avatar);
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
                MessageBox.Show($"Ошибка при входе: {ex.Message}");
            }
        }

        // --- 4. ОЧИСТИТЬ (button3) ---
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            pictureBox1.Image = null;
            base64Avatar = "";
            MessageBox.Show("Форма успешно очищена.");
        }

        // Класс модели (по твоей таблице на скриншоте)
        [Table("users")]
        class User : BaseModel
        {
            [PrimaryKey("id", false)]
            public int Id { get; set; }

            [Column("username")]
            public string Name { get; set; }

            [Column("password")]
            public string Password { get; set; }

            [Column("avatar")]
            public string Avatar { get; set; }
        }
    }
}