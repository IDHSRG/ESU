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
    public partial class AddElement : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        DataTable dt0 = new DataTable();
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        ForEmployees f1;
        ForHeadDeps f2;
        MainForm f3;
        string elem, role, idUp;
        bool isUpdate; 
        public AddElement(NpgsqlConnection c, ForEmployees fe)
        {
            InitializeComponent();
            groupConsequences.Visible = false;
            groupDepartments.Visible = false;
            groupEngagements.Visible = false;
            groupCompanies.Visible = false;
            conn = c;
            elem = "disaster"; role = "empl";

            dt = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM companies_city_view", conn);
            dt.Load(comm.ExecuteReader());

            f1 = fe;

            foreach(DataRow row in dt.Rows)
                comboBox1.Items.Add("[#"+row[0].ToString()+"] " + "\""+row[1].ToString() + "\" - " + row[2].ToString());

            dt = new DataTable();
            comm = new NpgsqlCommand("SELECT * FROM type_disaster_view", conn);
            dt.Load(comm.ExecuteReader());

            foreach (DataRow row in dt.Rows)
                comboBox2.Items.Add(row[1].ToString());
        }

        public AddElement(NpgsqlConnection c, ForHeadDeps fe, string elementName)
        {
            InitializeComponent();
            groupConsequences.Visible = false;
            groupDepartments.Visible = false;
            groupDisasters.Visible = false;
            groupEngagements.Visible = false;
            groupCompanies.Visible = false;
            conn = c; f2 = fe; role = "head"; elem = elementName;
            comboBox8.SelectedIndex = 1; comboBox11.SelectedIndex = 1;
            switch (elementName)
            {

                case "consequence":
                    groupConsequences.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM disasters_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox5.Items.Add("[#" + row[0].ToString() + "] " + row[3].ToString() + " - " + row[1].ToString() + "(" + row[2].ToString() + ")");
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM kind_disaster_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox3.Items.Add(row[1].ToString());
                    break;
                case "disaster":
                    groupDisasters.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM companies_city_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox1.Items.Add("[#" + row[0].ToString() + "] " + "\"" + row[1].ToString() + "\" - " + row[2].ToString());
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM type_disaster_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox2.Items.Add(row[1].ToString());
                    break;
                case "engagement":
                    groupEngagements.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM disasters_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox10.Items.Add("[#" + row[0].ToString() + "] " + row[3].ToString() + " - " + row[1].ToString()+"("+ row[2].ToString() + ")");
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM departments_user_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox9.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
                    comboBox9.SelectedIndex = 0;
                    comboBox9.Enabled = false;
                    break;
                case "company":
                    groupCompanies.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM industries_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox7.Items.Add(row[1].ToString());
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM cities_own", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox6.Items.Add(row[1].ToString());
                    comboBox6.SelectedIndex = 0;
                    comboBox6.Enabled = false;
                    break;
            }
        }

        public AddElement(NpgsqlConnection c, ForHeadDeps fe, string elementName, string id)
        {
            InitializeComponent();
            this.Text = "Изменить";
            button1.Text = "Сохранить";
            int i;
            groupConsequences.Visible = false;
            groupDepartments.Visible = false;
            groupDisasters.Visible = false;
            groupEngagements.Visible = false;
            groupCompanies.Visible = false;
            conn = c; f2 = fe; role = "head"; elem = elementName; idUp = id; isUpdate = true;
            switch (elementName)
            {
                case "consequence":
                    groupConsequences.Visible = true;

                    dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM data_consequence(" + id + ")", conn);
                    dt0.Load(comm.ExecuteReader());
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM disasters_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox5.Items.Add("[#" + row[0].ToString() + "] " + row[3].ToString() + " - " + row[1].ToString() + "(" + row[2].ToString() + ")");
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM kind_disaster_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox3.Items.Add(row[1].ToString());
                    i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][0].ToString())
                        {
                            comboBox5.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    i = 0;
                    foreach (DataRow row in dt2.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][1].ToString())
                        {
                            comboBox3.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    break;
                case "disaster": //
                    groupDisasters.Visible = true;

                    dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM data_disaster(" + id + ")", conn);
                    dt0.Load(comm.ExecuteReader());
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM companies_city_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox1.Items.Add("[#" + row[0].ToString() + "] " + "\"" + row[1].ToString() + "\" - " + row[2].ToString());
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM type_disaster_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox2.Items.Add(row[1].ToString());
                    i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][0].ToString())
                        {
                            comboBox1.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    i = 0;
                    foreach (DataRow row in dt2.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][1].ToString())
                        {
                            comboBox2.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    maskedTextBox1.Text = dt0.Rows[0][2].ToString();
                    break;
                case "engagement": //
                    groupEngagements.Visible = true;

                    dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM data_engagement(" + id + ")", conn);
                    dt0.Load(comm.ExecuteReader());
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM disasters_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox10.Items.Add("[#" + row[0].ToString() + "] " + row[3].ToString() + " - " + row[1].ToString() + "(" + row[2].ToString() + ")");
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM departments_user_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox9.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
                    comboBox9.SelectedIndex = 0;
                    comboBox9.Enabled = false;
                    i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][0].ToString())
                        {
                            comboBox10.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    numericUpDown7.Value = Int32.Parse(dt0.Rows[0][1].ToString());
                    numericUpDown5.Value = Int32.Parse(dt0.Rows[0][2].ToString());
                    break;
                case "company": //
                    groupCompanies.Visible = true;

                    dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM data_company(" + id + ")", conn);
                    dt0.Load(comm.ExecuteReader());
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM industries_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox7.Items.Add(row[1].ToString());
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM cities_own", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox6.Items.Add(row[1].ToString());
                    comboBox6.SelectedIndex = 0;
                    comboBox6.Enabled = false;

                    textBox2.Text = dt0.Rows[0][0].ToString();
                    numericUpDown6.Value = Int32.Parse(dt0.Rows[0][4].ToString());
                    if (dt0.Rows[0][3].ToString().Contains("+7"))
                    {
                        comboBox11.SelectedIndex = 0;
                        maskedTextBox3.Text = dt0.Rows[0][3].ToString().Remove(0, 3);
                    }
                    else
                    {
                        comboBox11.SelectedIndex = 1;
                        maskedTextBox3.Text = dt0.Rows[0][3].ToString().Remove(0, 4);
                    }
                    i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][2].ToString())
                        {
                            comboBox7.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    break;
                case "department":

                    dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM data_department(" + id + ")", conn);
                    dt0.Load(comm.ExecuteReader());
                    groupDepartments.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM cities_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox4.Items.Add(row[1].ToString());
                    textBox1.Text = dt0.Rows[0][0].ToString();
                    i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][1].ToString())
                        {
                            comboBox4.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    if (dt0.Rows[0][3].ToString().Contains("+7"))
                    {
                        comboBox8.SelectedIndex = 0;
                        maskedTextBox2.Text = dt0.Rows[0][4].ToString().Remove(0, 3);
                    }
                    else
                    {
                        comboBox8.SelectedIndex = 1;
                        maskedTextBox2.Text = dt0.Rows[0][4].ToString().Remove(0, 4);
                    }
                    numericUpDown1.Value = Int32.Parse(dt0.Rows[0][2].ToString());
                    numericUpDown2.Value = Int32.Parse(dt0.Rows[0][3].ToString());
                    numericUpDown1.Enabled = false; numericUpDown2.Enabled = false; comboBox4.Enabled = false;
                    break;
            }
        }

        public AddElement(NpgsqlConnection c, MainForm mf, string elementName)
        {
            InitializeComponent();
            groupConsequences.Visible = false;
            groupDepartments.Visible = false;
            groupDisasters.Visible = false;
            groupEngagements.Visible = false;
            groupCompanies.Visible = false;
            conn = c; f3 = mf; role = "admin"; elem = elementName;
            comboBox8.SelectedIndex = 1; comboBox11.SelectedIndex = 1;

            switch (elementName)
            {
                case "consequence":
                    groupConsequences.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM disasters_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox5.Items.Add("[#" + row[0].ToString() + "] " + row[3].ToString() + " - " + row[1].ToString() + "(" + row[2].ToString() + ")");
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM kind_disaster_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox3.Items.Add(row[1].ToString());
                    break;
                case "disaster":
                    groupDisasters.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM companies_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox1.Items.Add("[#" + row[0].ToString() + "] " + "\"" + row[1].ToString() + "\" - " + row[2].ToString());
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM type_disaster_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox2.Items.Add(row[1].ToString());
                    break;
                case "engagement":
                    groupEngagements.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM disasters_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox10.Items.Add("[#" + row[0].ToString() + "] " + row[3].ToString() + " - " + row[1].ToString() + "(" + row[2].ToString() + ")");
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM departments_all_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox9.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
                    break;
                case "company":
                    groupCompanies.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM industries_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox7.Items.Add(row[1].ToString());
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM cities_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox6.Items.Add(row[1].ToString());
                    break;
                case "department":
                    groupDepartments.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM cities_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox4.Items.Add(row[1].ToString());
                    break;
            }
        }

        public AddElement(NpgsqlConnection c, MainForm mf, string elementName, string id)
        {
            InitializeComponent();
            this.Text = "Изменить";
            button1.Text = "Сохранить";
            int i;
            groupConsequences.Visible = false;
            groupDepartments.Visible = false;
            groupDisasters.Visible = false;
            groupEngagements.Visible = false;
            groupCompanies.Visible = false;
            conn = c; f3 = mf; role = "admin"; elem = elementName; idUp = id; isUpdate = true;
            switch (elementName)
            {
                case "consequence":
                    groupConsequences.Visible = true;

                    dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM data_consequence(" + id + ")", conn);
                    dt0.Load(comm.ExecuteReader());
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM disasters_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox5.Items.Add("[#" + row[0].ToString() + "] " + row[3].ToString() + " - " + row[1].ToString() + "(" + row[2].ToString() + ")");
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM kind_disaster_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox3.Items.Add(row[1].ToString());
                    i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][0].ToString())
                        {
                            comboBox5.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    i = 0;
                    foreach (DataRow row in dt2.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][1].ToString())
                        {
                            comboBox3.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    break;
                case "disaster":
                    groupDisasters.Visible = true;

                    dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM data_disaster(" + id + ")", conn);
                    dt0.Load(comm.ExecuteReader());
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM companies_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox1.Items.Add("[#" + row[0].ToString() + "] " + "\"" + row[1].ToString() + "\" - " + row[2].ToString());
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM type_disaster_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox2.Items.Add(row[1].ToString());
                    i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][0].ToString())
                        {
                            comboBox1.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    i = 0;
                    foreach (DataRow row in dt2.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][1].ToString())
                        {
                            comboBox2.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    maskedTextBox1.Text = dt0.Rows[0][2].ToString();
                    break;
                case "engagement":
                    groupEngagements.Visible = true;

                    dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM data_engagement(" + id + ")", conn);
                    dt0.Load(comm.ExecuteReader());
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM disasters_all_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox10.Items.Add("[#" + row[0].ToString() + "] " + row[3].ToString() + " - " + row[1].ToString() + "(" + row[2].ToString() + ")");
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM departments_all_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox9.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
                    i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][0].ToString())
                        {
                            comboBox10.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    i = 0;
                    foreach (DataRow row in dt2.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][3].ToString())
                        {
                            comboBox9.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    numericUpDown7.Value = Int32.Parse(dt0.Rows[0][1].ToString());
                    numericUpDown5.Value = Int32.Parse(dt0.Rows[0][2].ToString());
                    break;
                case "company":
                    groupCompanies.Visible = true;

                    dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM data_company(" + id + ")", conn);
                    dt0.Load(comm.ExecuteReader());
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM industries_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox7.Items.Add(row[1].ToString());
                    dt2 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM cities_view", conn);
                    dt2.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt2.Rows)
                        comboBox6.Items.Add(row[1].ToString());
                    textBox2.Text = dt0.Rows[0][0].ToString();
                    numericUpDown6.Value = Int32.Parse(dt0.Rows[0][4].ToString());
                    if (dt0.Rows[0][3].ToString().Contains("+7"))
                    {
                        comboBox11.SelectedIndex = 0;
                        maskedTextBox3.Text = dt0.Rows[0][3].ToString().Remove(0, 3);
                    }
                    else
                    {
                        comboBox11.SelectedIndex = 1;
                        maskedTextBox3.Text = dt0.Rows[0][3].ToString().Remove(0, 4);
                    }
                    i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][2].ToString())
                        {
                            comboBox7.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    foreach (DataRow row in dt2.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][1].ToString())
                        {
                            comboBox6.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    break;
                case "department":

                    dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM data_department(" + id + ")", conn);
                    dt0.Load(comm.ExecuteReader());
                    groupDepartments.Visible = true;
                    dt = new DataTable();
                    comm = new NpgsqlCommand("SELECT * FROM cities_view", conn);
                    dt.Load(comm.ExecuteReader());
                    foreach (DataRow row in dt.Rows)
                        comboBox4.Items.Add(row[1].ToString());
                    textBox1.Text = dt0.Rows[0][0].ToString();
                    i = 0;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() == dt0.Rows[0][1].ToString())
                        {
                            comboBox4.SelectedIndex = i;
                            break;
                        }
                        i++;
                    }
                    if (dt0.Rows[0][3].ToString().Contains("+7"))
                    {
                        comboBox8.SelectedIndex = 0;
                        maskedTextBox2.Text = dt0.Rows[0][4].ToString().Remove(0, 3);
                    }
                    else
                    {
                        comboBox8.SelectedIndex = 1;
                        maskedTextBox2.Text = dt0.Rows[0][4].ToString().Remove(0, 4);
                    }
                    numericUpDown1.Value = Int32.Parse(dt0.Rows[0][2].ToString());
                    numericUpDown2.Value = Int32.Parse(dt0.Rows[0][3].ToString());
                    break;
            }
        }

        private void groupConsequences_Enter(object sender, EventArgs e)  {}

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddElement_FormClosing(object sender, FormClosingEventArgs e)
        {
            switch (role)
            {
                case "empl":
                    f1.RefreshTable();
                    f1.Show();
                    break;
                case "head":
                    f2.RefreshTable();
                    f2.Show();
                    break;
                case "admin":
                    f3.RefreshTable();
                    f3.Show();
                    break;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (role == "empl" || elem == "disaster")
            {
                if (isUpdate)
                {
                    try
                    {
                        comm = new NpgsqlCommand("SELECT * FROM update_disaster(" + idUp + ", " + dt.Rows[comboBox1.SelectedIndex][0].ToString() +", " +
                      dt2.Rows[comboBox2.SelectedIndex][0].ToString()+  ", (\'" + maskedTextBox1.Text + "\')::date)", conn);
                    
                        int n = comm.ExecuteNonQuery();
                        MessageBox.Show("Запись изменена успешно!");
                        this.Close();
                    }
                    catch (PostgresException err)
                    {
                        if (err.MessageText.Contains("неверный синтаксис для типа date"))
                            MessageBox.Show("Неверно введена дата, повторите попытку!");
                        else
                            MessageBox.Show("Что-то не так");
                    }
                    catch (Exception err)
                    {
                        if (err.Message.Contains("Длина не может быть меньше нуля"))
                            MessageBox.Show("Вы не ввели все необходимые данные, повторите попытку!");
                    }
                }
                else
                {
                    try
                    {
                        string id = comboBox1.Text.Substring(comboBox1.Text.IndexOf('#') + 1, comboBox1.Text.IndexOf(']') - 2);
                    comm = new NpgsqlCommand("SELECT * FROM add_disaster(" + id + ", " + dt.Rows[comboBox2.SelectedIndex][0].ToString() + ", (\'" + maskedTextBox1.Text + "\')::date)", conn);
                    
                        comm.ExecuteNonQuery();
                        MessageBox.Show("Запись успешно добавлена!");
                        this.Close();
                    }
                    catch (PostgresException err)
                    {
                        if (err.MessageText.Contains("неверный синтаксис для типа date"))
                            MessageBox.Show("Неверно введена дата, повторите попытку!");
                        else
                            MessageBox.Show("Что-то не так, пожалуйста, повторите попытку позже.");
                    }
                    catch (Exception err)
                    {
                        if (err.Message.Contains("Длина не может быть меньше нуля"))
                            MessageBox.Show("Вы не ввели все необходимые данные, повторите попытку!");
                    }
                }
            }
            else switch (elem)
                {
                    case "company":
                        if (isUpdate)
                        {
                            try
                            {
                                comm = new NpgsqlCommand("SELECT * FROM update_company("+idUp+", \'" + textBox2.Text + "\', " + dt2.Rows[comboBox6.SelectedIndex][0].ToString() + ", " +
                                dt.Rows[comboBox7.SelectedIndex][0].ToString() + ", " + numericUpDown6.Value.ToString() + ", \'" + comboBox11.Text + " " + maskedTextBox3.Text + "\')", conn);
                           
                                int n = comm.ExecuteNonQuery();
                                MessageBox.Show("Запись изменена успешно!");
                                this.Close();
                            }
                            catch (PostgresException err)
                            {
                                if (err.MessageText.Contains("значение домена phone_number нарушает ограничение-проверку"))
                                    MessageBox.Show("Введенный вами номер телефона неверен, повторите попытку, пожалуйста!");
                                else if (err.MessageText.Contains("Invalid name"))
                                    MessageBox.Show("Введённое вами название предприятия недопустимо, повторите попытку, пожалуйста!");
                                else if (err.MessageText.Contains("Invalid year"))
                                    MessageBox.Show("Введённый вами год является недействительным, повторите попытку, пожалуйста!");
                                else
                                    MessageBox.Show("Что-то не так. Повторите попытку позже.");
                            }
                        }
                        else
                        {
                            try
                            {
                                comm = new NpgsqlCommand("SELECT * FROM add_company(\'" + textBox2.Text + "\', " + dt2.Rows[comboBox6.SelectedIndex][0].ToString() + ", " +
                                dt.Rows[comboBox7.SelectedIndex][0].ToString() + ", " + numericUpDown6.Value.ToString() + ", \'" + comboBox11.Text + " " + maskedTextBox3.Text + "\')", conn);
                            
                                comm.ExecuteNonQuery();
                                MessageBox.Show("Запись успешно добавлена!");
                                this.Close();
                            }
                            catch (PostgresException err)
                            {
                                if (err.MessageText.Contains("значение домена phone_number нарушает ограничение-проверку"))
                                    MessageBox.Show("Введенный вами номер телефона неверен, повторите попытку, пожалуйста!");
                                else if (err.MessageText.Contains("Invalid name"))
                                    MessageBox.Show("Введённое вами название предприятия недопустимо, повторите попытку, пожалуйста!");
                                else if (err.MessageText.Contains("Invalid year"))
                                    MessageBox.Show("Введённый вами год является недействительным, повторите попытку, пожалуйста!");
                                else
                                    MessageBox.Show("Что-то не так. Повторите попытку позже.");
                            }
                            catch (Exception err) {
                                if (err.Message == "В позиции -1 строка отсутствует.")
                                    MessageBox.Show("Вы не ввели все необходимые данные, повторите попытку!");
                            }
                        }
                        break;
                    case "engagement":
                        if (isUpdate)
                        {
                            try
                            {
                                comm = new NpgsqlCommand("SELECT * FROM update_engagement("+idUp+", " + dt.Rows[comboBox10.SelectedIndex][0].ToString() + ", " +
                                dt2.Rows[comboBox9.SelectedIndex][0].ToString() + ", " + numericUpDown7.Value.ToString() + ", " + numericUpDown5.Value.ToString() + ")", conn);

                                int n = comm.ExecuteNonQuery();
                                MessageBox.Show("Запись изменена успешно!");
                                this.Close();
                            }
                            catch (PostgresException err)
                            {
                                if (err.MessageText.Contains("There is no such employees"))
                                    MessageBox.Show("В данном отделе ЭСУ нет столько сотрудников, повторите попытку, пожалуйста!");
                                else
                                    MessageBox.Show("Что-то не так. Повторите попытку позже.");
                            }
                        }
                        else
                        {
                            try
                            {
                                comm = new NpgsqlCommand("SELECT * FROM add_engagement(" + dt.Rows[comboBox10.SelectedIndex][0].ToString() + ", " +
                                dt2.Rows[comboBox9.SelectedIndex][0].ToString() + ", " + numericUpDown7.Value.ToString() + ", " + numericUpDown5.Value.ToString() + ")", conn);

                                comm.ExecuteNonQuery();
                                MessageBox.Show("Запись успешно добавлена!");
                                this.Close();
                            }
                            catch (PostgresException err)
                            {
                                if (err.MessageText.Contains("There is no such employees"))
                                    MessageBox.Show("В данном отделе ЭСУ нет столько сотрудников, повторите попытку, пожалуйста!");
                                else
                                    MessageBox.Show("Что-то не так. Повторите попытку позже.");
                            }
                            catch (Exception err)
                            {
                                if (err.Message == "В позиции -1 строка отсутствует.")
                                    MessageBox.Show("Вы не ввели все необходимые данные, повторите попытку!");
                            }
                        }
                        break;
                    case "consequence":
                        if (isUpdate)
                        {
                            try
                            {
                                comm = new NpgsqlCommand("SELECT * FROM update_consequence("+idUp+", "+ dt.Rows[comboBox5.SelectedIndex][0].ToString() + ", " +
                           dt2.Rows[comboBox3.SelectedIndex][0].ToString() + ", (\'" + numericUpDown4.Value.ToString() + "\')::numeric, " + numericUpDown3.Value.ToString() + ")", conn);

                                int n = comm.ExecuteNonQuery();
                                MessageBox.Show("Запись изменена успешно!");
                                this.Close();
                            }
                            catch (PostgresException err)
                            {
                                if (err.MessageText == "Your department did not engage solving this disaster")
                                    MessageBox.Show("Ваш отдел не принимал участие в устранении этой катастрофы. Вы не можете поменять катастрофу на ту, в которой ваш отдел не принимал участия!");
                                else
                                    MessageBox.Show("Что-то не так");
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
                                comm = new NpgsqlCommand("SELECT * FROM add_consequence(" + dt.Rows[comboBox5.SelectedIndex][0].ToString() + ", " +
                           dt2.Rows[comboBox3.SelectedIndex][0].ToString() + ", (\'" + numericUpDown4.Value.ToString() + "\')::numeric, " + numericUpDown3.Value.ToString() + ")", conn);

                                comm.ExecuteNonQuery();
                                MessageBox.Show("Запись успешно добавлена!");
                                this.Close();
                            }
                            catch (PostgresException err)
                            {
                                if (err.MessageText == "Your department did not engage solving this disaster")
                                    MessageBox.Show("Ваш отдел не принимал участие в устранении этой катастрофы. Вы не можете добавлять последствия к катастрофам, в которых не принимал участия ваш отдел!");
                                else
                                    MessageBox.Show("Что-то не так");
                            }
                            catch (Exception err)
                            {
                                if (err.Message == "В позиции -1 строка отсутствует.")
                                    MessageBox.Show("Вы не ввели все необходимые данные, повторите попытку!");
                            }
                        }
                        break;
                    case "department":
                        if (isUpdate)
                        {
                            try
                            {
                                comm = new NpgsqlCommand("SELECT * FROM update_department("+ idUp +", \'" + textBox1.Text + "\', " + dt.Rows[comboBox4.SelectedIndex][0].ToString() + ", \'" +
                                comboBox8.Text + " " + maskedTextBox2.Text + "\', " + numericUpDown1.Value.ToString() + ", " + numericUpDown2.Value.ToString() + ")", conn);

                                comm.ExecuteNonQuery();
                                MessageBox.Show("Запись изменена успешно!");
                                this.Close();
                            }
                            catch (PostgresException err)
                            {
                                if (err.MessageText.Contains("значение домена phone_number нарушает ограничение-проверку"))
                                    MessageBox.Show("Введенный вами номер телефона неверен, повторите попытку, пожалуйста!");
                                else if (err.MessageText.Contains("Invalid name"))
                                    MessageBox.Show("Введённое вами название отдела ЭСУ недопустимо, повторите попытку, пожалуйста!");
                                else if (err.MessageText.Contains("Invalid year"))
                                    MessageBox.Show("Введённый вами год является недействительным, повторите попытку, пожалуйста!");
                                else if (err.MessageText.Contains("Invalid employees"))
                                    MessageBox.Show("Число работников не может быть меньше числа пользователь из данного отдела, повторите попытку!");
                                else
                                    MessageBox.Show("Что-то не так. Повторите попытку позже.");
                            }

                        }
                        else
                        {
                            try
                            {
                                comm = new NpgsqlCommand("SELECT * FROM add_department(\'" + textBox1.Text + "\', " + dt.Rows[comboBox4.SelectedIndex][0].ToString() + ", \'" +
                                comboBox8.Text + " " + maskedTextBox2.Text + "\', " + numericUpDown1.Value.ToString() + ", " + numericUpDown2.Value.ToString() + ")", conn);

                                comm.ExecuteNonQuery();
                                MessageBox.Show("Запись успешно добавлена!");
                                this.Close();
                            }
                            catch (PostgresException err)
                            {
                                if (err.MessageText.Contains("значение домена phone_number нарушает ограничение-проверку"))
                                    MessageBox.Show("Введенный вами номер телефона неверен, повторите попытку, пожалуйста!");
                                else if (err.MessageText.Contains("Invalid name"))
                                    MessageBox.Show("Введённое вами название предприятия недопустимо, повторите попытку, пожалуйста!");
                                else if (err.MessageText.Contains("Invalid year"))
                                    MessageBox.Show("Введённый вами год является недействительным, повторите попытку, пожалуйста!");
                                else
                                    MessageBox.Show("Что-то не так. Повторите попытку позже.");
                            }
                            catch (Exception err)
                            {
                                if (err.Message == "В позиции -1 строка отсутствует.")
                                    MessageBox.Show("Вы не ввели все необходимые данные, повторите попытку!");
                            }
                        }
                        break;
                }
        }
    }
}
