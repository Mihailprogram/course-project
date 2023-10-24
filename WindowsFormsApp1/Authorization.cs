using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Npgsql;

namespace WindowsFormsApp1
{
    public partial class Authorization : Form
    {
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public Authorization()
        {
            InitializeComponent();
        }
        private void Authorization_Load(object sender, EventArgs e)
        {
            try
            {
                if (cnct.State == ConnectionState.Closed)
                    cnct.Open();


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
                MainForm mainForm = new MainForm();




                if (count > 0)
                {

                   UserLoggedIn += (userId) => {
                        mainForm.HandleUserLogin(userId);
                        mainForm.userId= userId;
                        this.Hide();
                        mainForm.Show();
                    };
                    UserLoggedIn?.Invoke(count);

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
    }
}
