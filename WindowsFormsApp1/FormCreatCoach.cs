using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormCreatCoach : Form
    {
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public FormCreatCoach()
        {
            InitializeComponent();
            comboBoxSport.DropDownStyle = ComboBoxStyle.DropDownList;
           // this.FormClosing += new FormClosingEventHandler(FormCreatCoach_FormClosing);
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Text;
            string lastName = textBoxLastName.Text;
            string firstName = textBoxFirstName.Text;
            string selectedSport = comboBoxSport.Text;
            int experience = Convert.ToInt32(textBoxexp.Text);
            DateTime birthDate = dateTimePicker1.Value;
            DateTime currentDate = DateTime.Today;

            int age = currentDate.Year - birthDate.Year;

            if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
            {
                age--;
            }
            try
            {

                // Вставка данных в таблицу "user"
                string insertUserSql = "INSERT INTO sportclub.user " +
                    "(login, password, roleid) VALUES (@login, @password, 2) RETURNING iduser";
                using (NpgsqlCommand cmd = new NpgsqlCommand(insertUserSql, cnct))
                {
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@password", password);

                    // Получение ID только что созданного пользователя
                    int userId = (int)cmd.ExecuteScalar();


                    // Вставка данных в таблицу "sportman"

                    string ins = "SELECT sportclub.insert_coach(@firstName,@age,@experience,@selectedSport,@lastName,@birthdate,@userId)";
                    using (NpgsqlCommand sportmanCmd = new NpgsqlCommand(ins, cnct))
                    {

                        sportmanCmd.Parameters.Add(new NpgsqlParameter("@age", NpgsqlDbType.Integer)).Value = age;
                        sportmanCmd.Parameters.Add(new NpgsqlParameter("@selectedSport", NpgsqlDbType.Varchar)).Value = selectedSport;
                        sportmanCmd.Parameters.Add(new NpgsqlParameter("@firstName", NpgsqlDbType.Varchar)).Value = firstName;
                        sportmanCmd.Parameters.Add(new NpgsqlParameter("@lastName", NpgsqlDbType.Varchar)).Value = lastName;
                        sportmanCmd.Parameters.Add(new NpgsqlParameter("@birthdate", NpgsqlDbType.Date)).Value = birthDate;
                        sportmanCmd.Parameters.Add(new NpgsqlParameter("@userId", NpgsqlDbType.Integer)).Value = userId;
                        sportmanCmd.Parameters.Add(new NpgsqlParameter("@experience", NpgsqlDbType.Integer)).Value = experience;
                        sportmanCmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Данные успешно добавлены в базу данных.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении данных: " + ex.Message);
            }

            // Отобразите форму Registration

            this.Close();
            cnct.Close();
            Application.OpenForms["FormAdmin"].Show();

        }

        private void FormCreatCoach_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = cnct; // Используйте существующее подключение

                try
                {
                    cnct.Open();

                    // SQL-запрос для выборки видов спорта из таблицы type_sport
                    cmd.CommandText = "SELECT sport_type FROM sportclub.type_sport";

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Добавляем виды спорта из таблицы в ComboBox
                            comboBoxSport.Items.Add(reader["sport_type"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке видов спорта: " + ex.Message);
                }
            }
        }

        private void FormCreatCoach_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Здесь вы можете добавить проверки или действия перед закрытием формы
            DialogResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Подтверждение", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                // Если пользователь нажал "Нет", отменим закрытие формы
                e.Cancel = true;
            }
            else
            {
                Application.OpenForms["Authorization"].Close();
            }
        }
        private void textBoxPassword_TextChanged(object sender, EventArgs e)
        {
            textBoxPassword.PasswordChar = '•';
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBoxPassword.PasswordChar = '\0'; // Отключаем замену символов
            }
            else
            {
                textBoxPassword.PasswordChar = '•'; // Включаем замену символов
            }
        }
    }
}
