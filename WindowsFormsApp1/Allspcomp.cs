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
    public partial class Allspcomp : Form
    {
        // Строка подключения к базе данных PostgreSQL
        static String connection = Properties.Settings.Default.pgConnection;

        // Подключение к базе данных PostgreSQL
        NpgsqlConnection cnct = new NpgsqlConnection(connection);

        public Allspcomp()
        {
            InitializeComponent();
        }

        // Переменная для хранения идентификатора типа спорта
        public int idtypesport = 0;

        // Метод загрузки формы
        private void Allspcomp_Load(object sender, EventArgs e)
        {
            // Настройка формы
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            dataGridView1.BackgroundColor = Color.White;

            // Запрос к базе данных для получения списка соревнований по выбранному типу спорта
            using (NpgsqlCommand cmd = new NpgsqlCommand())
            {
                cmd.Connection = cnct; // Используйте существующее подключение

                try
                {
                    cnct.Open();

                    // SQL-запрос для выборки соревнований из таблицы sport_competition
                    cmd.CommandText = "SELECT name_competition FROM sportclub.sport_competition WHERE id_type_sport = @idtypesport";

                    cmd.Parameters.AddWithValue("@idtypesport", idtypesport);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Добавляем соревнования в ComboBox
                            name_comp.Items.Add(reader["name_competition"].ToString());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузке соревнований: " + ex.Message);
                }
            }
        }

        // Обработчик нажатия кнопки
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Получаем выбранное соревнование из ComboBox
                string namecomp = name_comp.Text;

                // Создаем SQL-команду для получения спортсменов по выбранному соревнованию
                NpgsqlCommand cmds = new NpgsqlCommand("SELECT * FROM sportclub.select_allsportsman(@namecomp)", cnct);
                cmds.Parameters.AddWithValue("@namecomp", namecomp);

                // Создаем адаптер данных
                NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmds);
                DataSet ds = new DataSet();

                // Заполняем таблицу данными
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].HeaderText = "Имя";
                dataGridView1.Columns[1].HeaderText = "Фамилия";
                dataGridView1.Columns[2].HeaderText = "Возраст";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // Обработчик изменения выбранного элемента в ComboBox
        private void name_comp_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Запретить редактирование ComboBox
            name_comp.DropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
}
