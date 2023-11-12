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
            cnct.Open(); 
            try
            {
                DateTime currentDate = DateTime.Now;
                string sql = "SELECT id_achievments FROM sportclub.sport_competition_sportsman WHERE id_sportsman=@id_sportsman";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                cmd.Parameters.AddWithValue("@id_sportsman", idsportsman);

                object result = cmd.ExecuteScalar();
                int id_achievments = Convert.ToInt32(result);

                string sqlnew = "SELECT COUNT(*) FROM sportclub.sport_competition WHERE date<@date and id_achievments = @id_achievments";
                NpgsqlCommand cmdnew = new NpgsqlCommand(sqlnew, cnct);
                cmdnew.Parameters.AddWithValue("@date", currentDate);
                cmdnew.Parameters.AddWithValue("@id_achievments", id_achievments);

                object resnew = cmdnew.ExecuteScalar();
                int count = Convert.ToInt32(resnew);
                if (count == 0)
                {
                    label1.Text = "Соревнование еще не закончилось";
                    label2.Visible = false;
                    btaddres.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Er" + ex.Message);
            }

        }

        private void btaddres_Click(object sender, EventArgs e)
        {
                
        }
    }
}
