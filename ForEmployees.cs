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
    public partial class ForEmployees : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        DataTable dt = new DataTable();
        string table, tableEn; bool isRelogin;
        public ForEmployees(string login, NpgsqlConnection c)
        {
            InitializeComponent();
            conn = c;
            toolStripLabel1.Text = "Здравствуйте, " + login + "!";
            radioButton2.Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddElement form = new AddElement(conn, this);
            form.Show();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
                RefreshTable();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
                RefreshTable();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked == true)
                RefreshTable();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked == true)
                RefreshTable();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton5.Checked == true)
                RefreshTable();
            
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton9.Checked == true)
                RefreshTable();
        }

        private void ForEmployees_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isRelogin)
            Application.Exit();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            RefreshTable();
        }

        private void сотрудникиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users usr = new Users(conn,this);
            usr.Show();
        }

        private void обОтделеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WorkersOfDepartment wk = new WorkersOfDepartment(conn, this);
            wk.Show();
        }

        private void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isRelogin = true;
            this.Close();
            Authorisation at = new Authorisation();
            at.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RefreshTable();
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
            if (radioButton2.Checked)
            {
                textBox2.Enabled = false; comboBox1.Enabled = false; comboBox2.Enabled = false;
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
                dataGridView1.Columns[0].Visible = false;
            }
            else if (radioButton3.Checked)
            {
                textBox2.Enabled = false; comboBox1.Enabled = false; comboBox2.Enabled = false;
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
                dataGridView1.Columns[0].Visible = false;
            }
            else if (radioButton4.Checked)
            {
                textBox2.Enabled = false; comboBox1.Enabled = false; comboBox2.Enabled = false;
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
                dataGridView1.Columns[0].Visible = false;
            }
            else if (radioButton5.Checked)
            {
                textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true;
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
                dataGridView1.Columns[0].Visible = false;
            }
            else if (radioButton6.Checked)
            {
                textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true;
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
                dataGridView1.Columns[0].Visible = false;
            }
            else if (radioButton7.Checked)
            {
                textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true;
                table = "катастрофы"; tableEn = "disaster";
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM disasters_city_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                label2.Text = "Просмотр: Катастрофы";
                renewFilters();
                dataGridView1.Columns[0].Visible = false;
            }
            else if (radioButton9.Checked)
            {
                textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true;
                table = "участие"; tableEn = "engagement";
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM engagements_department_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                label2.Text = "Просмотр: Участие в устранении катастроф";
                renewFilters();
                dataGridView1.Columns[0].Visible = false;
            }
        }
    }
}
