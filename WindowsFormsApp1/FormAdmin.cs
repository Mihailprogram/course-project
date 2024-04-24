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
    public partial class FormAdmin : Form
    {
        static String connection = Properties.Settings.Default.pgConnection;

        NpgsqlConnection cnct = new NpgsqlConnection(connection);
        public bool flagadmin = false;
        public bool flagUser = true;
        public FormAdmin()
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


            RoundButtonCorners(button4, 40);
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

            RoundButtonCorners(button7, 20);
            button7.FlatStyle = FlatStyle.Flat;
            button7.FlatAppearance.BorderSize = 0;


            RoundButtonCorners(buttonDel, 40);
            buttonDel.FlatStyle = FlatStyle.Flat;
            buttonDel.FlatAppearance.BorderSize = 0;

            



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
        // Создание профиля для тренера
        private void button1_Click(object sender, EventArgs e)
        {
            FormCreatCoach formCreatCoach = new FormCreatCoach();
            formCreatCoach.Show();
        }
        // Просмотр всех тренеров
        private void button2_Click(object sender, EventArgs e)
        {

            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sportclub.viewauth1", cnct);
            cmd.Connection = cnct;
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["userid"].Visible = false;
        }
        // Просмотр всех спорсменов
        private void button3_Click(object sender, EventArgs e)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM sportclub.view_sportsman", cnct);
            cmd.Connection = cnct;
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["userid"].Visible = false;

        }

        private void FormAdmin_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            dataGridView1.ReadOnly = true;
            cnct.Open();

            dataGridView1.BackgroundColor = Color.White;
        }


        // Создание профиля для спортсмена
        private void button4_Click(object sender, EventArgs e)
        {
            Registration registration = new Registration();
            registration.flagadmin = flagadmin;
            registration.Show();
        }

        private void FormAdmin_FormClosing(object sender, FormClosingEventArgs e)
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
        //Удаление
        private void button5_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Получаем выбранную строку
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Получаем данные из выбранной строки
                int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                int userid = Convert.ToInt32(selectedRow.Cells["userid"].Value);

                string sqlrole = "SELECT roleid FROM sportclub.user " +
                    "WHERE iduser=@userid";
                NpgsqlCommand cmdrole = new NpgsqlCommand(sqlrole, cnct);
                cmdrole.Parameters.AddWithValue("@userid", userid);
             

                object resultrole = cmdrole.ExecuteScalar();
                int role = Convert.ToInt32(resultrole);

                DialogResult result = MessageBox.Show("Вы действительно хотите удалить товар?",
                    "Сообщение",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button2);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        if (role == 1)
                        {
                            NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM sportclub.sportsman WHERE id_sportsman = @id", cnct);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                            NpgsqlCommand cmd1 = new NpgsqlCommand("DELETE FROM sportclub.user WHERE iduser = @iduser", cnct);
                            cmd1.Parameters.AddWithValue("@iduser", userid);
                            cmd1.ExecuteNonQuery();
                            MessageBox.Show("Успешно");
                        }
                        else
                        {
                            NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM sportclub.coach WHERE id_sportsman = @id", cnct);
                            cmd.Parameters.AddWithValue("@id", id);
                            cmd.ExecuteNonQuery();
                            NpgsqlCommand cmd1 = new NpgsqlCommand("DELETE FROM sportclub.user WHERE iduser = @iduser", cnct);
                            cmd1.Parameters.AddWithValue("@iduser", userid);
                            cmd1.ExecuteNonQuery();
                            MessageBox.Show("Успешно");

                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку в таблице для удаление данных.");

            }
        }
        //Изменение
        private void button5_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Получаем данные из выбранной строки
                int id = Convert.ToInt32(selectedRow.Cells["ID"].Value);
                int userid = Convert.ToInt32(selectedRow.Cells["userid"].Value);

                string sqlrole = "SELECT roleid FROM sportclub.user " +
                    "WHERE iduser=@userid";
                NpgsqlCommand cmdrole = new NpgsqlCommand(sqlrole, cnct);
                cmdrole.Parameters.AddWithValue("@userid", userid);


                object resultrole = cmdrole.ExecuteScalar();
                int role = Convert.ToInt32(resultrole);

                if (role == 1)
                {
                    string data1 = selectedRow.Cells["Имя"].Value.ToString();
                    string data2 = selectedRow.Cells["Фамилия"].Value.ToString();
                    string data3 = selectedRow.Cells["Дата рождения"].Value.ToString();
                    string data4 = selectedRow.Cells["Тип спорта"].Value.ToString();

                    FormUpsportsman formUpsportsman = new FormUpsportsman();
                    formUpsportsman.SetData(data1, data2, data3, data4,id);
                    formUpsportsman.Show();
                }
                else
                {
                    string data1 = selectedRow.Cells["Имя"].Value.ToString();
                    string data2 = selectedRow.Cells["Фамилия"].Value.ToString();
                    string data3 = selectedRow.Cells["Дата рождения"].Value.ToString();
                    string data4 = selectedRow.Cells["Тип спорта"].Value.ToString();
                    string data5 = selectedRow.Cells["Опыт"].Value.ToString();

                    UPtrener uPtrener = new UPtrener();
                    uPtrener.SetData(data1, data2, data3, data4, data5, id);
                    uPtrener.Show();
                }

            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите строку в таблице для удаление данных.");
            }
        }
        // Добавление сорвенования
        private void button6_Click(object sender, EventArgs e)
        {
            FormCup formCompet = new FormCup();
            formCompet.Show();
        }
        // Выход из фомы
        private void button7_Click(object sender, EventArgs e)
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
        // Фильтрация данных по имени
        private void SearhButton_Click(object sender, EventArgs e)
        {
            string searchName = textBoxSearch.Text.Trim();

            
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = string.Format("Имя LIKE '%{0}%'", searchName);
        }
        // Сброс фильтров
        private void butBreack_Click(object sender, EventArgs e)
        {
            textBoxSearch.Text = "";
            textfilter.Text = "";

            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = "";
        }
        // Фильтрация данных по возрасту
        private void butfilter_Click(object sender, EventArgs e)
        {
            string textfilters = textfilter.Text.Trim();

            // Проверка, что введенное значение - это число
            if (!int.TryParse(textfilters, out int ageFilter))
            {
                MessageBox.Show("Введите корректное значение для фильтрации по возрасту.");
                return;
            }


(           dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"Возраст = '{ageFilter.ToString()}'";
        }
    }

}
