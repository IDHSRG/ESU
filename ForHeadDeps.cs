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

namespace ESU
{
    public partial class ForHeadDeps : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        DataTable dt = new DataTable();
        string table, tableEn;
        bool isRelogin = false;
        public ForHeadDeps(string login, NpgsqlConnection c)
        {
            InitializeComponent();
            conn = c;
            toolStripLabel1.Text = "Здравствуйте, " + login + "!";
            radioButton1.Checked = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 1 || dataGridView1.SelectedRows.Count < 1)
                MessageBox.Show("Для редактирования необходимо выбрать одно поле. Повторите попытку, пожалуйста");
            else
            {
                AddElement ae = new AddElement(conn, this, tableEn, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                ae.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tableEn == "city" || tableEn == "industry" || tableEn == "type_disaster" || tableEn == "kind_disaster")
            {
                RefreshTable();
                int j = dataGridView1.Rows.Count;
                for (int i = 0; i < j;)
                {
                    if (textBox1.Text != "" && !dataGridView1.Rows[i].Cells[1].Value.ToString().ToLower().Contains(textBox1.Text.ToLower()))
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        j--;
                    }
                    else
                        i++;
                }
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
            }
            else if (comboBox1.Text == "" && comboBox2.Text == "")
                MessageBox.Show("Вы не выбрали фильтры для поиска, повторите попытку");
            else
            {
                int s1 = comboBox1.SelectedIndex; int s2 = comboBox2.SelectedIndex;
                RefreshTable();
                comboBox1.SelectedIndex = s1; comboBox2.SelectedIndex = s2;
                int j = dataGridView1.Rows.Count;
                for (int i = 0; i < j;)
                {
                    if (textBox1.Text != "" && comboBox1.SelectedIndex != -1 && !dataGridView1.Rows[i].Cells[comboBox1.SelectedIndex].Value.ToString().ToLower().Contains(textBox1.Text.ToLower()))
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        j--;
                    }
                    else if (textBox2.Text != "" && comboBox2.SelectedIndex != -1 && !dataGridView1.Rows[i].Cells[comboBox2.SelectedIndex].Value.ToString().ToLower().Contains(textBox2.Text.ToLower()))
                    {
                        dataGridView1.Rows.RemoveAt(i);
                        j--;
                    }
                    else
                        i++;
                }
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
                RefreshTable();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
                RefreshTable();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked)
                RefreshTable();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                RefreshTable();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RefreshTable();
        }

        private void ForHeadDeps_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isRelogin)
            Application.Exit();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
                RefreshTable();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
                RefreshTable();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked)
                RefreshTable();
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked)
                RefreshTable();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddElement ae = new AddElement(conn, this, tableEn);
            ae.Show();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                RefreshTable();
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users usr = new Users(conn, this);
            usr.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int deleted=0;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                comm = new NpgsqlCommand("SELECT * FROM delete_engagement(" + row.Cells[0].Value.ToString()+")", conn);
                deleted += comm.ExecuteNonQuery();
            }
            if (deleted == 0)
                MessageBox.Show("Не было удалено ни одного элемента");
            else
                MessageBox.Show("Было успешно удалено " + (-1*deleted).ToString() + " элементов!");
            RefreshTable();
        }

        private void обОтделеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkersOfDepartment wk = new WorkersOfDepartment(conn, this, "-1");
            wk.Show();
        }

        private void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isRelogin = true;
            this.Close();
            Authorisation at = new Authorisation();
            at.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            WorkersOfDepartment wkd;
            DepartmentEngageDisaster ded;
            if (radioButton8.Checked && (e.ColumnIndex == 0 || e.ColumnIndex == 1) && e.RowIndex >= 0)
            {
                wkd = new WorkersOfDepartment(conn, this, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                wkd.Show();
            }
            else if (radioButton7.Checked && (e.ColumnIndex == 0 || e.ColumnIndex == 1) && e.RowIndex >= 0)
            {
                ded = new DepartmentEngageDisaster(conn, this, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                ded.Show();
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            RefreshTable();
        }

        public void renewFilters()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                comboBox1.Items.Add(col.HeaderText);
                comboBox2.Items.Add(col.HeaderText);
            }
        }

        public void RefreshTable()
        {
            if (radioButton1.Checked)
            {
                textBox2.Enabled = false; comboBox1.Enabled = false; comboBox2.Enabled = false; button2.Enabled = false; button3.Enabled = false; button4.Enabled = false;
                table = "город"; tableEn = "city";
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM cities_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                label2.Text = "Просмотр: Города";
                renewFilters();
            }
            else if (radioButton2.Checked)
            {
                textBox2.Enabled = false; comboBox1.Enabled = false; comboBox2.Enabled = false; button2.Enabled = false; button3.Enabled = false; button4.Enabled = false;
                table = "отрасль производства"; tableEn = "industry";
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM industries_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                label2.Text = "Просмотр: Отрасли производства";
                renewFilters();
            }
            else if (radioButton3.Checked)
            {
                textBox2.Enabled = false; comboBox1.Enabled = false; comboBox2.Enabled = false; button2.Enabled = false; button3.Enabled = false; button4.Enabled = false;
                table = "вид последствия"; tableEn = "kind_disaster";
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM kind_disaster_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                label2.Text = "Просмотр: Виды последствий катастроф";
                renewFilters();
            }
            else if (radioButton4.Checked)
            {
                textBox2.Enabled = false; comboBox1.Enabled = false; comboBox2.Enabled = false; button2.Enabled = false; button3.Enabled = false; button4.Enabled = false;
                table = "тип катастрофы"; tableEn = "type_disaster";
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM type_disaster_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                label2.Text = "Просмотр: Типы катастроф";
                renewFilters();
            }
            else if (radioButton5.Checked)
                if (checkBox1.Checked)
                {
                    textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true; button2.Enabled = false; button3.Enabled = true; button4.Enabled = true;
                    table = "последствие"; tableEn = "consequence";
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM consequences_department_view", conn);
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                    label2.Text = "Просмотр: Последствия от катастроф";
                    renewFilters();
                }
                else
                {
                    textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true; button2.Enabled = false; button3.Enabled = false; button4.Enabled = true;
                    table = "последствие"; tableEn = "consequence";
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM consequences_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                    label2.Text = "Просмотр: Последствия от катастроф";
                    renewFilters();
                }
            else if (radioButton6.Checked)
                if (checkBox1.Checked)
                {
                    textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true; button2.Enabled = false; button3.Enabled = true; button4.Enabled = true;
                    table = "предприятие"; tableEn = "company";
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM companies_city_view", conn);
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                    label2.Text = "Просмотр: Предприятия";
                    renewFilters();
                }
                else
                {
                    textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true; button2.Enabled = false; button3.Enabled = false; button4.Enabled = true;
                    table = "предприятие"; tableEn = "company";
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM companies_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                    label2.Text = "Просмотр: Предприятия";
                    renewFilters();
                }
            else if (radioButton7.Checked)
                if (checkBox1.Checked)
                {
                    textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true; button2.Enabled = false; button3.Enabled = true; button4.Enabled = true;
                    table = "катастрофы"; tableEn = "disaster";
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM disasters_city_view", conn);
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                    label2.Text = "Просмотр: Катастрофы (Для просмотра участвоваших отделов, нажмите на номер катастрофы)";
                    renewFilters();
                }
                else
                {
                    textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true; button2.Enabled = false; button3.Enabled = false; button4.Enabled = true;
                    table = "катастрофы"; tableEn = "disaster";
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM disasters_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                    label2.Text = "Просмотр: Катастрофы (Для просмотра участвоваших отделов, нажмите на номер катастрофы)";
                    renewFilters();
                }
            else if (radioButton8.Checked)
                if (checkBox1.Checked)
                {
                    textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true; button2.Enabled = false; button3.Enabled = true; button4.Enabled = false;
                    table = "отделение"; tableEn = "department";
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM departments_user_view", conn);
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                    label2.Text = "Просмотр: Отделения ЭСУ (Для просмотра информации об отделении нажмите на его название или номер)";
                    renewFilters();
                }
                else
                {
                    textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true; button2.Enabled = false; button3.Enabled = false; button4.Enabled = false;
                    table = "отделение"; tableEn = "department";
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM departments_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                    label2.Text = "Просмотр: Отделения ЭСУ (Для просмотра информации об отделении нажмите на его название или номер)";
                    renewFilters();
                }
            else if (radioButton9.Checked)
                if (checkBox1.Checked)
                {
                    textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true; button2.Enabled = true; button3.Enabled = true; button4.Enabled = true;
                    table = "участие"; tableEn = "engagement";
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM engagements_department_view", conn);
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                    label2.Text = "Просмотр: Участие отделов ЭСУ в устранении катастроф";
                    renewFilters();
                }
                else
                {
                    textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true; button2.Enabled = false; button3.Enabled = false; button4.Enabled = true;
                    table = "участие"; tableEn = "engagement";
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM engagements_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                    label2.Text = "Просмотр: Участие отделов ЭСУ в устранении катастроф";
                    renewFilters();
                }
        }
    }
}
