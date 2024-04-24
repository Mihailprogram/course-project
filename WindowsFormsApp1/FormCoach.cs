using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class FormCoach : Form
    {
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public FormCoach()
        {
            InitializeComponent();
            RoundButtonCorners(button1, 40);
            button1.FlatStyle = FlatStyle.Flat;
            button1.FlatAppearance.BorderSize = 0;



            RoundButtonCorners(button2, 40);
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;


            RoundButtonCorners(button3, 40);
            button3.FlatStyle = FlatStyle.Flat;
            button3.FlatAppearance.BorderSize = 0;


            RoundButtonCorners(button4, 20);
            button4.FlatStyle = FlatStyle.Flat;
            button4.FlatAppearance.BorderSize = 0;


            RoundButtonCorners(button5, 40);
            button5.FlatStyle = FlatStyle.Flat;
            button5.FlatAppearance.BorderSize = 0;


            RoundButtonCorners(button6, 40);
            button6.FlatStyle = FlatStyle.Flat;
            button6.FlatAppearance.BorderSize = 0;

            RoundButtonCorners(button7, 40);
            button7.FlatStyle = FlatStyle.Flat;
            button7.FlatAppearance.BorderSize = 0;

            RoundButtonCorners(button7, 40);
            button7.FlatStyle = FlatStyle.Flat;
            button7.FlatAppearance.BorderSize = 0;

            
            RoundButtonCorners(butInjur, 40);
            butInjur.FlatStyle = FlatStyle.Flat;
            butInjur.FlatAppearance.BorderSize = 0;


            
            RoundButtonCorners(btAchiv, 40);
            btAchiv.FlatStyle = FlatStyle.Flat;
            btAchiv.FlatAppearance.BorderSize = 0;


        }
        // Метод для создания кнопки с закругленными углами
        private void RoundButtonCorners(Control control, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(control.ClientRectangle.Width - cornerRadius, 0, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(control.ClientRectangle.Width - cornerRadius, control.ClientRectangle.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(0, control.ClientRectangle.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            path.AddArc(0, 0, cornerRadius, cornerRadius, 180, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }

        public int userID;
        public int id_coach = 0;
        public bool flagUser = true;
        bool allowActions = true;
        public int idtypesport = 0;
        private void FormCoach_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            dataGridView1.ReadOnly = true;

            dataGridView1.BackgroundColor = Color.White;
            string name = "";
            string lastName = "";
            string expir = "";
            string age = "";
            string typesport = "";
            
            cnct.Open();
            try
            {

                NpgsqlCommand cmds = new NpgsqlCommand("SELECT id_coach FROM sportclub.coach WHERE userid = @userid", cnct);
                cmds.Parameters.AddWithValue("@userid", userID);
                object resulid = cmds.ExecuteScalar();
                id_coach = Convert.ToInt32(resulid);


                NpgsqlCommand cmd1 = new NpgsqlCommand("SELECT * FROM sportclub.get_trains_coach(@id_coach)", cnct);
                cmd1.Parameters.AddWithValue("@id_coach", id_coach);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd1);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns["id_sportsman"].Visible = false;
                dataGridView1.Columns[1].HeaderText = "Имя";

                // Измените название второго столбца на "Фамилия"
                dataGridView1.Columns[2].HeaderText = "Фамилия";
                dataGridView1.Columns[3].HeaderText = "Количество тренировок в неделю";
                dataGridView1.Columns[4].HeaderText = "Время тренировок";


                string sql = "SELECT id_sport_type,name,last_name,age,experience FROM sportclub.coach " +
                        "WHERE userid = @userid";

                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                cmd.Parameters.AddWithValue("@userid", userID);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    name = reader["name"].ToString();
                    lastName = reader["last_name"].ToString();
                    idtypesport = Convert.ToInt32(reader["id_sport_type"]);
                    age = reader["age"].ToString();
                    expir = reader["experience"].ToString();
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

                DateTime currentDate = DateTime.Now;
                string delsql = "DELETE FROM sportclub.injury WHERE end_date < @date";
                NpgsqlCommand cmddel = new NpgsqlCommand(delsql, cnct);
                cmddel.Parameters.AddWithValue("@date", currentDate);
                cmddel.ExecuteNonQuery();
                string tr = "лет";
                string tr1 = "лет";
                int remainder1 = Convert.ToInt32(age) ;
                int remainder = Convert.ToInt32(expir) ;
                if ((remainder % 10 == 2 || remainder % 10 == 3 || remainder % 10 == 4) && (remainder!=11 && remainder!=12 && remainder!=13 && remainder != 14))
                {
                    tr = "года";
                }
                if (remainder % 10 == 1 && (remainder != 11 && remainder != 12 && remainder != 13 && remainder != 14))
                {
                    tr = "год";
                }
                if ((remainder1 % 10 == 2 || remainder1 % 10 == 3 || remainder1 % 10 == 4) && (remainder1 != 11 && remainder1 != 12 && remainder1 != 13 && remainder1 != 14))
                {
                    tr1 = "года";
                }
                if ((remainder1 % 10 == 1) && (remainder1 != 11 && remainder1 != 12 && remainder1 != 13 && remainder1 != 14))
                {
                    tr1 = "год";
                }

                label1.Text = $"ТРЕНЕР: {name} {lastName}\n Опыт: {expir} {tr}\n Возраст: {age} {tr1}";
                label1.TextAlign = ContentAlignment.MiddleRight;
                label2.Text = $"Ваш вид спорта {typesport}";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка!!: " + ex.Message);
            }
        }
        // Показывает списко спорсменов
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                allowActions = true;
                NpgsqlCommand cmds = new NpgsqlCommand("SELECT id_coach FROM sportclub.coach WHERE userid = @userid", cnct);
                cmds.Parameters.AddWithValue("@userid", userID);
                object resulid = cmds.ExecuteScalar();
                id_coach = Convert.ToInt32(resulid);


                NpgsqlCommand cmd1 = new NpgsqlCommand("SELECT * FROM sportclub.get_trains_coach(@id_coach)", cnct);
                cmd1.Parameters.AddWithValue("@id_coach", id_coach);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd1);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns["id_sportsman"].Visible = false;
                dataGridView1.Columns[1].HeaderText = "Имя";

                // Измените название второго столбца на "Фамилия"
                dataGridView1.Columns[2].HeaderText = "Фамилия";
                dataGridView1.Columns[3].HeaderText = "Количество тренировок в неделю";
                dataGridView1.Columns[4].HeaderText = "Время тренировок";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void upForms()
        {
            try
            {
                allowActions = true;
                NpgsqlCommand cmds = new NpgsqlCommand("SELECT id_coach FROM sportclub.coach WHERE userid = @userid", cnct);
                cmds.Parameters.AddWithValue("@userid", userID);
                object resulid = cmds.ExecuteScalar();
                id_coach = Convert.ToInt32(resulid);


                NpgsqlCommand cmd1 = new NpgsqlCommand("SELECT * FROM sportclub.get_trains_coach(@id_coach)", cnct);
                cmd1.Parameters.AddWithValue("@id_coach", id_coach);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd1);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns["id_sportsman"].Visible = false;
                dataGridView1.Columns[1].HeaderText = "Имя";

                // Измените название второго столбца на "Фамилия"
                dataGridView1.Columns[2].HeaderText = "Фамилия";
                dataGridView1.Columns[3].HeaderText = "Количество тренировок в неделю";
                dataGridView1.Columns[4].HeaderText = "Время тренировок";


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        //Изменение тренировки
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && allowActions==true)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["id_sportsman"].Value);
                int count = Convert.ToInt32(selectedRow.Cells["count_traning_week"].Value);
                int time = Convert.ToInt32(selectedRow.Cells["traning_time"].Value);
                Uptrens uptrens = new Uptrens();
                uptrens.sportsmanid = id;
                uptrens.coachid = id_coach;
                uptrens.count = count;
                uptrens.time = time;

                uptrens.Show();

            }
            else
            {
                if (allowActions == true)
                {
                    MessageBox.Show("Нужно выделить полностью строку");
                }
                else
                {
                    MessageBox.Show("Нельзя выполнить это действие");

                }
            }
        }

        // Добавление травмированных спорсменов
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && allowActions==true)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["id_sportsman"].Value);
                FormInjury formInjury = new FormInjury();
                formInjury.sportsmanid = id;
                formInjury.Show();
            }
            else
            {
                if (allowActions == true)
                {
                    MessageBox.Show("Нужно выделить полностью строку");
                }
                else
                {
                    MessageBox.Show("Нельзя выполнить это действие");

                }
            }
        }
        // Выход на форму с аунтификацией
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

        private void FormCoach_FormClosing(object sender, FormClosingEventArgs e)
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
        // Метод показывает список травмированных спорсмменов
        private void butInjur_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "SELECT name,last_name,name_injury,date_start,end_date FROM sportclub.injury JOIN sportclub.sportsman ON  sportclub.sportsman.id_sportsman = sportclub.injury.id_sportsman";
                NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);

                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                dataGridView1.Columns[0].HeaderText = "Имя";

                // Измените название второго столбца на "Фамилия"
                dataGridView1.Columns[1].HeaderText = "Фамилия";
                dataGridView1.Columns[2].HeaderText = "Название травмы";
                dataGridView1.Columns[3].HeaderText = "Дата травмирования";
                dataGridView1.Columns[4].HeaderText = "Дата выписки";
                allowActions = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка" + ex.Message);
            }
        }

        // Запись на соревнование
        private void button5_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0 && allowActions == true)
            {

                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["id_sportsman"].Value);
                try
                {
                    string sql = "SELECT COUNT(*) FROM sportclub.injury WHERE id_sportsman=@id_sportsman";

                    NpgsqlCommand cmd = new NpgsqlCommand(sql, cnct);
                    cmd.Parameters.AddWithValue("@id_sportsman", id);
                    object res = cmd.ExecuteScalar();
                    int result = Convert.ToInt32(res);
                    if (result == 0)
                    {
                        FormReckCup formReckCup = new FormReckCup();
                        formReckCup.idspotsman = id;
                        formReckCup.idcoach = id_coach;
                        formReckCup.typecup = idtypesport;
                        formReckCup.Show();
                    }
                    else
                    {
                        MessageBox.Show("Спортсмен травмирован");
                    }
                }
                catch (Exception)
                {

                }


            }
            else
            {
                if (allowActions == true)
                {
                    MessageBox.Show("Нужно выделить полностью строку");
                }
                else
                {
                    MessageBox.Show("Нельзя выполнить это действие");

                }
            }
        }
        // Метод показывает результат соревнования
        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0 && allowActions == true)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                int id = Convert.ToInt32(selectedRow.Cells["id_sportsman"].Value);
                CreatResult creatResult = new CreatResult();
                creatResult.idsportsman = id;
                creatResult.Show();
            }
            else
            {
                if (allowActions == true)
                {
                    MessageBox.Show("Нужно выделить полностью строку");
                }
                else
                {
                    MessageBox.Show("Нельзя выполнить это действие");

                }

            }
        }
        // Показывает списко достижений спорсменов
        private void btAchiv_Click(object sender, EventArgs e)
        {
            allowActions = false;
            try
            {
                NpgsqlCommand cmd1 = new NpgsqlCommand("SELECT * FROM sportclub.get_result_coach(@id_coach)", cnct);
                cmd1.Parameters.AddWithValue("@id_coach", id_coach);
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd1);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Columns[0].HeaderText = "Имя";
                    dataGridView1.Columns[1].HeaderText = "Фамилия";
                    dataGridView1.Columns[2].HeaderText = "Возраст";
                    dataGridView1.Columns[3].HeaderText = "Название соревнования";
                    dataGridView1.Columns[4].HeaderText = "Дата проведения";
                    dataGridView1.Columns[5].HeaderText = "Результат соревнования(Место)";
                }
                else
                {
                    dataGridView1.Columns[0].HeaderText = "Пока нет достижений";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERR" + ex.Message);
            }
        }
        // Показывает список участников соревнования
        private void button7_Click(object sender, EventArgs e)
        {
            Allspcomp allspcomp = new Allspcomp();
            allspcomp.idtypesport = idtypesport;
            allspcomp.Show();
        }
    }
}
