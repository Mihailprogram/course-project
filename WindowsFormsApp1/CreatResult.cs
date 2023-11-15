using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class CreatResult : Form
    {
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public int idsportsman;
        public CreatResult()
        {
            InitializeComponent();
        }

        private void CreatResult_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.BackgroundColor = Color.White;

            cnct.Open();
            try
            {
                DateTime currentDate = DateTime.Now;

                
                string sql =  "SELECT * FROM sportclub.get_result(@id,@date)";
                        NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                        cmd.Parameters.AddWithValue("@id", idsportsman);
                cmd.Parameters.Add(new NpgsqlParameter("@date", NpgsqlDbType.Date));
                cmd.Parameters["@date"].Value = currentDate;

                        NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                            DataSet ds = new DataSet();
                            adapter.Fill(ds);
                            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                            {
                                dataGridView1.DataSource = ds.Tables[0];

                                // Скрываем ненужные столбцы
                                dataGridView1.Columns["id_achievments"].Visible = false;
                                dataGridView1.Columns["id_sportsman"].Visible = false;

                                // Изменяем заголовки столбцов
                                dataGridView1.Columns[0].HeaderText = "Название соревнования";
                                dataGridView1.Columns[2].HeaderText = "Дата проведения";
                            }
                            else
                            {
                                label1.Text = "Нет доступных соревнований";
                                label2.Visible = false;
                                dataGridView1.Visible = false;
                                btaddres.Visible = false;
                                label3.Visible = false;
                                textBox1.Visible = false;
                            }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er" + ex.Message);
            }

        }

        private void btaddres_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int reslult = Convert.ToInt32(textBox1.Text);
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int idsp = Convert.ToInt32(selectedRow.Cells["id_sportsman"].Value);
                int id_achievments = Convert.ToInt32(selectedRow.Cells["id_achievments"].Value);

                string sql = "UPDATE sportclub.sport_competition_sportsman SET rusult = @rusult WHERE id_sportsman = @id_sportsman and id_achievments = @id_achievments";

                NpgsqlCommand cmd = new NpgsqlCommand(sql,cnct);
                cmd.Parameters.AddWithValue("@rusult", reslult);
                cmd.Parameters.AddWithValue("@id_sportsman", idsp);
                cmd.Parameters.AddWithValue("@id_achievments", id_achievments);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Успешно");
                this.Close();

            }
            else
            {
                
                MessageBox.Show("Нужно выделить полностью строку");
                
            }
        }
    }
}
