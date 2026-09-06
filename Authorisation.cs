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
    public partial class Authorisation : Form
    {
        NpgsqlConnection conn;
        public Authorisation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           string connString = "Server = localhost; Port = 5432; User ID= " + textBox1.Text + "; Password = " + textBox2.Text + "; Database = ESU;";
           try
            {

                conn = new NpgsqlConnection(connString);
                conn.Open();
                NpgsqlCommand comm = new NpgsqlCommand("SELECT check_role_in()", conn);
                NpgsqlDataReader dr = comm.ExecuteReader();
                int res = -1;
                
                while (dr.Read()){ res = Int32.Parse(dr[0].ToString()); }
                dr.Close();
                switch (res) 
                {
                    case 1:
                        StartAdmin(textBox1.Text, true);
                        break;
                    case 2:
                        StartAdmin(textBox1.Text, false);
                        break;
                    case 3:
                        StartManager();
                        break;
                    case 4:
                        StartHead(textBox1.Text);
                        break;
                    default:
                        StartEmployee(textBox1.Text);
                        break;
                }
                this.Hide();
           }
           catch (NpgsqlException)
           {
               MessageBox.Show("Не удаётся подключиться к базе данных. Возможно, вы ввели неправильный логин или пароль. Повторите попытку.");
            }
        }

        private void Authorisation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn != null)
                try{conn.Close();}catch { }
        }

        private void StartAdmin(string a, bool isAdmin) 
        {
           MainForm main = new MainForm(textBox1.Text, isAdmin, conn); main.Show();
        }
        private void StartEmployee(string a)
        {
            ForEmployees main = new ForEmployees(textBox1.Text, conn);
            main.Show();
        }
        private void StartHead(string a)
        {
            ForHeadDeps main = new ForHeadDeps(textBox1.Text, conn);
            main.Show();
        }
        private void StartManager()
        {
            OnlyReferences main = new OnlyReferences(conn);
            main.Show();
        }
    }
}
