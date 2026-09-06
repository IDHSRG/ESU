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
    public partial class AddUser : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        Users usrs;
        WorkersOfDepartment wk;
        DataTable dt , dt0;
        string idUp;
        bool isUpdate = false; bool isUndefined;
        public AddUser(NpgsqlConnection c, Users u, int isAdmin)
        {
            InitializeComponent();
            usrs = u;
            conn = c; string[] roles;
            if (isAdmin != -1) 
            {
                if (isAdmin == 1)
                    roles = new string[] { "Сотрудник", "Глава отдела", "Менеджер", "Модератор" };
                else
                    roles = new string[] { "Сотрудник", "Глава отдела", "Менеджер" };
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM departments_all_view", conn);
                dt.Load(comm.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                    comboBox1.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
            }

            else
            {
                roles = new string[] { "Сотрудник" };
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM departments_user_view", conn);
                dt.Load(comm.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                    comboBox1.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
                comboBox2.Enabled = false;
                comboBox1.Enabled = false;
                
            }
            foreach (string r in roles)
                comboBox2.Items.Add(r);
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 1;
        }

        public AddUser(NpgsqlConnection c, Users u, int isAdmin, string id)
        {
            InitializeComponent();
            isUpdate = true;
            button1.Text = "Сохранить";
            int i;
            idUp = id;
            usrs = u;
            conn = c; string[] roles;
            if (isAdmin != -1)
            {
                if (isAdmin == 1)
                    roles = new string[] { "Сотрудник", "Глава отдела", "Менеджер", "Модератор" };
                else
                    roles = new string[] { "Сотрудник", "Глава отдела", "Менеджер" };
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM departments_all_view", conn);
                dt.Load(comm.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                    comboBox1.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
            }
            else
            {
                roles = new string[] { "Сотрудник" };
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM departments_user_view", conn);
                dt.Load(comm.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                    comboBox1.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
                comboBox2.Enabled = false;
                comboBox1.Enabled = false;

            }
            foreach (string r in roles)
                comboBox2.Items.Add(r);
            textBox2.Visible = false;
            dt0 = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM data_worker(" + id + ")", conn);
            dt0.Load(comm.ExecuteReader());
            textBox1.Text = dt0.Rows[0][0].ToString();
            textBox4.Text = dt0.Rows[0][1].ToString();
            textBox3.Text = dt0.Rows[0][2].ToString();
            textBox5.Text = dt0.Rows[0][3].ToString();
            maskedTextBox1.Text = dt0.Rows[0][4].ToString();
            if (dt0.Rows[0][5].ToString().Contains("+7"))
            {
                comboBox3.SelectedIndex = 0;
                maskedTextBox2.Text = dt0.Rows[0][5].ToString().Remove(0, 3);
            }
            else
            {
                comboBox3.SelectedIndex = 1;
                maskedTextBox2.Text = dt0.Rows[0][5].ToString().Remove(0, 4);
            }
            i = 0;
            foreach (string row in comboBox2.Items)
            {
                if (row == dt0.Rows[0][6].ToString())
                {
                    comboBox2.SelectedIndex = i;
                    break;
                }
                i++;
            }
            isUndefined = false;
            if (i > comboBox2.Items.Count-1)
            {
                comboBox2.Items.Add(dt0.Rows[0][6].ToString());
                comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
                comboBox2.Enabled = false; isUndefined = true;
            }
            i = 0;
            foreach (DataRow row in dt.Rows)
            {
                if (row[0].ToString() == dt0.Rows[0][7].ToString())
                {
                    comboBox1.SelectedIndex = i;
                    break;
                }
                i++;
            }
        }

        public AddUser(NpgsqlConnection c, WorkersOfDepartment w, int isAdmin, string id, bool isEdit)
        {
            InitializeComponent();
            wk = w;
            conn = c; string[] roles;
            int i;
            idUp = id;
            if (isAdmin != -1)
            {
                if (isAdmin == 1)
                    roles = new string[] { "Сотрудник", "Глава отдела", "Менеджер", "Модератор" };
                else
                    roles = new string[] { "Сотрудник", "Глава отдела", "Менеджер" };
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM departments_all_view", conn);
                dt.Load(comm.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                    comboBox1.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
            }

            else
            {
                roles = new string[] { "Сотрудник" };
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM departments_user_view", conn);
                dt.Load(comm.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                    comboBox1.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
                comboBox2.Enabled = false;
                comboBox1.Enabled = false;

            }
            foreach (string r in roles)
                comboBox2.Items.Add(r);
            if (isEdit)
            {
                isUpdate = true;
                textBox2.Visible = false;
                dt0 = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM data_worker(" + id + ")", conn);
                dt0.Load(comm.ExecuteReader());
                textBox1.Text = dt0.Rows[0][0].ToString();
                textBox4.Text = dt0.Rows[0][1].ToString();
                textBox3.Text = dt0.Rows[0][2].ToString();
                textBox5.Text = dt0.Rows[0][3].ToString();
                maskedTextBox1.Text = dt0.Rows[0][4].ToString();
                if (dt0.Rows[0][5].ToString().Contains("+7"))
                {
                    comboBox3.SelectedIndex = 0;
                    maskedTextBox2.Text = dt0.Rows[0][5].ToString().Remove(0, 3);
                }
                else
                {
                    comboBox3.SelectedIndex = 1;
                    maskedTextBox2.Text = dt0.Rows[0][5].ToString().Remove(0, 4);
                }
                i = 0;
                foreach (string row in comboBox2.Items)
                {
                    if (row == dt0.Rows[0][6].ToString())
                    {
                        comboBox2.SelectedIndex = i;
                        break;
                    }
                    i++;
                }
                isUndefined = false;
                if (i > comboBox2.Items.Count - 1)
                {
                    comboBox2.Items.Add(dt0.Rows[0][6].ToString());
                    comboBox2.SelectedIndex = comboBox2.Items.Count - 1;
                    comboBox2.Enabled = false; isUndefined = true;
                }
                i = 0;
                if (isAdmin != -1)
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][7].ToString())
                        {
                            comboBox1.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                else
                    comboBox1.SelectedIndex = 0;
            }
            else
            {
                comboBox2.SelectedIndex = 0;
                comboBox3.SelectedIndex = 1;
                i = 0;
                dt = new DataTable();
                comm = new NpgsqlCommand("SELECT * FROM departments_all_view", conn);
                dt.Load(comm.ExecuteReader());
                if (isAdmin != -1)
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == id)
                        {
                            comboBox1.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                else
                    comboBox1.SelectedIndex = 0;
            }
      
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (isUpdate)
            {
                try
                {
                    string dep = comboBox1.Text.Substring(comboBox1.Text.IndexOf('#') + 1, comboBox1.Text.IndexOf(']') - 2);
                if (isUndefined)
                    comm = new NpgsqlCommand("SELECT * FROM update_worker(" + idUp + ", " + dep + ", \'" + textBox3.Text + "\', \'" + textBox4.Text + "\', \'" + textBox5.Text + "\', (\'" + maskedTextBox1.Text.Replace(',', '.') + "\')::date, "
                    + "\'" + textBox1.Text + "\', \'" + comboBox3.Text + " " + maskedTextBox2.Text + "\')", conn);
                else
                comm = new NpgsqlCommand("SELECT * FROM update_worker("+idUp+", "+ dep + ", \'" + textBox3.Text + "\', \'" + textBox4.Text + "\', \'" + textBox5.Text + "\', (\'" + maskedTextBox1.Text.Replace(',','.') + "\')::date, \'" + comboBox2.Text + "\', "
                    + "\'" + textBox1.Text + "\', \'" + comboBox3.Text + " " + maskedTextBox2.Text + "\')", conn);

                    comm.ExecuteNonQuery();
                    MessageBox.Show("Пользователь изменён успешно!");
                    this.Close();
                }
                catch (PostgresException err)
                {
                    if (err.MessageText.Contains("значение домена phone_number нарушает ограничение-проверку"))
                        MessageBox.Show("Введенный вами номер телефона неверен, повторите попытку, пожалуйста!");
                    else if (err.MessageText.Contains("Invalid name"))
                        MessageBox.Show("Имя и фамилия неверны, повторите попытку, пожалуйста!");
                    else if (err.MessageText.Contains("неверный синтаксис для типа date"))
                        MessageBox.Show("Неверно введена дата рождения, повторите попытку!");
                    else if (err.MessageText.Contains("Invalid date of birth"))
                        MessageBox.Show("Введенная дата рождения недопустима, повторите попытку, пожалуйста!");
                    else if (err.MessageText.Contains("Empty login"))
                        MessageBox.Show("Вы не ввели логин, повторите попытку!");
                    else if (err.MessageText.Contains("Invalid login"))
                        MessageBox.Show("Пользователь с таким логином уже существует, повторите попытку!");
                    else if (err.MessageText.Contains("Invalid password"))
                        MessageBox.Show("Пароль не может быть пустым, повторите попытку!");
                    else if (err.MessageText.Contains("пользователя текущего сеанса нельзя переименовать"))
                        MessageBox.Show("Вы не можете изменить свой логин, для замены логина запросите изменение у главы вашего отдела или модератора!");
                    else
                        MessageBox.Show("Что-то не так. Повторите попытку позже.");
                }
                catch (Exception err)
                {
                    if (err.Message == "В позиции -1 строка отсутствует.")
                        MessageBox.Show("Вы не ввели все необходимые данные, повторите попытку!");
                }
            }
            else
            {
                try
                {
                    string dep = comboBox1.Text.Substring(comboBox1.Text.IndexOf('#') + 1, comboBox1.Text.IndexOf(']') - 2);
                comm = new NpgsqlCommand("SELECT * FROM add_worker(" + dep + ", \'" + textBox3.Text + "\', \'" + textBox4.Text + "\', \'" + textBox5.Text + "\', (\'" + maskedTextBox1.Text + "\')::date, \'" + comboBox2.Text + "\', "
                    + "\'" + textBox1.Text + "\', \'" + comboBox3.Text + " " + maskedTextBox2.Text + "\', \'" + textBox2.Text + "\')", conn); ;
                    comm.ExecuteNonQuery();
                    MessageBox.Show("Пользователь успешно добавлен!");
                    this.Close();
                }
                catch (PostgresException err)
                {
                    if (err.MessageText.Contains("значение домена phone_number нарушает ограничение-проверку"))
                        MessageBox.Show("Введенный вами номер телефона неверен, повторите попытку, пожалуйста!");
                    else if (err.MessageText.Contains("Invalid name"))
                        MessageBox.Show("Имя и фамилия неверны, повторите попытку, пожалуйста!");
                    else if (err.MessageText.Contains("неверный синтаксис для типа date"))
                        MessageBox.Show("Неверно введена дата рождения, повторите попытку!");
                    else if (err.MessageText.Contains("Invalid date of birth"))
                        MessageBox.Show("Введенная дата рождения недопустима, повторите попытку, пожалуйста!");
                    else if (err.MessageText.Contains("Empty login"))
                        MessageBox.Show("Вы не ввели логин, повторите попытку!");
                    else if (err.MessageText.Contains("Invalid login"))
                        MessageBox.Show("Пользователь с таким логином уже существует, повторите попытку!");
                    else if (err.MessageText.Contains("Invalid password"))
                        MessageBox.Show("Пароль не может быть пустым, повторите попытку!");
                    else
                        MessageBox.Show("Что-то не так. Повторите попытку позже.");
                }
                catch (Exception err)
                {
                    if (err.Message == "В позиции -1 строка отсутствует.")
                        MessageBox.Show("Вы не ввели все необходимые данные, повторите попытку!");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddUser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (usrs != null)
                usrs.RefreshTable();
            if (wk != null)
                wk.RefreshTable();
        }
    }
}
