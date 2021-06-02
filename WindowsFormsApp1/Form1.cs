using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        WeatherModel curWeather;
        int[] duration; int Point = 0;
        public Form1()
        {
            InitializeComponent();
            curWeather = new WeatherModel();
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 10;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            if (timer1.Enabled)
            {
                double SumDuration = duration.Sum();
                double[] RealProb = new double[3];
                for (int i = 0; i < 3; i++)
                {
                    RealProb[i] = duration[i] / SumDuration;
                }
                chart1.Series[1].Points.AddXY(0, RealProb[0]);
                chart1.Series[1].Points.AddXY(1, RealProb[1]);
                chart1.Series[1].Points.AddXY(2, RealProb[2]);
                
                (bool, double) Result = curWeather.GetChiSquare(RealProb, Point);
                label1.Visible = true;
                label1.Text = $"ChiSquare = {Math.Round(Result.Item2,4)}";
                if (Result.Item1)
                {
                    label2.ForeColor = Color.Red;
                    label1.Text += " < 5.991";
                    label2.Text = $" is {false}";
                }
                else
                {
                    label2.ForeColor = Color.Green;
                    label1.Text += "> 5.991";
                    label2.Text = $" is {true}";
                }
                timer1.Stop();
                button1.Text = "Restart";
            }
            else
            {
                chart1.Series[0].Points.Clear();
                chart1.Series[1].Points.Clear();
                label2.Text = "";
                label1.Text = "";
                chart1.ChartAreas[0].AxisX.ScrollBar.Axis.ScaleView.Position = 0;
                Point = 0;
                curWeather.GetNextStage();
                chart1.Series[0].Points.AddXY(Point, (int)curWeather.GetCurrentStage() + 1);
                duration = new int[3];
                duration[(int)curWeather.GetCurrentStage()] += curWeather.GetTime();
                Point++;
                timer1.Start();
                button1.Text = "Stop";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (curWeather.GetTime() == 0)
            {
                curWeather.GetNextStage();
                chart1.Series[0].Points.AddXY(Point, (int)curWeather.GetCurrentStage() + 1);
                duration[(int)curWeather.GetCurrentStage()] += curWeather.GetTime();
                Point++;
            }
            else
            {
                chart1.Series[0].Points.AddXY(Point, (int)curWeather.GetCurrentStage() + 1);
                Point++;
                curWeather.DecreaseTime();
            }
            label3.Text = "Weather is: " + curWeather.GetCurrentStage();
            if (Point > 10)
            {
                chart1.ChartAreas[0].AxisX.ScrollBar.Axis.ScaleView.Position = Point - 10;
            }
        }
    }
}
