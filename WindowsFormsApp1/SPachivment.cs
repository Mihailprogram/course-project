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
    public partial class SPachivment : Form
    {
        public int id_sportsman;
        public SPachivment()
        {
            InitializeComponent();
        }
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        private void SPachivment_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.BackgroundColor = Color.White;
            try
            {
                NpgsqlCommand cmd1 = new NpgsqlCommand("SELECT * FROM sportclub.get_result_sportsman(@id_sportsman)", cnct);
                cmd1.Parameters.AddWithValue("@id_sportsman", id_sportsman);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd1);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Columns[0].HeaderText = "Название соревнования";
                    dataGridView1.Columns[1].HeaderText = "Дата проведения";
                    dataGridView1.Columns[2].HeaderText = "Результат соревнования(Место)";
                }
                else
                {
                    label2.Text = "У вас пока нет достижений";
                    dataGridView1.Visible = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("ERR");
            }
        }
    }
}
