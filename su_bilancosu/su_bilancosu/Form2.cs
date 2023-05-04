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

namespace su_bilancosu
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public double[] yagis = new double[12];
        public double[] dpe= new double[12];
        public double[] gercek = new double[12];

        Form1 form1 = new Form1();


        public void Form2_Load(object sender, EventArgs e)
        {
            /*var t = chart1.ChartAreas[0];
            t.AxisX.IntervalType = DateTimeIntervalType.Number;*/


            //Form1'den aldığımız yağış,düzeltilmiş Pe,Gerçek Evapotranspirasyon değerlerini Form2'ye aktarıyoruz.
            //Alınan değerleri chart üzerinde lines kullanarak çizgi belirliyoruz.
            var chart = chart1.ChartAreas[0];
            chart.AxisX.IntervalType = DateTimeIntervalType.Number;

            chart.AxisX.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.Format = "";
            chart.AxisY2.LabelStyle.Format = "";
            chart.AxisY.LabelStyle.IsEndLabelVisible = true;

            chart.AxisX.Minimum = 1;
            chart.AxisX.Maximum = 13;

            //chart.AxisY.Minimum = 0;
            //chart.AxisY.Maximum = 300;

            //chart.AxisY2.Minimum = 0;
            //chart.AxisY2.Maximum= 20;

            chart.AxisX.Interval = 1;
            chart.AxisY.Interval = 50;
            chart.AxisY2.Interval = 50;

           
            this.chart1.ChartAreas[0].AxisY.Title = "YAĞIŞ-GERÇEK EVAPOTRANSPİRASYON(MM)";
            this.chart1.ChartAreas[0].AxisX.Title = "AYLAR";
            this.chart1.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;

            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
           


            
                var lines = new Series("Yağış");
                lines.ChartType = SeriesChartType.Line;
                lines.Color = Color.Blue;

                lines.Points.Add(new DataPoint(1, yagis[0]));
                lines.Points.Add(new DataPoint(2, yagis[1]));
                lines.Points.Add(new DataPoint(3, yagis[2]));
                lines.Points.Add(new DataPoint(4, yagis[3]));
                lines.Points.Add(new DataPoint(5, yagis[4]));
                lines.Points.Add(new DataPoint(6, yagis[5]));
                lines.Points.Add(new DataPoint(7, yagis[6]));
                lines.Points.Add(new DataPoint(8, yagis[7]));
                lines.Points.Add(new DataPoint(9, yagis[8]));
                lines.Points.Add(new DataPoint(10, yagis[9]));
                lines.Points.Add(new DataPoint(11, yagis[10]));
                lines.Points.Add(new DataPoint(12, yagis[11]));
                lines.Points.Add(new DataPoint(13, yagis[0]));
                lines.YAxisType = AxisType.Primary;
                lines.MarkerStyle = MarkerStyle.Circle;
                chart1.Series.Add(lines);


                var lines2 = new Series("Düzeltilmiş PE");
                lines2.ChartType = SeriesChartType.Line;
                lines2.Color = Color.Red;
                lines2.Points.Add(new DataPoint(1, dpe[0]));
                lines2.Points.Add(new DataPoint(2, dpe[1]));
                lines2.Points.Add(new DataPoint(3, dpe[2]));
                lines2.Points.Add(new DataPoint(4, dpe[3]));
                lines2.Points.Add(new DataPoint(5, dpe[4]));
                lines2.Points.Add(new DataPoint(6, dpe[5]));
                lines2.Points.Add(new DataPoint(7, dpe[6]));
                lines2.Points.Add(new DataPoint(8, dpe[7]));
                lines2.Points.Add(new DataPoint(9, dpe[8]));
                lines2.Points.Add(new DataPoint(10, dpe[9]));
                lines2.Points.Add(new DataPoint(11, dpe[10]));
                lines2.Points.Add(new DataPoint(12, dpe[11])); 
                lines2.Points.Add(new DataPoint(13, dpe[0]));
                lines2.YAxisType = AxisType.Primary;
                lines2.MarkerStyle = MarkerStyle.Circle;
                chart1.Series.Add(lines2);


                var lines3 = new Series("Gerçek Evapo.");
                lines3.ChartType = SeriesChartType.Line;
                lines3.Color = Color.Gray;
                lines3.Points.Add(new DataPoint(1, gercek[0]));
                lines3.Points.Add(new DataPoint(2, gercek[1]));
                lines3.Points.Add(new DataPoint(3, gercek[2]));
                lines3.Points.Add(new DataPoint(4, gercek[3]));
                lines3.Points.Add(new DataPoint(5, gercek[4]));
                lines3.Points.Add(new DataPoint(6, gercek[5]));
                lines3.Points.Add(new DataPoint(7, gercek[6]));
                lines3.Points.Add(new DataPoint(8, gercek[7]));
                lines3.Points.Add(new DataPoint(9, gercek[8]));
                lines3.Points.Add(new DataPoint(10, gercek[9]));
                lines3.Points.Add(new DataPoint(11, gercek[10]));
                lines3.Points.Add(new DataPoint(12, gercek[11]));
                lines3.Points.Add(new DataPoint(13, gercek[0]));
                lines3.YAxisType = AxisType.Primary;
                lines3.MarkerStyle = MarkerStyle.Circle;
                chart1.Series.Add(lines3);
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
 
       }
    }
