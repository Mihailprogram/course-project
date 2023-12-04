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
    public partial class Allspcomp : Form
    {
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public Allspcomp()
        {
            InitializeComponent();
        }

        private void Allspcomp_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            dataGridView1.BackgroundColor = Color.White;

            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = cnct; // Используйте существующее подключение

                try
                {
                    cnct.Open();

                    // SQL-запрос для выборки видов спорта из таблицы type_sport
                    cmd.CommandText = "SELECT name_competition FROM sportclub.sport_competition";

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Добавляем виды спорта из таблицы в ComboBox
                            name_comp.Items.Add(reader["name_competition"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке видов спорта: " + ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string namecomp = name_comp.Text;

            NpgsqlCommand cmds = new NpgsqlCommand("SELECT * FROM sportclub.select_allsportsman(@namecomp)", cnct);
            cmds.Parameters.AddWithValue("@namecomp", namecomp);

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmds);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].HeaderText = "Имя";

            // Измените название второго столбца на "Фамилия"
            dataGridView1.Columns[1].HeaderText = "Фамилия";
            dataGridView1.Columns[2 ].HeaderText = "Возраст";

        }

        private void name_comp_SelectedIndexChanged(object sender, EventArgs e)
        {
            name_comp.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}
