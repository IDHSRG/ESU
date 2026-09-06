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
    public partial class Users : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        DataTable dt;
        MainForm mf;
        ForHeadDeps fhd;
        ForEmployees fe;
        string role;
        public Users(NpgsqlConnection c, ForEmployees f)
        {
            InitializeComponent();
            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            conn = c; fe = f;
            dt = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM workers_department_view", conn);
            dt.Load(comm.ExecuteReader());
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            role = "employee";
        }
        public Users(NpgsqlConnection c, MainForm f, int isAdmin)
        {
            InitializeComponent();
            conn = c; mf = f;
            dt = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM workers_all_view", conn);
            dt.Load(comm.ExecuteReader());
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            if (isAdmin == 1)
                role = "admin";
            else
                role = "moderator";
        }
        public Users(NpgsqlConnection c, ForHeadDeps f)
        {
            conn = c; fhd = f;
            InitializeComponent();
            dt = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM workers_department_view", conn);
            dt.Load(comm.ExecuteReader());
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            role = "head";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddUser au;
            switch (role) 
            {
                case "admin":
                    au = new AddUser(conn, this, 1);
                    au.Show();
                    break;

                case "moderator":
                    au = new AddUser(conn, this, 0);
                    au.Show();
                    break;

                case "head":
                    au = new AddUser(conn, this,  -1);
                    au.Show();
                    break;
            } 
        }

        public void RefreshTable() 
        {
            dt = new DataTable();
            switch (role)
            {
                case "admin":
                    comm = new NpgsqlCommand("SELECT * FROM workers_all_view", conn);
                    break;
                case "head":
                    comm = new NpgsqlCommand("SELECT * FROM workers_department_view", conn);
                    break;
                case "employee":
                    comm = new NpgsqlCommand("SELECT * FROM workers_department_view", conn);
                    break;
            }
            dt.Load(comm.ExecuteReader());
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int deleted = 0;
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                try
                {
                    comm = new NpgsqlCommand("SELECT * FROM delete_worker(" + row.Cells[0].Value.ToString() + ")", conn);
                    deleted += comm.ExecuteNonQuery();
                }
                catch (PostgresException err)
                {
                    if (err.MessageText.Contains("пользователя текущего сеанса нельзя удалить"))
                        MessageBox.Show("Вы не можете удалить себя");
                    if (err.MessageText.Contains("delete admin"))
                        MessageBox.Show("Администратор не может быть удалён");
                }
            }
            if (deleted == 0)
                MessageBox.Show("Не было удалено ни одного пользователя");
            else
                MessageBox.Show("Было успешно удалено " + (-1 * deleted).ToString() + " пользователей!");
            RefreshTable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 1 || dataGridView1.SelectedRows.Count < 1)
                MessageBox.Show("Для редактирования необходимо выбрать одно поле. Повторите попытку, пожалуйста");
            else
            {
                AddUser au;
                switch (role)
                {
                    case "admin":
                        au = new AddUser(conn, this, 1, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                        au.Show();
                        break;

                    case "moderator":
                        au = new AddUser(conn, this, 0, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                        au.Show();
                        break;

                    case "head":
                        au = new AddUser(conn, this, -1, dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                        au.Show();
                        break;
                }
            }
        }
    }
}
