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
            //this.FormClosing += new FormClosingEventHandler(FormTraning_FormClosing);


        }
        public bool flagUser = true;
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public int idsport;
        public int idUser;
        public int id_sportsman;
        public int trener;
        public bool flag = false;

        
        public void SetData(string trener,int count,int time)
        {
            if (flag == true)
            {
                comboBoxTime.Text = time.ToString();
                comboBoxTrener.Text = trener;
                comboBoxCount.Text = count.ToString();
                button1.Text = "Изменить";
            }
        }

        private void FormTraning_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            button1.Select();
            string[] treningtime = { "60", "120" };
            comboBoxTime.Items.AddRange(treningtime);

            string[] week = { "1", "2", "3", "4", "5", "6", "7" };
            comboBoxCount.Items.AddRange(week);

            cnct.Open();
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT id_coach,name,last_name FROM sportclub.coach WHERE id_sport_type=@idsport", cnct);
                cmd.Parameters.AddWithValue("@idsport", idsport);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string idtrener = reader["id_coach"].ToString();
                        string name = reader["name"].ToString();
                        string last_name = reader["last_name"].ToString();
                        string new_str = name + " " + last_name + "                                        " + idtrener;
                        comboBoxTrener.Items.Add(new_str);
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message);
            }
        }
        public event Action<int> UserLoggedIn;
        private void button1_Click(object sender, EventArgs e)
        {
            string text = comboBoxTrener.Text;
            int count = Convert.ToInt32(comboBoxCount.Text);
            int time = Convert.ToInt32(comboBoxTime.Text);
            int idtren = 0;
            if (text.Length > 0)
            {
                string[] maspick = text.Split(' ');
                idtren = Convert.ToInt32(maspick.Last());
            }
            string sql = "";
            try
            {

                if (flag == false)
                {
                    sql = "INSERT INTO sportclub.traning(id_coach,id_sportsman,count_traning_week,traning_time) " +
                    "VALUES(@idtren,@idUser,@count,@time)";
                    
                }
                else
                {
                    sql = "UPDATE sportclub.traning SET id_coach = @idtren, count_traning_week = @count, traning_time = @time WHERE id_sportsman = @idUser";
                    
                }

                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                cmd.Parameters.AddWithValue("@idtren", idtren);
                cmd.Parameters.AddWithValue("@idUser", id_sportsman);
                cmd.Parameters.AddWithValue("@count", count);
                cmd.Parameters.AddWithValue("@time", time);
                cmd.ExecuteNonQuery();
                if (flag == false)
                {
                    MessageBox.Show("Вы записались на тренировку");
                }
                else
                {
                    MessageBox.Show("Вы изменили тренировку");
                }
                MainForm mainForm = new MainForm();
                flagUser = false;
                this.Close();
                cnct.Close();
                mainForm.HandleUserLogin(idUser);
                mainForm.userId = idUser;
                mainForm.coachId = idtren;
                mainForm.time = time;
                mainForm.week = count;
                mainForm.Show();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Вы уже записаны к этому тренеру "+ ex.Message);
            }
        }

        private void comboBoxTrener_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxTrener.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void comboBoxCount_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxCount.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void comboBoxTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxTime.DropDownStyle = ComboBoxStyle.DropDownList;
            
        }

        private void FormTraning_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flagUser)
            {
                e.Cancel = true;
            }
        }
    }
}
