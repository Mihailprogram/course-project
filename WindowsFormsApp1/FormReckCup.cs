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
    public partial class FormReckCup : Form
    {
        public FormReckCup()
        {
            InitializeComponent();
        }
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public int idspotsman;
        public int idcoach;
        public int typecup;

        private void FormReckCup_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            cnct.Open();

            string sql = "SELECT COUNT(*) FROM sportclub.sport_competition WHERE id_type_sport = @id_type_sport";
            NpgsqlCommand cmds = new NpgsqlCommand(sql, cnct);
            cmds.Parameters.AddWithValue("@id_type_sport", typecup);
            object resul = cmds.ExecuteScalar();
            int count = Convert.ToInt32(resul);
            if (count > 0)
            {
                dataGridView1.ReadOnly = true;

                dataGridView1.BackgroundColor = Color.White;
                try
                {
                    NpgsqlCommand cmd = new NpgsqlCommand("SELECT name_competition,date,id_achievments FROM sportclub.sport_competition WHERE id_type_sport = @id_type_sport", cnct);
                    cmd.Parameters.AddWithValue("@id_type_sport", typecup);
                    NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Columns["id_achievments"].Visible = false;
                    dataGridView1.Columns[0].HeaderText = "Название соревнования";
                    dataGridView1.Columns[1].HeaderText = "Дата";



                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка");
                }
            }
            else
            {
                button1.Visible = false;
                dataGridView1.Visible = false;
                label2.Text = "По этому виду спорта нет соревнований";
                label2.TextAlign = ContentAlignment.TopCenter;
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try { 
                    DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                
                    int id = Convert.ToInt32(selectedRow.Cells["id_achievments"].Value);

                    string sql = "INSERT INTO sportclub.sport_competition_sportsman(id_sportsman,id_achievments,id_coach) VALUES(@id_sportsman,@id_achievments,@id_coach)";

                    NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                    cmd.Parameters.AddWithValue("@id_sportsman", idspotsman);
                    cmd.Parameters.AddWithValue("@id_achievments", id);
                    cmd.Parameters.AddWithValue("@id_coach", idcoach);

                    string sqlsel = "SELECT COUNT(*) FROM sportclub.sport_competition_sportsman WHERE id_sportsman = @id_sportsman and id_achievments = @id_achievments";

                    NpgsqlCommand cmdsel = new NpgsqlCommand(sqlsel, cnct);
                    cmdsel.Parameters.AddWithValue("@id_sportsman", idspotsman);
                    cmdsel.Parameters.AddWithValue("@id_achievments", id);
                    object result = cmdsel.ExecuteScalar();
                    int count = Convert.ToInt32(result);
                    this.Close();

                    if (count == 0)
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Записан на соревнования");

                    }
                    else
                    {
                        MessageBox.Show("Вы уже записали на соревнования");

                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка" + ex.Message);
                }
            }
            else
            {
                
                MessageBox.Show("Нужно выделить полностью строку");
                
            }
        }
    }
}
