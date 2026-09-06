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
    public partial class WorkersOfDepartment : Form
    {
        MainForm mf;
        ForHeadDeps fhd;
        ForEmployees fe;
        string role, idDep;
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        DataTable dt0 = new DataTable();
        DataTable dt = new DataTable();

        public WorkersOfDepartment(NpgsqlConnection c, MainForm form, string id, bool isAdmin)
        {
            InitializeComponent();
            if (isAdmin)
                role = "admin";
            else role = "moderator";
            mf = form; conn = c;
            idDep = id;
            dt0 = new DataTable();
            if (idDep == "-1")
            {
                comm = new NpgsqlCommand("SELECT * FROM get_department_of_view()", conn);
                dt0.Load(comm.ExecuteReader());
                idDep = dt0.Rows[0][0].ToString();
            }
            else
            {
                comm = new NpgsqlCommand("SELECT * FROM get_department_of_view(" + id + ")", conn);
                dt0.Load(comm.ExecuteReader());
                idDep = dt0.Rows[0][0].ToString();

            }
            RefreshTable();
        }
        public WorkersOfDepartment(NpgsqlConnection c, ForHeadDeps form, string id)
        {
            InitializeComponent();
            fhd = form; conn = c;
            idDep = id;
            role = "head";
            dt0 = new DataTable();
            if (idDep == "-1")
                comm = new NpgsqlCommand("SELECT * FROM get_department_of_view()", conn);
            else
            {
                button4.Visible = false;
                button3.Visible = false;
                button2.Visible = false;
                comm = new NpgsqlCommand("SELECT * FROM get_department_of_view(" + id + ")", conn);
            }
            dt0.Load(comm.ExecuteReader());
            idDep = dt0.Rows[0][0].ToString();
            RefreshTable();
        }
        public WorkersOfDepartment(NpgsqlConnection c, ForEmployees form)
        {
            InitializeComponent();
            fe = form; conn = c; role = "employee";
            button4.Visible = false;
            button3.Visible = false;
            button2.Visible = false;
            dt0 = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM get_department_of_view()", conn);
            dt0.Load(comm.ExecuteReader());
            idDep = dt0.Rows[0][0].ToString();
            RefreshTable();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (role)
            {
                case "admin":
                    mf.RefreshTable();
                    break;
                case "head":
                    fhd.RefreshTable();
                    break;
                case "employee":
                    fe.RefreshTable();
                    break;
            }
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddUser au;
            switch (role)
            {
                case "admin":
                    au = new AddUser(conn, this, 1, idDep, false);
                    au.Show();
                    break;

                case "moderator":
                    au = new AddUser(conn, this, 0, idDep, false);
                    au.Show();
                    break;

                case "head":
                    au = new AddUser(conn, this, -1, idDep, false);
                    au.Show();
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 1 || dataGridView1.SelectedRows.Count < 1)
                MessageBox.Show("Для редактирования необходимо выбрать одно поле. Повторите попытку, пожалуйста");
            else
            {
                AddUser au;
                switch (role)
                {
                    case "admin":
                        au = new AddUser(conn, this, 1, dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), true);
                        au.Show();
                        break;

                    case "moderator":
                        au = new AddUser(conn, this, 0, dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), true);
                        au.Show();
                        break;

                    case "head":
                        au = new AddUser(conn, this, -1, dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), true);
                        au.Show();
                        break;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RefreshTable();
        }

        public void RefreshTable()
        {
            dt0 = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM get_department_of_view(" + idDep + ")", conn);
            dt0.Load(comm.ExecuteReader());
            groupDepartments.Text = "Номер отдела: "+idDep;
            label1.Text = dt0.Rows[0][1].ToString();
            label2.Text = dt0.Rows[0][2].ToString();
            label3.Text = dt0.Rows[0][4].ToString();
            label9.Text = dt0.Rows[0][3].ToString();
            label10.Text = dt0.Rows[0][5].ToString();
            dt = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM task_workers_of_department("+idDep+")", conn);
            dt.Load(comm.ExecuteReader());
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt;
            dataGridView1.Refresh();
            switch (role)
            {
                case "admin":
                   mf.RefreshTable();
                break;
                case "head":
                    fhd.RefreshTable();
                break;
                case "employee":
                    fe.RefreshTable();
                break;
            }
        }
    }
}
