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
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public Authorization()
        {
            InitializeComponent();
            RoundButtonCorners(button1, 40);

            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;
        }

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
        public void ClearTextFields()
        {
            // Очищаем текстовые поля
            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
        }
        private void Authorization_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

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

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            try
            {
                // ExecuteReader(): 
                string sql = "SELECT iduser FROM sportclub.user " +
                    "WHERE login = @login and password = @password";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", password);

                object result = cmd.ExecuteScalar();
                int count = Convert.ToInt32(result);
                //
                string sqlrole = "SELECT roleid FROM sportclub.user " +
                    "WHERE login = @login and password = @password";
                NpgsqlCommand cmdrole = new NpgsqlCommand(sqlrole, cnct);
                cmdrole.Parameters.AddWithValue("@login", login);
                cmdrole.Parameters.AddWithValue("@password", password);

                object resultrole = cmdrole.ExecuteScalar();
                int role = Convert.ToInt32(resultrole);

                MainForm mainForm = new MainForm();

                


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
                    // Пользователь не найден или пароль не совпадает
                    MessageBox.Show("Неверное имя пользователя или пароль.");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
           

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '•';
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Registration registrationForm = new Registration();
            // Отобразите форму Registration
            this.Hide();


            
            registrationForm.Show();
            


        }
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            try
            {
                // ExecuteReader(): 
                string sql = "SELECT iduser FROM sportclub.user " +
                    "WHERE login = @login and password = @password";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                cmd.Parameters.AddWithValue("@login", login);
                cmd.Parameters.AddWithValue("@password", password);

                object result = cmd.ExecuteScalar();
                int count = Convert.ToInt32(result);
                //
                string sqlrole = "SELECT roleid FROM sportclub.user " +
                    "WHERE login = @login and password = @password";
                NpgsqlCommand cmdrole = new NpgsqlCommand(sqlrole, cnct);
                cmdrole.Parameters.AddWithValue("@login", login);
                cmdrole.Parameters.AddWithValue("@password", password);

                object resultrole = cmdrole.ExecuteScalar();
                int role = Convert.ToInt32(resultrole);

                MainForm mainForm = new MainForm();




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
                    // Пользователь не найден или пароль не совпадает
                    MessageBox.Show("Неверное имя пользователя или пароль.");
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }
    }
}
