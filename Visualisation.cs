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
using System.Windows.Forms.DataVisualization.Charting;

namespace ESU
{
    public partial class Visualisation : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand comm;
        DataTable dt;
        byte type;
        public Visualisation(NpgsqlConnection c, byte t)
        {
            InitializeComponent();
            conn = c;
            type = t;
        }

        private void Visualisation_Load(object sender, System.EventArgs e)
        {
            switch (type)
            {
                case 1:
                    comm = new NpgsqlCommand("SELECT * FROM task_city_disasters_top_view", conn);
                    dt = new DataTable();
                    dt.Load(comm.ExecuteReader());
                    chart1.Series.Add(new Series("ColumnSeries") { ChartType = SeriesChartType.Pie });
                    List<string> names = new List<string>();
                    List<double> values = new List<double>();
                    foreach (DataRow row in dt.Rows)
                    {
                        names.Add("" + row[0]);
                        values.Add(Convert.ToDouble(row[2]));
                    }
                    chart1.Series["ColumnSeries"].Points.DataBindXY(names.ToArray(), values.ToArray());
                    chart1.Visible = true;
                    break;
                case 2:
                    DataTable dt0 = new DataTable();
                    comm = new NpgsqlCommand("SELECT \"Тип катастрофы\" FROM type_disaster_view ORDER BY \"Тип катастрофы\"", conn);
                    dt0.Load(comm.ExecuteReader());
                    string gen = "";
                    chart1.Series.Clear();

                    int i = 0;
                    foreach (DataRow row in dt0.Rows)
                    {

                        chart1.Series.Add(row[0] + "");
                        chart1.Series[i].ChartType = SeriesChartType.StackedBar;
                        gen += i.ToString() + row[0];
                        i++;
                    }
                    for (int j = 1990; j < DateTime.UtcNow.Date.Year; j++)
                    {
                        dt = new DataTable();
                        comm = new NpgsqlCommand("SELECT * FROM task_type_disasters_in_period(\'01.01."+j.ToString()+"\', \'31.12."+j.ToString()+"\')", conn);
                        dt.Load(comm.ExecuteReader());
                        foreach (DataRow row in dt.Rows)
                        {
                            chart1.Series[row[1] + ""].Points.AddXY(j, Convert.ToInt32("" + row[2]));
                        }
                    }
                    chart1.Visible = true;
                    break;
               case 3:
                    comm = new NpgsqlCommand("SELECT * FROM task_departments_active_view", conn);
                    dt = new DataTable();
                    dt.Load(comm.ExecuteReader());
                    List<string> name = new List<string>();
                    List<int> val1 = new List<int>();
                    List<int> val2 = new List<int>();
                    foreach (DataRow row in dt.Rows)
                    {
                        name.Add("[#"+row[0]+"] " + row[1]);
                        val1.Add(Convert.ToInt32(row[3]));
                        val2.Add(Convert.ToInt32(row[2]));
                    }
                    chart1.Series.Add(new Series("Вне города") { ChartType = SeriesChartType.Column });
                    chart1.Series.Add(new Series("Внутри города") { ChartType = SeriesChartType.Column });
                    
                    chart1.Series["Вне города"].Points.DataBindXY(name.ToArray(), val2.ToArray());
                    chart1.Series["Внутри города"].Points.DataBindXY(name.ToArray(), val1.ToArray());
                    chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
                    LabelStyle style = new LabelStyle();
                    style.Interval = 1;
                    chart1.ChartAreas[0].AxisX.LabelStyle = style;
                    chart1.Visible = true;
                    break;

            }
        }
    }
}
