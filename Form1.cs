using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Task1HotCup
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            double deltaInMinutes =0.01;
            double tempOut = 22;
            var finalTempetures = new double[] { 83, 77.7, 75.1, 73, 71.1, 69.4, 67.8, 66.4, 64.7, 63.4, 62.1, 61.0, 59.9, 58.7, 57.8, 56.6 };
            double gamma = Program.FindGamma(tempOut, finalTempetures, deltaInMinutes);
            var theoreticalArray = Program.GetTheoreticalArray(tempOut, finalTempetures[0], deltaInMinutes, gamma, finalTempetures.Length);

            DrawChart1(finalTempetures, theoreticalArray);
            DrawTable1(tempOut, finalTempetures[0], gamma, deltaInMinutes);
            DrawTable2(tempOut, gamma, deltaInMinutes);
            DrawKTextBox(gamma);
            DrawChart3Table4(tempOut, finalTempetures[0], gamma);
            double delta1 = Program.FindDelta(tempOut, finalTempetures[0], 1, gamma, 80.55, 0, 1);
            double delta5 = Program.FindDelta(tempOut, finalTempetures[0], 5, gamma, 71.69, 0, 1);
            double Temp1 = Math.Round(Program.FindTemp(tempOut, finalTempetures[0], 1, delta1, gamma, 0), 4);
            double Temp5 = Math.Round(Program.FindTemp(tempOut, finalTempetures[0], 5, delta5, gamma, 0),4);
            textBox5.Text = "Задание 1.7 1 мин delta = " + delta1.ToString()+" | T(1) = "+Temp1.ToString();
            textBox6.Text = "Задание 1.7 5 мин delta = " + delta5.ToString()+"| T(5) = "+Temp5.ToString();
        }

        private void DrawChart1 (double[] FinalTempetures, double[] theoreticalArray)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            for (int i = 0; i < FinalTempetures.Length; i++)
            {
                chart1.Series[0].Points.AddXY(i, theoreticalArray[i]);
                chart1.Series[1].Points.AddXY(i, FinalTempetures[i]);
            }
            chart1.Series[1].Color = Color.Red;
            chart1.Series[0].Color = Color.Blue;
            chart1.Series[0].BorderWidth = 2;
            chart1.Series[1].BorderWidth = 2;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 16;
            chart1.ChartAreas[0].AxisY.Minimum = 50;
            chart1.ChartAreas[0].AxisY.Maximum = 85;
            chart1.ChartAreas[0].AxisY.Interval = 5;
            chart1.ChartAreas[0].AxisY.Title = "T, ℃";
            chart1.ChartAreas[0].AxisX.Title = "t, мин";
            chart1.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 10.0f);
            chart1.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 10.0f);
            chart1.Series[0].LegendText = "Theoretical Data";
            chart1.Series[1].LegendText = "Expirement Data";
 
        }
    
        private void DrawChart3Table4(double tempOut, double tempIn, double gamma)
        {
            chart3.ChartAreas[0].AxisY.Title = "|Tₑₓₚ - Tₜₕₑₒᵣ|, ℃";
            chart3.ChartAreas[0].AxisX.Title = "Δt, мин";
            chart3.ChartAreas[0].AxisY.TitleFont = new Font("Microsoft Sans Serif", 11.0f);
            chart3.ChartAreas[0].AxisX.TitleFont = new Font("Microsoft Sans Serif", 11.0f);
            DataTable table4 = new DataTable();
            table4.Columns.Add("delta");
            table4.Columns.Add("Difference");
            chart3.Series[0].Points.Clear();
            for (double delta = 0.1; delta >= 0.025; delta /= 2)
            {
               var x1 = Math.Abs(80.55 - Program.FindTemp(tempOut, tempIn, 1, delta, gamma, 0));
                table4.Rows.Add(delta, Math.Round(x1,4));
                chart3.Series[0].Points.AddXY(delta,x1);
            }
            for (double delta = 0.01; delta >= 0.005; delta /= 2)
            {
               var x2 = Math.Abs(80.55 - Program.FindTemp(tempOut, tempIn, 1, delta, gamma, 0));
                chart3.Series[0].Points.AddXY(delta, x2);
                table4.Rows.Add(delta, Math.Round(x2,4));
            }
            dataGridView4.DataSource = table4;
        }

        private void DrawTable1 (double tempOut, double tempIn, double gamma, double deltaInMinutes )
        {
            DataTable table1 = new DataTable();
            table1.Columns.Add("Tempeture Difference");
            table1.Columns.Add("Time In minutes");
            table1.Rows.Add(61.0 / 2, Math.Round(Program.FindTime(tempOut, tempIn, gamma, deltaInMinutes, tempOut + 61.0 / 2), 2));
            table1.Rows.Add(61.0 / 4, Math.Round(Program.FindTime(tempOut, tempIn, gamma, deltaInMinutes, tempOut + 61.0 / 4), 2));
            table1.Rows.Add(61.0 / 8, Math.Round(Program.FindTime(tempOut, tempIn, gamma, deltaInMinutes, tempOut + 61.0 / 8), 2));
            dataGridView1.DataSource = table1;
            dataGridView1.Columns[0].Width = 170;
            dataGridView1.Columns[1].Width = 170;
            dataGridView1.Rows[0].Height = 50;
            dataGridView1.Rows[1].Height = 50;
            dataGridView1.Rows[2].Height = 50;
            textBox1.BorderStyle = BorderStyle.None;
        }

        private void DrawTable2(double tempOut, double gamma, double deltaInMinutes)
        {
            DataTable table2 = new DataTable();

            table2.Columns.Add("Time with Milk at the beggining");
            table2.Columns.Add("Time with Milk at the end");
            table2.Rows.Add(Math.Round(Program.FindTime(tempOut, 85, gamma, deltaInMinutes, 75), 2), Math.Round(Program.FindTime(tempOut, 90, gamma, deltaInMinutes, 80), 2));
            dataGridView2.DataSource = table2;
            dataGridView2.Columns[0].Width = 170;
            dataGridView2.Columns[1].Width = 170;
            dataGridView2.Rows[0].Height = 70;
            textBox2.BorderStyle = BorderStyle.None;
        }
        private void DrawKTextBox (double gamma)
        {
            textBox3.Text = "gamma = " + Math.Round(gamma, 4).ToString();
            textBox3.Font = new Font("Microsoft Sans Serif", 10.0f);
            textBox3.BorderStyle = BorderStyle.None;
        }
        

        private void chart1_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

