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

namespace WindowsFormsApp1
{
    public partial class FormInjury : Form
    {
        public FormInjury()
        {
            InitializeComponent();
        }
        public int sportsmanid;

        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        private void FormInjury_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            cnct.Open();
        }
        // Метод для добавления трамы спорсмену
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                DateTime currentDate = DateTime.Now;
                string nameinj = textBox1.Text;
                int period = Convert.ToInt32(textBox2.Text);
                DateTime end_date = currentDate.AddDays(period);
                string sql = "INSERT INTO sportclub.injury(id_sportsman,name_injury,recovery_period,date_start,end_date) VALUES(@id_sportsman,@name_injury,@recovery_period,@date_start,@end_date)";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                cmd.Parameters.AddWithValue("@id_sportsman", sportsmanid);
                cmd.Parameters.AddWithValue("@name_injury", nameinj);
                cmd.Parameters.AddWithValue("@recovery_period", period);
                cmd.Parameters.AddWithValue("@date_start", currentDate      );
                cmd.Parameters.AddWithValue("@end_date", end_date);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Успешно");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
