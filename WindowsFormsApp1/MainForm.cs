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

        public int userId;
        public int coachId;
        public int time;
        public int week;
        public int id_sportsman;
        public bool flagUser = true;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            cnct.Open();
            //label4

            try
            {
                
                string sql = "SELECT id_sportsman FROM sportclub.sportsman " +
                    "WHERE userid=@userId";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                cmd.Parameters.AddWithValue("@userId", userId);
                object result = cmd.ExecuteScalar();
                id_sportsman = Convert.ToInt32(result);


                string sqltran = "SELECT COUNT(*) FROM sportclub.traning " +
                    "WHERE id_sportsman=@id_sportsman";
                NpgsqlCommand cmdtran = new NpgsqlCommand(sqltran, cnct);
                cmdtran.Parameters.AddWithValue("@id_sportsman", id_sportsman);
                object resultran = cmdtran.ExecuteScalar();
                int traning = Convert.ToInt32(resultran);
                
                string name = "";
                string last_name = "";
                week = 0;
                time = 0;

                string sqlinj = "SELECT COUNT(*) FROM sportclub.injury WHERE id_sportsman=@id_sportsman";
                NpgsqlCommand cmdinj = new NpgsqlCommand(sqlinj, cnct);
                cmdinj.Parameters.AddWithValue("@id_sportsman", id_sportsman);
                object resulinj = cmdinj.ExecuteScalar();
                int injur = Convert.ToInt32(resulinj);
                //MessageBox.Show(count.ToString());
                    if (traning > 0 && injur==0)
                {
                    button1.Visible = false;
                    string sqls = "SELECT * FROM sportclub.get_training_info(@id_sportsman)";

                    NpgsqlCommand cmds = new NpgsqlCommand(sqls, cnct);
                    cmds.Parameters.AddWithValue("@id_sportsman", id_sportsman);
                    NpgsqlDataReader reader = cmds.ExecuteReader();
                    while (reader.Read())
                    {
                        name = reader["name"].ToString();
                        last_name = reader["last_name"].ToString();
                        week = Convert.ToInt32(reader["count_traning_week"]);
                        time = Convert.ToInt32(reader["traning_time"]);
                    }

                    reader.Close();
                    label4.Text = $"Ваш тренер {name} {last_name} \n" +
                        $"Количество тренировок в неделю {week} \nВремя занятий {time} мин.";
                }
                else
                {
                    button2.Visible = false;
                    button3.Visible = false;
                    if (injur == 0)
                    {
                        label4.Text = "Вы пока не записаны на тренировки";
                    }
                    else
                    {
                        button1.Visible = false;
                        string sqlinj1 = "SELECT end_date FROM sportclub.injury WHERE id_sportsman=@id_sportsman";
                        NpgsqlCommand cmdinj1 = new NpgsqlCommand(sqlinj1, cnct);
                        cmdinj1.Parameters.AddWithValue("@id_sportsman", id_sportsman);
                        label4.Text = $"Вы травмированны и пока не можете тренироваться";

                    }
                }
                cnct.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка!!: " + ex.Message);
            }
        }
        

        int idtypesport = 0;
        public void HandleUserLogin(int userID)
        {
            string name = "";
            string lastName = "";
            string typesport = "";
            cnct.Open();
            try
            {
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


                label1.Text = $"{name} {lastName}";
                label2.Text = $"Ваш вид спорта {typesport}";

                cnct.Close();
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
            formTraning.id_sportsman = id_sportsman;
            formTraning.idUser = userId;
            formTraning.Show();
            cnct.Close();
            flagUser = false;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cnct.Open();
            string sql = "DELETE FROM sportclub.traning WHERE id_sportsman = @id_sportsman ";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
            cmd.Parameters.AddWithValue("@id_sportsman", id_sportsman);
            cmd.ExecuteNonQuery();
            MessageBox.Show("Успешно");
            MainForm mainForm = new MainForm();
            flagUser = false;
            this.Close();
            cnct.Close();
            mainForm.HandleUserLogin(userId);
            mainForm.userId = userId;
            mainForm.coachId = coachId;
            mainForm.time = time;
            mainForm.week = week;
            mainForm.Show();
            cnct.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cnct.Open();


            string sqls = "SELECT id_coach FROM sportclub.traning " +
                    "WHERE id_sportsman=@id_sportsman";
            NpgsqlCommand cmds = new NpgsqlCommand(sqls, cnct);
            cmds.Parameters.AddWithValue("@id_sportsman", id_sportsman);
            object result = cmds.ExecuteScalar();
            int trenerid = Convert.ToInt32(result);

            string new_str = "";
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT id_coach,name,last_name FROM sportclub.coach WHERE id_coach=@coachId", cnct);
            cmd.Parameters.AddWithValue("@coachId", trenerid);
            using (NpgsqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    string idtrener = reader["id_coach"].ToString();
                    string name = reader["name"].ToString();
                    string last_name = reader["last_name"].ToString();
                    new_str = name + " " + last_name + "                                        " + idtrener;
                    
                }

            }
            
            FormTraning formTraning = new FormTraning();
            formTraning.idsport = idtypesport;
            formTraning.idUser = userId;
            formTraning.id_sportsman = id_sportsman;
            formTraning.flag = true;
            formTraning.SetData(new_str,week,time);
           formTraning.Show();
            flagUser = false;
            this.Close();
            cnct.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            DialogResult result = MessageBox.Show("Вы действительно хотите выйти?",
                    "Сообщение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button2);
            if (result == DialogResult.Yes)
            {
                flagUser = false;
                this.Close();
                Authorization authorization = new Authorization();
                authorization.Show();       
               
            }

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (flagUser)
            {
                DialogResult result = MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Подтверждение", MessageBoxButtons.YesNo);

                if (result == DialogResult.No)
                {

                    e.Cancel = true;
                }
                else
                {
                    Application.OpenForms["Authorization"].Close();
                }
            }
        }
  
    }
}
