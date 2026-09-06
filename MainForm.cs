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
    public partial class MainForm : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        DataTable dt = new DataTable();
        string table="", tableEn="";
        int isAdmin; bool isRelogin = false;
        public MainForm(String login, bool isAdmin, NpgsqlConnection c)
        {
            InitializeComponent();
            conn = c;
            if (isAdmin)
                this.isAdmin = 1;
            else { this.isAdmin = 0; toolStripButton1.Visible = false; }
            toolStripLabel1.Text = "Здравствуйте, " + login + "!";
            radioButton1.Checked = true;
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

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
                RefreshTable();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
                RefreshTable();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton8.Checked)
                RefreshTable();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
            {
                RefreshTable();
            }
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isRelogin)
                Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tableEn == "city" || tableEn == "industry" || tableEn == "type_disaster" || tableEn == "kind_disaster")
            {
                string input = Interaction.InputBox("Пожалуйста, введите запись в поле ниже", "Добавить " + table, "");
                if (input.Trim() == "")
                    MessageBox.Show("Вы не ввели значение. Повторите попытку");
                else
                {
                    try
                    {
                        comm = new NpgsqlCommand("SELECT * FROM add_" + tableEn + "(\'" + input + "\')", conn);
                        comm.ExecuteNonQuery();
                        MessageBox.Show("Запись успешно добавлена");
                    }
                    catch { MessageBox.Show("Введенное значение неверно, повторите попытку!"); }
                    switch (tableEn)
                    {
                        case "city":
                            radioButton1.Select();
                            break;
                        case "industry":
                            radioButton2.Select();
                            break;
                        case "kind_disaster":
                            radioButton3.Select();
                            break;
                        case "type_disaster":
                            radioButton4.Select();
                            break;
                    }
                }
            }
            else
            {
                AddElement ae = new AddElement(conn, this, tableEn);
                ae.Show();
            }
        }

        private void просмотретьПользователейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Users usr = new Users(conn, this, isAdmin);
            usr.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int deleted = 0;
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    comm = new NpgsqlCommand("SELECT * FROM delete_" + tableEn + "(" + row.Cells[0].Value.ToString() + ")", conn);
                    deleted += comm.ExecuteNonQuery();
                }
                if (deleted == 0)
                    MessageBox.Show("Не было удалено ни одного элемента");
                else
                    MessageBox.Show("Было успешно удалено " + (-1 * deleted).ToString() + " элементов!");
                RefreshTable();
            }
            catch (PostgresException err)
            {
                if (err.MessageText.Contains("delete yourself"))
                {
                    MessageBox.Show("Вы удалили своего пользователя. Выход из системы.");
                    сменитьПользователяToolStripMenuItem_Click(this, e);
                }
                else
                    MessageBox.Show("Что-то не так, повторите попытку позже.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 1 || dataGridView1.SelectedRows.Count < 1)
                MessageBox.Show("Для редактирования необходимо выбрать одно поле. Повторите попытку, пожалуйста");
            else
            {
                if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton4.Checked)
                {
                    string input = Interaction.InputBox("Пожалуйста, введите запись в поле ниже", "Изменить запись \"" + table + "\"", "");
                    if (input.Trim() == "")
                        MessageBox.Show("Вы не ввели значение. Повторите попытку");
                    else
                    {
                        try
                        {
                            comm = new NpgsqlCommand("SELECT * FROM update_" + tableEn + "(" + dataGridView1.SelectedRows[0].Cells[0].Value.ToString() + ",\'" + input + "\')", conn);
                            comm.ExecuteNonQuery();
                            MessageBox.Show("Запись успешно изменена");
                        }
                        catch { MessageBox.Show("Введенное значение неверно, повторите попытку!"); }

                    }
                    RefreshTable();
                }
                else
                {
                    AddElement ae = new AddElement(conn, this, tableEn, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                    ae.Show();
                }
            }
        }

        public void сменитьПользователяToolStripMenuItem_Click(object sender, EventArgs e)
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
                if (isAdmin == 1)
                {
                    wkd = new WorkersOfDepartment(conn, this, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), true);
                    wkd.Show();
                }
                else
                {
                    wkd = new WorkersOfDepartment(conn, this, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), false);
                    wkd.Show();
                }
            else if (radioButton7.Checked && (e.ColumnIndex == 0 || e.ColumnIndex == 1) && e.RowIndex >= 0)
                {
                    ded = new DepartmentEngageDisaster(conn, this, dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    ded.Show();
                }
        }
      

        public void RefreshTable()
        {
            if (radioButton1.Checked)
            {
                textBox2.Enabled = false; comboBox1.Enabled = false; comboBox2.Enabled = false; button2.Enabled = true;
                dt = new DataTable();
                table = "город"; tableEn = "city";
                comm = new NpgsqlCommand("SELECT * FROM cities_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                label2.Text = "Просмотр: Города";
                renewFilters();
                dataGridView1.Columns[0].Visible = false;
            }
            else if (radioButton2.Checked)
            {
                textBox2.Enabled = false; comboBox1.Enabled = false; comboBox2.Enabled = false; 
                dt = new DataTable();
                table = "отрасль производства"; tableEn = "industry";
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
                {
                textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true;
                dt = new DataTable();
                table = "предприятие"; tableEn = "company";
                comm = new NpgsqlCommand("SELECT * FROM companies_all_view", conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
                label2.Text = "Просмотр: Предприятия";
                foreach (DataGridViewColumn col in dataGridView1.Columns)
                    renewFilters();
            }
            else if (radioButton7.Checked)
            {
                textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true;
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
            {
                textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true;
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
            {
                textBox2.Enabled = true; comboBox1.Enabled = true; comboBox2.Enabled = true;
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

        private void button5_Click(object sender, EventArgs e)
        {
            RefreshTable();
        }

        private void вывестиТаблицуКатастрофИИхПоследствияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 5, "Вывести таблицу катастроф и их последствий");
            t.Show();
        }

        private void вывестиТаблицуОтделовИИхУчастияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 6, "Вывести таблицу отделов и их участий");
            t.Show();
        }

        private void вывестиТаблицуПредприятияИИхКатастрофToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 7, "Вывести таблицу предприятия и их катастроф");
            t.Show();
        }

        private void вывестиОтделыНеПринимавшиеУчастияВУстраненииКакихлибоКатастрофToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 8, "Вывести отделы, не принимавшие участия в устранении каких-либо катастроф");
            t.Show();
        }

        private void вывестиПредприятияКоторыеЕщёНеПострадалиОтЭкологическихКатастрофToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 9, "Вывести предприятия, которые ещё не пострадали от экологических катастроф");
            t.Show();
        }

        private void вывестиКатастрофыВУстраненииКоторыхНиктоНеУчаствовалToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 10, "Вывести катастрофы, в устранении которых никто не участвовал");
            t.Show();
        }

        private void вывестиОбщееКоличествоРазныхВидовКатастрофToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 11, "Вывести общее количество разных видов катастроф");
            t.Show();
        }

        private void вывестиОбщееЧислоКатастрофИПоДесятилеткамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 12, "Вывести общее число катастроф и по десятилеткам");
            t.Show();
        }

        private void ывестиКоличествоПерсоналаСДолжностьюСотрудникПоГородамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 15, "Вывести количество персонала с должностью \"Сотрудник\" по городам");
            t.Show();
        }

        private void вывестиВсеПредприятияИКоличествоЭкологическихКатастрофToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 16, "Вывести все предприятия и количество экологических катастроф");
            t.Show();
        }

        private void вывестиКатастрофыИСуммарноеЧислоПострадавшихОтНихToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 19, "Вывести катастрофы и суммарное число пострадавших от них");
            t.Show();
        }

        private void вывестиСуммарноеЧислоПредприятийИОтделовЭСУНаКаждыйГородToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 20, "Вывести суммарное число предприятий и отделов ЭСУ на каждый город");
            t.Show();
        }

        private void определить10ГородовСНаибольшимЧисломКатастрофИЖертвToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 24, "Определить 10 городов с наибольшим числом катастроф и жертв");
            t.Show();
        }

        private void вывестиВсеУчастияВУстраненииОпределеннойКатастрофыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 1, "Вывести все участия в устранении определенной катастрофы");
            t.Show();
        }

        private void вывестиВсехРаботниковОпределенногоОтделаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 2, "Вывести всех работников определенного отдела");
            t.Show();
        }

        private void вывестиВсеПредприятияЗарегистрированныеДоОпределенногоГодаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 4, "Вывести все предприятия, зарегистрированные до определенного года");
            t.Show();
        }

        private void вывестиОтделыВКоторыхНеМенееОпределенногоЧислаСотрудниковToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 21, "Вывести отделы, в которых не менее определенного числа сотрудников");
            t.Show();
        }

        private void вывестиОтделыКоторыеОснованыНеПозжеОпределенногоГодаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 22, "Вывести отделы, которые основаны не позже определенного года");
            t.Show();
        }

        private void вывестиКоличествоСотрудниковФамилииКоторыхНачинаютсяНаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 14, "Вывести количество сотрудников, фамилии которых начинаются на ...");
            t.Show();
        }

        private void вывестиКатастрофыСуммарныйМатериальныйУщербКоторыхПревышаетУказанноеЗначениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 17, "Вывести катастрофы, суммарный материальный ущерб которых превышает указанное значение");
            t.Show();
        }

        private void вывестиВсеКатастрофыВПромежуткеВремениToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 3, "Вывести все катастрофы в промежутке времени");
            t.Show();
        }

        private void вывестиКоличествоУчастийОтделовПоКатастрофамПроизошедшимВОпределенныйПериодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 13, "Вывести количество участий отделов по катастрофам, произошедшим в определенный период");
            t.Show();
        }

        private void вывестиКатастрофыЗаОпределенныйПериодСуммарныйМатериальныйУщербКоторыхПревышаетУказанноеЗначениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 18, "Вывести катастрофы за определенный период, суммарный материальный ущерб которых превышает указанное значение");
            t.Show();
        }

        private void вывестиКоличествоКатастрофРазногоТипаПроизошедшийВОпределенныйПромежутокВремениToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Tasks t = new Tasks(conn, 23, "Вывести количество катастроф разного типа, произошедший в определенный промежуток времени");
            t.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Generator g = new Generator(conn);
            g.Run();
        }

        private void вывестиКатастрофыИСуммарноеЧислоПострадавшихОтНихToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Visualisation v = new Visualisation(conn, 1);
            v.Show();
        }

        private void вывестиКоличествоКатастрофРазногоТипаПроизошедшийВОпределенныйПромежутокВремениToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Visualisation v = new Visualisation(conn, 2);
            v.Show();
        }

        private void вывестиКоличествоОтделовЭСУИПредприятийПоГородамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visualisation v = new Visualisation(conn, 3);
            v.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Generator g = new Generator(conn, false);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Generator g = new Generator(conn, true);
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
    }
}
