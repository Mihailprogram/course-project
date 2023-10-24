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

namespace WindowsFormsApp1
{
    public partial class FormTraning : Form
    {
        public FormTraning()
        {
            InitializeComponent();
        }
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public int idsport;
        int idtrener = 0;
        private void FormTraning_Load(object sender, EventArgs e)
        {
            cnct.Open();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT id_coach,name,last_name FROM sportclub.coach WHERE id_sport_type=@idsport", cnct);
                cmd.Parameters.AddWithValue("@idsport", idsport);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        idtrener = Convert.ToInt32 (reader["id_coach"]);
                        string name = reader["name"].ToString();
                        string last_name = reader["last_name"].ToString();
                        string new_str = name + " " + last_name;
                        // Добавляем виды спорта из таблицы в ComboBox
                        comboBoxTrener.Items.Add(new_str);

                        //comboBox1.Items.Add(reader["pickuppointstreet"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message);
            }
        }
    }
}
