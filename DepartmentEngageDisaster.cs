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
    public partial class DepartmentEngageDisaster : Form
    {
        MainForm mf;
        ForHeadDeps fhd;
        ForEmployees fe;
        string role, idDis;
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        DataTable dt0 = new DataTable();
        DataTable dt = new DataTable();

        public DepartmentEngageDisaster(NpgsqlConnection c, MainForm form, string id)
        {
            InitializeComponent();
            role = "admin";
            mf = form; conn = c;
            idDis = id;
            dt0 = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM get_disaster_of_view("+ id+")", conn);
            dt0.Load(comm.ExecuteReader());
            groupDepartments.Text += idDis;
            label1.Text = dt0.Rows[0][1].ToString();
            label2.Text = dt0.Rows[0][2].ToString();
            label3.Text = dt0.Rows[0][3].ToString();
            label9.Text = dt0.Rows[0][4].ToString().Substring(0, dt0.Rows[0][4].ToString().IndexOf(' '));
            RefreshTable();
        }
        public DepartmentEngageDisaster(NpgsqlConnection c, ForHeadDeps form, string id)
        {
            InitializeComponent();
            role = "head";
            fhd = form; conn = c;
            idDis = id;
            dt0 = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM get_disaster_of_view(" + id + ")", conn);
            dt0.Load(comm.ExecuteReader());
            groupDepartments.Text += idDis;
            label1.Text = dt0.Rows[0][1].ToString();
            label2.Text = dt0.Rows[0][2].ToString();
            label3.Text = dt0.Rows[0][3].ToString();
            label9.Text = dt0.Rows[0][4].ToString();
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
            }
            this.Close();
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            RefreshTable();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        public void RefreshTable()
        {
            dt = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM department_engage_disaster("+idDis+")", conn);
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
            }
        }
    }
}
