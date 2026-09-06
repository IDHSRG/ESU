using System;
using Npgsql;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace ESU
{
    public partial class Tasks : Form
    {
        List<string> task = new List<string>()
        { "task_engagements_of_disaster",
          "task_workers_of_department",
          "task_disasters_in_period",
          "task_companies_before_year",
          "task_disasters_and_consequences_view",
          "task_departments_and_engagements_view",
          "task_companies_and_disasters_view",
          "task_departments_lazy_view",
          "task_companies_harmless_view",
          "task_engageless_disasters_view",
          "task_disasters_total_view",
          "task_disasters_by_years_view",
          "task_engagements_of_disaster_in_period",
          "task_workers_by_mask",
          "task_employees_of_cities_view",
          "task_companies_count_disasters_view",
          "task_disasters_with_n_damage",
          "task_disasters_with_n_damage_in_period",
          "task_disasters_total_casualtys_view",
          "task_companies_and_departments_in_cities_view",
          "task_departments_with_n_employees",
          "task_departments_less_n_year",
          "task_type_disasters_in_period",
          "task_city_disasters_top_view"
        };
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        DataTable dt;
        DataTable dt0;
        int n; string name;
        byte type;
        public Tasks(NpgsqlConnection c, int NumTask, string NameTask)
        {
            InitializeComponent();
            conn = c;
            n = NumTask;
            name = NameTask;
            listPanel.Visible = false;
            textPanel.Visible = false;
            dataPanel.Visible = false;
            switch (NumTask)
            {
                case 1:
                    listPanel.Visible = true;
                    type = 1;
                    break;
                case 2:
                    listPanel.Visible = true;
                    type = 1;
                    break;
                case 3:
                    dataPanel.Visible = true;
                    type = 2;
                    break;
                case 4:
                    textPanel.Visible = true;
                    type = 3;
                    break;
                case 13:
                    dataPanel.Visible = true;
                    type = 2;
                    break;
                case 14:
                    textPanel.Visible = true;
                    type = 3;
                    break;
                case 17:
                    textPanel.Visible = true;
                    type = 3;
                    break;
                case 18:
                    dataPanel.Visible = true;
                    label7.Visible = true;
                    textBox1.Visible = true;
                    type = 2;
                    break;
                case 21:
                    textPanel.Visible = true;
                    type = 3;
                    break;
                case 22:
                    textPanel.Visible = true;
                    type = 3;
                    break;
                case 23:
                    dataPanel.Visible = true;
                    type = 2;
                    break;
                default:
                    button4.Visible = false;
                    break;
            }
            dt = new DataTable();

            if (task[NumTask - 1].Contains("_view"))
            {
                panel1.Location = new Point(1, 4);
                this.Size = new Size(1161, 547);
                comm = new NpgsqlCommand("SELECT * FROM " + task[NumTask - 1], conn);
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label2.Text += NameTask;
                label5.Text += dataGridView1.Rows.Count;
            }
            else
            {
                panel1.Location = new Point(1, 81);
                this.Size = new Size(1161, 625);
                if (type == 1)
                    switch (NumTask)
                    {
                        case 1:
                            dt0 = new DataTable();
                            comm = new NpgsqlCommand("SELECT * FROM disasters_all_view", conn);
                            dt0.Load(comm.ExecuteReader());
                            foreach (DataRow row in dt0.Rows)
                                comboBox2.Items.Add("[#" + row[0].ToString() + "] " + row[3].ToString() + " - " + row[1].ToString() + "(" + row[2].ToString() + ")");
                            break;
                        case 2:
                            dt0 = new DataTable();
                            comm = new NpgsqlCommand("SELECT * FROM departments_all_view", conn);
                            dt0.Load(comm.ExecuteReader());
                            label4.Text = "Отдел:";
                            foreach (DataRow row in dt0.Rows)
                                comboBox2.Items.Add("[#" + row[0].ToString() + "] " + row[1].ToString() + " - " + row[2].ToString());
                            break;
                    }
                else if (type == 2)
                {
                    panel1.Location = new Point(1, 200);
                    this.Size = new Size(1161, 738);
                    if (NumTask == 18)
                    {
                        textBox1.Visible = true;
                        label7.Visible = true;
                        label7.Text = "Суммарный материальный ущерб";
                    }
                }
                else if (type == 3)
                    switch (NumTask)
                    {
                        case 4:
                            numericUpDown1.Visible = true;
                            numericUpDown1.Maximum = 9999;
                            numericUpDown1.Minimum = 1800;
                            label6.Text = "До года:";
                            break;
                        case 21:
                            numericUpDown1.Visible = true;
                            numericUpDown1.Maximum = 99999999;
                            numericUpDown1.Minimum = 0;
                            label6.Text = "Сотрудников:";
                            break;
                        case 22:
                            numericUpDown1.Visible = true;
                            numericUpDown1.Maximum = 9999;
                            numericUpDown1.Minimum = 1800;
                            label6.Text = "До года:";
                            break;
                        case 14:
                            label6.Text = "Начало:";
                            break;
                        case 17:
                            label6.Text = "Мат. ущерб:";
                            break;
                    };
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (type == 1)
                if (comboBox2.SelectedIndex == -1)
                    MessageBox.Show("Вы не выбрали элемент из списка, повторите попытку, пожалуйста!");
                else
                {
                    dt = new DataTable();
                    switch (n)
                    {
                        case 1:
                            comm = new NpgsqlCommand("SELECT * FROM task_engagements_of_disaster(" + dt0.Rows[comboBox2.SelectedIndex][0].ToString() + ")", conn);
                            break;
                        case 2:

                            comm = new NpgsqlCommand("SELECT * FROM task_workers_of_department(" + dt0.Rows[comboBox2.SelectedIndex][0].ToString() + ")", conn);
                            break;
                    };
                    dt.Load(comm.ExecuteReader());
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Refresh();
                    label2.Text = "Таблица: " + name;
                    label5.Text = "Записей: " + dataGridView1.Rows.Count;
                }
            else if (type == 2)
            {
                dt = new DataTable();
                String l = monthCalendar1.SelectionRange.Start.ToString("dd.MM.yyyy");
                String r = monthCalendar2.SelectionRange.Start.ToString("dd.MM.yyyy");
                switch (n)
                {
                    
                    case 3:
                        comm = new NpgsqlCommand("SELECT * FROM task_disasters_in_period(\'" + l + "\', \'"+r+"\')", conn);
                        break;
                    case 13:
                        comm = new NpgsqlCommand("SELECT * FROM task_engagements_of_disaster_in_period(\'" + l + "\', \'" + r + "\')", conn);
                        break;
                    case 18:
                        double res;
                        if (Double.TryParse(textBox1.Text.Replace('.',','), out res))
                            comm = new NpgsqlCommand("SELECT * FROM task_disasters_with_n_damage_in_period("+textBox1.Text+", \'" + l + "\', \'" + r + "\')", conn);
                        else
                        {
                            MessageBox.Show("Вы ввели неверное числовое значение, повторите попытку, пожалуйста!");
                            return;
                        }
                        
                        break;
                    case 23:
                        comm = new NpgsqlCommand("SELECT * FROM task_type_disasters_in_period(\'" + l + "\', \'" + r + "\')", conn);
                        break;
                };
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label2.Text = "Таблица: " + name;
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
            }

            else if (type == 3)
            {
                dt = new DataTable();
                switch (n)
                {
                    case 4:
                        comm = new NpgsqlCommand("SELECT * FROM task_companies_before_year(" + numericUpDown1.Value.ToString() + ")", conn);
                        break;
                    case 21:
                        comm = new NpgsqlCommand("SELECT * FROM task_departments_with_n_employees(" + numericUpDown1.Value.ToString() + ")", conn);
                        break;
                    case 22:
                        comm = new NpgsqlCommand("SELECT * FROM task_departments_less_n_year(" + numericUpDown1.Value.ToString() + ")", conn);
                        break;
                    case 14:
                        comm = new NpgsqlCommand("SELECT * FROM task_workers_by_mask(\'" + textBox2.Text + "\')", conn);
                        break;
                    case 17:
                        double res;
                        if (Double.TryParse(textBox2.Text, out res))
                            comm = new NpgsqlCommand("SELECT * FROM task_disasters_with_n_damage(" + textBox2.Text + ")", conn);
                        else
                        { 
                            MessageBox.Show("Вы ввели неверное значение, повторите попытку, пожалуйста!"); 
                            return; 
                        }
                        break;
                };
                dt.Load(comm.ExecuteReader());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Refresh();
                label2.Text = "Таблица: " + name;
                label5.Text = "Записей: " + dataGridView1.Rows.Count;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                SaveFileDialog sfd = new SaveFileDialog(); sfd.Filter = "Excel документ (*.xlsx)|*.xlsx"; sfd.FileName = "Экспорт.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (SpreadsheetDocument document = SpreadsheetDocument.Create(sfd.FileName,
                                                         SpreadsheetDocumentType.Workbook))
                    {
                        WorkbookPart workbookPart = document.AddWorkbookPart(); workbookPart.Workbook = new Workbook();
                        WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = new SheetData(); worksheetPart.Worksheet = new Worksheet(sheetData);

                        Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                        Sheet sheet = new Sheet()
                        {Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Sheet1"};

                        sheets.Append(sheet);
                        Row headerRow = new Row();
                        List<String> columns = new List<string>();
                        foreach (DataGridViewColumn col in dataGridView1.Columns)
                        {
                            columns.Add(col.HeaderText);
                            Cell cell = new Cell();
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue(col.HeaderText);
                            headerRow.AppendChild(cell);
                        }
                        sheetData.AppendChild(headerRow);
                        int i;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            Row newRow = new Row();
                            i = 0;
                            foreach (String col in columns)
                            {
                                Cell cell = new Cell(); cell.DataType = CellValues.String;
                                cell.CellValue = new CellValue(Convert.ToString(row.Cells[i].Value));
                                newRow.AppendChild(cell); i++;
                            }
                            sheetData.AppendChild(newRow);
                        }
                        workbookPart.Workbook.Save();
                    }
                }
            }

        }
    }
}
