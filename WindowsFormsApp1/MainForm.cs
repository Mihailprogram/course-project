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
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WindowsFormsApp1
{
    public partial class MainForm : Form
    {
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }
        public int userId;
        int idtypesport = 0;
        public void HandleUserLogin(int userID)
        {
            string name = "";
            string lastName = "";
            string typesport = "";
            
            try
            {
                cnct.Open();
                string sql = "SELECT id_sport_type,name,last_name FROM sportclub.sportsman " +
                        "WHERE userid = @userid";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                cmd.Parameters.AddWithValue("@userid", userID);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = reader["name"].ToString();
                    lastName = reader["last_name"].ToString();
                    idtypesport = Convert.ToInt32(reader["id_sport_type"]);
                }

                reader.Close();
                string sqlsport = "SELECT sport_type FROM sportclub.type_sport " +
                    "WHERE id_sport_type = @idtypesport";

                NpgsqlCommand cmdsp = new NpgsqlCommand(sqlsport, cnct);
                cmdsp.Parameters.AddWithValue("@idtypesport", idtypesport);
                NpgsqlDataReader reader1 = cmdsp.ExecuteReader();
                while (reader1.Read())
                {
                    typesport = reader1["sport_type"].ToString();
                }
                reader1.Close();
                label1.Text = $"{name} {lastName}!";
                label2.Text = $"Ваш вид спорта {typesport}";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка!!: " + ex.Message);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            FormTraning formTraning = new FormTraning();
            formTraning.idsport = idtypesport;
            formTraning.Show();
            this.Hide();
        }
    }
}
