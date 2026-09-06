using Microsoft.VisualBasic;
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
    public partial class OnlyReferences : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        DataTable dt = new DataTable();
        string table, tableEn;
        public OnlyReferences(NpgsqlConnection c)
        {
            InitializeComponent();
            conn = c; 
            radioButton1.Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            string input = Interaction.InputBox("Пожалуйста, введите запись в поле ниже", "Добавить " + table, "");
            if (input.Trim() == "")
                MessageBox.Show("Вы не ввели значение. Повторите попытку");
            else
            {
                comm = new NpgsqlCommand("SELECT * FROM add_" + tableEn + "(\'" + input + "\')", conn);
                try
                {
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Запись успешно добавлена");
                    RefreshTable();
                }
                catch(PostgresException err)
                {
                    if (err.MessageText == "повторяющиеся значения ключа")
                        MessageBox.Show("Вы ввели повторяющееся значение в справочник, повторите попытку!");
                    else
                        MessageBox.Show("Что-то не так, повторите попытку позже!");
                }
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 1)
            {
                MessageBox.Show("Вы не выбрали запись!");
            }
            else if (dataGridView1.SelectedRows.Count > 1)
                MessageBox.Show("Вы выбрали несколько записей - выберите одну!");
            else
            {
                string input = Interaction.InputBox("Пожалуйста, введите запись в поле ниже",  "Изменить запись \"" + table+"\"",  "");
                if (input.Trim() == "")
                    MessageBox.Show("Вы не ввели значение. Повторите попытку");
                else
                {
                    try
                    {
                        comm = new NpgsqlCommand("SELECT * FROM update_" + tableEn + "(" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + ",\'" + input + "\')", conn);
                        comm.ExecuteNonQuery();
                        MessageBox.Show("Запись успешно изменена");
                        RefreshTable();
                    }
                    catch (PostgresException err)
                    {
                        if (err.MessageText == "повторяющиеся значения ключа")
                            MessageBox.Show("Вы ввели повторяющееся значение в справочник, повторите попытку!");
                        else
                            MessageBox.Show("Что-то не так, повторите попытку позже!");
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RefreshTable();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            RefreshTable();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            RefreshTable();
        }

        private void OnlyReferences_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            RefreshTable();
        }
        public void RefreshTable()
        {
            if (radioButton1.Checked)
            {
                dt = new DataTable();
                table = "город"; tableEn = "city";
                comm = new NpgsqlCommand("SELECT * FROM cities_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                dataGridView1.Columns[0].Visible = false;
            }
            else if (radioButton2.Checked)
            {
                dt = new DataTable();
                table = "отрасль производства"; tableEn = "industry";
                comm = new NpgsqlCommand("SELECT * FROM industries_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                dataGridView1.Columns[0].Visible = false;
            }
            else if (radioButton3.Checked)
            {
                table = "вид последствия"; tableEn = "kind_disaster";
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM kind_disaster_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                dataGridView1.Columns[0].Visible = false;
            }
            else if (radioButton4.Checked)
            {
                table = "тип катастрофы"; tableEn = "type_disaster";
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM type_disaster_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                dataGridView1.Columns[0].Visible = false;
            }
        }
    }
}
