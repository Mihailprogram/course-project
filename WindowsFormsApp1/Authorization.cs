using System;
using MaterialSkin.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Drawing;

using Npgsql;
using MaterialSkin;
using System.Drawing.Drawing2D;

namespace WindowsFormsApp1
{

    public partial class Authorization : Form
    {
        private int failedAttempts = 0;
        private Random random = new Random();
        private int captchaValue;
        private PictureBox pictureBoxCaptcha;

        // Строка подключения к базе данных PostgreSQL
        static String connection = Properties.Settings.Default.pgConnection;
        // Подключение к базе данных PostgreSQL
        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public Authorization()
        {
            InitializeComponent();
            buttonauht.FlatStyle = FlatStyle.Flat;
            buttonauht.FlatAppearance.BorderSize = 0;

            RoundButtonCorners(buttonauht as Button, 40);


            pictureBoxCaptcha = new PictureBox();
            pictureBoxCaptcha.Width = 100;  
            pictureBoxCaptcha.Height = 100; 
            pictureBoxCaptcha.Location = new Point(100, 100); // установите координаты x и y
            Controls.Add(pictureBoxCaptcha);
            pictureBoxCaptcha.Visible = false;
            textBoxCaptcha.Visible = false;


            // Установите регион для кнопки buttonauht после ее инициализации
            LoadCaptcha();
;
        }
        int num = 0;
        // Метод загрузки CAPTCHA
        private void LoadCaptcha()
        {
            captchaValue = random.Next(100, 999);
            Bitmap img = new Bitmap(pictureBoxCaptcha.Width, pictureBoxCaptcha.Height);
            Font font = new Font("Arial", 30, FontStyle.Bold, GraphicsUnit.Pixel);
            Graphics graphics = Graphics.FromImage(img);
            graphics.DrawString(captchaValue.ToString(), font, Brushes.Red, new Point(0, 0));
            pictureBoxCaptcha.Image = img;
        }


        // Метод для закругления углов кнопки
        private void RoundButtonCorners(Control control, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(control.ClientRectangle.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(control.ClientRectangle.Width - cornerRadius, control.ClientRectangle.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(0, control.ClientRectangle.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        // Метод для очистки текстовых полей
        public void ClearTextFields()
        {
            /// Очищаем текстовые поля
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }
        // Метод загрузки формы
        private void Authorization_Load(object sender, EventArgs e)
        {


            try
            {
                if (cnct.State == ConnectionState.Closed)
                    cnct.Open();

                DateTime currentDate = DateTime.Now;
                string delsql = "DELETE FROM sportclub.injury WHERE end_date < @date";
                NpgsqlCommand cmddel = new NpgsqlCommand(delsql, cnct);
                cmddel.Parameters.AddWithValue("@date", currentDate);
                cmddel.ExecuteNonQuery();


            }
            catch (Exception ex)


            {
                MessageBox.Show(ex.Message);

            }
            
        }
        public event Action<int> UserLoggedIn;


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '•';
        }
        // Обработчик нажатия кнопки "Зарегистрироваться"
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration registrationForm = new Registration();
            // Отобразите форму Registration
            this.Hide();


            
            registrationForm.Show();
            


        }

        // Метод для закрытия всех окон приложения
        private void CloseALL()
        {
            foreach(Form form in Application.OpenForms)
            {
                form.Close();
            }
        }
        private void Authorization_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        // Обработчик нажатия кнопки "Войти"
        private void buttonauht_Click_1(object sender, EventArgs e)
        {
            
           

           
                try
                {
                    bool flagis = true;
                    string login = textBox1.Text;
                    string password = textBox2.Text;
               
                        string sql = "SELECT iduser FROM sportclub.user " +
                            "WHERE login = @login and password = @password";
                        NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);

                        object result = cmd.ExecuteScalar();
                        int count = Convert.ToInt32(result);
                   
                        string sqlrole = "SELECT roleid FROM sportclub.user " +
                            "WHERE login = @login and password = @password";
                        NpgsqlCommand cmdrole = new NpgsqlCommand(sqlrole, cnct);
                        cmdrole.Parameters.AddWithValue("@login", login);
                        cmdrole.Parameters.AddWithValue("@password", password);

                        object resultrole = cmdrole.ExecuteScalar();
                        int role = Convert.ToInt32(resultrole);

                        MainForm mainForm = new MainForm();


                    if (failedAttempts >= 3)
                    {

                        pictureBoxCaptcha.Visible = true;
                        textBoxCaptcha.Visible = true;

                        textBox1.Visible = false;
                        textBox2.Visible = false;
                   
                        linkLabel1.Visible = false;
                        label1.Visible = false;
                        label2.Text = "Введите \n Капчу";
                         flagis = false;
                        // Проверка CAPTCHA
                        int captchaAnswer;
                        if (!string.IsNullOrWhiteSpace(textBoxCaptcha.Text))
                        {
                            captchaAnswer = int.Parse(textBoxCaptcha.Text);



                            if (captchaAnswer != captchaValue)
                            {
                                MessageBox.Show("Неверная CAPTCHA. Пожалуйста, попробуйте еще раз.");
                                LoadCaptcha();
                                textBoxCaptcha.Clear();
                                return;
                            }
                            else
                            {
                                failedAttempts = 0;
                                textBoxCaptcha.Clear();
                                textBox1.Visible = true;
                                textBox2.Visible = true;
                                linkLabel1.Visible = true;
                                label1.Visible = true;
                                label2.Text = "Пароль";
                                pictureBoxCaptcha.Visible = false;
                                textBoxCaptcha.Visible = false;
                            }
                        }
                    }
                    failedAttempts += 1;
                

                        if (count > 0)
                        {
                    
                            if (role == 1)
                            {
                                UserLoggedIn += (userId) =>
                                {
                                    mainForm.HandleUserLogin(userId);
                                    mainForm.userId = userId;
                                    this.Hide();
                                    mainForm.Show();
                                };
                                UserLoggedIn?.Invoke(count);
                            }
                            if (role == 2)
                            {
                                FormCoach formCoach = new FormCoach();
                                formCoach.userID = count;
                                this.Hide();
                                formCoach.Show();

                            }
                            if (role == 3)
                            {
                                FormAdmin formAdmin = new FormAdmin();
                                this.Hide();
                                formAdmin.Show();
                            }
                        }
                    else 
                    {
                        if (flagis == true)
                        {
                            MessageBox.Show("Неверное имя пользователя или пароль");
                        }
                        
                    }
              


                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message);
                }
           
            

        }
    }
}
