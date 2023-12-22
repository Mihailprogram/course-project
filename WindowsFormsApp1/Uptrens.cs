using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WindowsFormsApp1
{
    public partial class Uptrens : Form
    {
        public Uptrens()
        {
            InitializeComponent();
        }
        public int sportsmanid;
        public int coachid;

        public int time;
        public int count;


        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);

        private void button1_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(comboBoxCount.Text);
            int time = Convert.ToInt32(comboBoxTime.Text);
            try
            {
                string sql = "UPDATE sportclub.traning SET count_traning_week = @count, traning_time = @time WHERE id_sportsman = @idUser and id_coach=@id_coach";

            

                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                cmd.Parameters.AddWithValue("@id_coach", coachid);
                cmd.Parameters.AddWithValue("@idUser", sportsmanid);
                cmd.Parameters.AddWithValue("@count", count);
                cmd.Parameters.AddWithValue("@time", time);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Успешно");
                this.Close();

                if (Application.OpenForms["FormCoach"] is FormCoach form1)
                {
                    form1.upForms();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Eror" + ex.Message);
            }
        }

        private void Uptrens_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            cnct.Open();
            string[] treningtime = { "60", "120" };
            comboBoxTime.Items.AddRange(treningtime);

            string[] week = { "1", "2", "3", "4", "5", "6", "7" };
            comboBoxCount.Items.AddRange(week);

            comboBoxTime.Text = time.ToString();
            comboBoxCount.Text = count.ToString();

        }
    }
}
