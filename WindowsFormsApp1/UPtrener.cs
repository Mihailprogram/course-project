using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class UPtrener : Form
    {
        public UPtrener()
        {
            InitializeComponent();
        }
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public int ID;
        public void SetData(string data1, string data2,
            string data3, string data4,string data5,
            int id)
        {
            textBoxFirstName.Text = data1;
            textBoxLastName.Text = data2;
            dateTimePicker1.Text = data3;
            comboBoxSport.Text = data4;
            textBox1.Text = data5;
            ID = id;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBoxFirstName.Text;
            string last_name = textBoxLastName.Text;
            string type_sport = comboBoxSport.Text;
            int expir = Convert.ToInt32(textBox1.Text);
            DateTime birthDate = dateTimePicker1.Value;
            DateTime currentDate = DateTime.Today;

            int age = currentDate.Year - birthDate.Year;

            if (currentDate.Month < birthDate.Month || (currentDate.Month == birthDate.Month && currentDate.Day < birthDate.Day))
            {
                age--;
            }

            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT id_sport_type FROM sportclub.type_sport WHERE sport_type=@type_sport", cnct);
                cmd.Parameters.AddWithValue("@type_sport", type_sport);
                object result = cmd.ExecuteScalar();
                int id_sport_type = Convert.ToInt32(result);

                NpgsqlCommand cmdup = new NpgsqlCommand("UPDATE sportclub.coach SET age = @age,name = @name,last_name = @last_name, id_sport_type = @id_sport_type, date_of_birth = @birthDate,experience=@expir WHERE id_coach = @ID", cnct);
                cmdup.Parameters.AddWithValue("@name", name);
                cmdup.Parameters.AddWithValue("@age", age);
                cmdup.Parameters.AddWithValue("@last_name", last_name);
                cmdup.Parameters.AddWithValue("@age", age);
                cmdup.Parameters.AddWithValue("@id_sport_type", id_sport_type);
                cmdup.Parameters.AddWithValue("@birthDate", birthDate);
                cmdup.Parameters.AddWithValue("@expir", expir);
                cmdup.Parameters.AddWithValue("@ID", ID);
                cmdup.ExecuteNonQuery();
                MessageBox.Show("Успешно");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке видов спорта: " + ex.Message);

            }
        }

        private void UPtrener_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            cnct.Open();
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = cnct; // Используйте существующее подключение

                try
                {


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

        private void comboBoxSport_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxSport.DropDownStyle = ComboBoxStyle.DropDownList;

        }
    }
}
