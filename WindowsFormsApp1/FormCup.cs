using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
namespace WindowsFormsApp1
{
    public partial class FormCup : Form
    {
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public FormCup()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime date = dateTimePicker1.Value;
            string name = textBox1.Text;
            string selectedSport = comboBoxSport.Text;
            try
            {
                string sqlid = "SELECT id_sport_type FROM sportclub.type_sport WHERE sport_type=@sport_type";

                NpgsqlCommand cmdid = new NpgsqlCommand(sqlid, cnct);
                cmdid.Parameters.AddWithValue("@sport_type", selectedSport);
                object res = cmdid.ExecuteScalar();
                int id_sport = Convert.ToInt32(res);

                string sql = "INSERT INTO sportclub.sport_competition(name_competition,date,id_type_sport) VALUES(@name,@date,@id_type_sport)";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@id_type_sport", id_sport  );

                cmd.ExecuteNonQuery();
                MessageBox.Show("Успешно");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void FormCup_Load(object sender, EventArgs e)
        {
            cnct.Open();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            

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
    }
}
