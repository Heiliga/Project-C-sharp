using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;


namespace Лаба1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        List<Node> Nodes = new List<Node>();
        int n, m;
        int InfactionRate;
        int RecoveryTime;
        int NS;
        int NI = 0;
        int NR = 0;
        int NX = 0;




        public void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void label1_Click(object sender, EventArgs e)
        {

        }

        //Рисовка графа.
        void Drawe()
        {
            var e = CreateGraphics();
            e.Clear(Color.White);

            Pen greenPen = new Pen(Color.Green, 3);
            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                if (Nodes[i].i == Nodes[i + 1].i)
                    e.DrawLine(greenPen, Nodes[i].X, Nodes[i].Y, Nodes[i + 1].X, Nodes[i + 1].Y);
            }
            for (int i = 0; i < Nodes.Count - m; i++)
                e.DrawLine(greenPen, Nodes[i].X, Nodes[i].Y, Nodes[i + m].X, Nodes[i + m].Y);
            greenPen.Dispose();

            SolidBrush mySolidBrush = new SolidBrush(Color.Yellow);
            for (var i = 0; i < Nodes.Count; i++)
                e.FillEllipse(mySolidBrush, (int)Nodes[i].X - 50 / m, (int)Nodes[i].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
            mySolidBrush.Dispose();

            if (checkBox1.Checked)
                for (int i = 0; i < Nodes.Count; i++)
                    e.DrawString("(" + Nodes[i].i + "|" + Nodes[i].j + ")", new Font("Arial", 10), Brushes.Black, (int)Nodes[i].X - 15, (int)Nodes[i].Y - 25);






        }

        //Расчет графа
        public void button2_Click(object sender, EventArgs a)
        {
            var e = CreateGraphics();
            int X, Y, Width, Height, Length;
            Width = 600;
            Height = 600;
            m = Convert.ToInt32(textBox1.Text);
            n = Convert.ToInt32(textBox2.Text);
            if (Height / (m + 1) > Width / (n + 1))
                Length = Width / (n + 1);
            else
                Length = Height / (m + 1);
            int[] DotsX = new int[m];
            int[] DotsY = new int[n];
            for (var i = 0; i < m; i++)
                DotsX[i] = Length / 2 + Length * i;
            for (var i = 0; i < n; i++)
                DotsY[i] = Length / 2 + Length * i;

            Nodes.Clear();

            for (var i = 0; i < n; i++)
                for (var j = 0; j < m; j++)
                    Nodes.Add(new Node(i, j, DotsX[j], DotsY[i]));



            for (int i = 0; i < Nodes.Count - 1; i++)
            {
                if (Nodes[i].i == Nodes[i + 1].i)
                {
                    Nodes[i].Near.Add(Nodes[i + 1]);
                    Nodes[i + 1].Near.Add(Nodes[i]);
                }

            }
            for (int i = 0; i < Nodes.Count - m; i++)
            {
                Nodes[i].Near.Add(Nodes[i + m]);
                Nodes[i + m].Near.Add(Nodes[i]);

            }
            NS = Nodes.Count;
            Drawe();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //Алгоритм заражение(изменение цвета)
        private void button3_Click(object sender, EventArgs e)
        {
            InfactionRate = Convert.ToInt32(textBox5.Text);
            RecoveryTime = Convert.ToInt32(textBox6.Text);
            Random rnd = new Random();
            int Time = 0;
            for (int j = 0; j < Nodes.Count; j++)
            {
                if (Nodes[j].I == true)
                {
                    Nodes[j].RecoveryTime = RecoveryTime;
                    Nodes[j].CountR++;
                }
                textBox8.Text = NS.ToString();
                textBox9.Text = NI.ToString();
                textBox10.Text = NR.ToString();
                if (Nodes[j].I == true)
                    listBox1.Items.Add("  (" + Nodes[j].i + ", " + Nodes[j].j + ")" + "                           Заражен(I)                                    " + Nodes[j].CountR + "                                   " + Nodes[j].CountTime);
                else if (Nodes[j].S == true)
                    listBox1.Items.Add("  (" + Nodes[j].i + ", " + Nodes[j].j + ")" + "                           Восприимчив(S)                           " + Nodes[j].CountR + "                                   " + Nodes[j].CountTime);
                else if (Nodes[j].Rem == true)
                    listBox1.Items.Add("  (" + Nodes[j].i + ", " + Nodes[j].j + ")" + "                           Удален(Rem)                                " + Nodes[j].CountR + "                                   " + Nodes[j].CountTime);
                else if (Nodes[j].R == true)
                    listBox1.Items.Add("  (" + Nodes[j].i + ", " + Nodes[j].j + ")" + "                           Восстановлен(R)                         " + Nodes[j].CountR + "                                   " + Nodes[j].CountTime);
            }
            timer1.Interval = Convert.ToInt32(textBox7.Text);
            timer1.Start();
            timer1.Tick += new EventHandler((o, ev) =>
            {
                listBox1.Items.Clear();
                Time++;
                for (int j = 0; j < Nodes.Count; j++)
                    {
                        if (Nodes[j].I == true)
                        {
                            for (int k = 0; k < Nodes[j].Near.Count; k++)
                            {
                                if (Nodes[j].Near[k].S == true && Nodes[j].Near[k].Iflag != 1 && Nodes[j].Near[k].Rem == false)
                                {
                                if (rnd.Next(0, 100) < InfactionRate)
                                {
                                    Nodes[j].Near[k].Iflag = 1;
                                    NI++;
                                    NS--;
                                    Nodes[j].Near[k].CountR++;
                                }
                                }
                            }
                        
                        }
                        if (Nodes[j].RecoveryTime > 0)
                        {
                            Nodes[j].RecoveryTime -= 1;

                        }
                        else
                        {
                            if (Nodes[j].RecoveryTime == 0)
                            {
                            Nodes[j].CountTime = Time;
                            Nodes[j].RecoveryTime = -1;
                                Nodes[j].Rflag = 1;
                                NI--;
                            if (radioButton2.Checked)
                                NR++;

                            else if (radioButton1.Checked)
                                NS++;
                            }
                        }
                    NX++;
                    }
                    SolidBrush mySolidBrush1 = new SolidBrush(Color.Red);
                    SolidBrush mySolidBrush2 = new SolidBrush(Color.Green);
                    SolidBrush mySolidBrush3 = new SolidBrush(Color.Yellow);
                var a = CreateGraphics();
                for (int j = 0; j < Nodes.Count; j++)
                {
                    //SIR
                    if (radioButton2.Checked)
                    {
                        if (Nodes[j].Iflag == 1)
                        {
                            Nodes[j].S = false;
                            Nodes[j].I = true;
                            Nodes[j].Iflag = 0;
                            Nodes[j].RecoveryTime = RecoveryTime;
                            a.FillEllipse(mySolidBrush1, (int)Nodes[j].X - 50 / m, (int)Nodes[j].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                        }

                        if (Nodes[j].Rflag == 1)
                        {
                            Nodes[j].I = false;
                            Nodes[j].R = true;
                            Nodes[j].Rflag = 0;
                            a.FillEllipse(mySolidBrush2, (int)Nodes[j].X - 50 / m, (int)Nodes[j].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                        }
                        DrawLineChart();
                        DrawPieChart(NS, NI, NR);
                    }
                    //SIS
                    else if (radioButton1.Checked)
                    {
                        if (Nodes[j].Iflag == 1)
                        {
                            Nodes[j].S = false;
                            Nodes[j].I = true;
                            Nodes[j].Iflag = 0;
                            Nodes[j].RecoveryTime = RecoveryTime;
                            a.FillEllipse(mySolidBrush1, (int)Nodes[j].X - 50 / m, (int)Nodes[j].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);

                        }

                        if (Nodes[j].Rflag == 1)
                        {
                            Nodes[j].I = false;
                            Nodes[j].S = true;
                            Nodes[j].Rflag = 0;
                            a.FillEllipse(mySolidBrush3, (int)Nodes[j].X - 50 / m, (int)Nodes[j].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                        }
                        DrawLineChart();
                        DrawPieChart(NS, NI, NR);
                    }
                    if (Nodes[j].I == true)
                        listBox1.Items.Add("  (" + Nodes[j].i + ", " + Nodes[j].j + ")" + "                           Заражен(I)                                    " + Nodes[j].CountR + "                                   " + Nodes[j].CountTime);
                    else if (Nodes[j].S == true)
                        listBox1.Items.Add("  (" + Nodes[j].i + ", " + Nodes[j].j + ")" + "                           Восприимчив(S)                           " + Nodes[j].CountR + "                                   " + Nodes[j].CountTime);
                    else if (Nodes[j].Rem == true)
                        listBox1.Items.Add("  (" + Nodes[j].i + ", " + Nodes[j].j + ")" + "                           Удален(Rem)                                " + Nodes[j].CountR + "                                   " + Nodes[j].CountTime);
                    else if (Nodes[j].R == true)
                        listBox1.Items.Add("  (" + Nodes[j].i + ", " + Nodes[j].j + ")" + "                           Восстановлен(R)                         " + Nodes[j].CountR + "                                   " + Nodes[j].CountTime);
                    textBox8.Text = NS.ToString();
                    textBox9.Text = NI.ToString();
                    textBox10.Text = NR.ToString();
                }
                mySolidBrush1.Dispose();
                mySolidBrush2.Dispose();
                mySolidBrush3.Dispose();

            });

        }

        private void button5_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        //Изменение графа в ручную
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            
            int x = e.X;
            int y = e.Y;
            double minDest1 = double.PositiveInfinity;
            double minDest2 = double.PositiveInfinity;
            double Dest;
            int k = 0;
            int l = 0;
            for (int i=0; i< Nodes.Count; i++)
            {
                Dest = Math.Sqrt(Math.Pow(x - Nodes[i].X, 2) + Math.Pow(y-Nodes[i].Y,2));
                if (Dest < minDest1)
                {
                    minDest1 = Dest;
                    k = i;
                }

            }
            for (int i = 0; i < Nodes.Count; i++)
            {
                Dest = Math.Sqrt(Math.Pow(x - Nodes[i].X, 2) + Math.Pow(y - Nodes[i].Y, 2));
                if (Dest < minDest2 && Dest!=minDest1)
                {
                    minDest2 = Dest;
                    l = i;
                }
            }
                        
            var a = CreateGraphics();
            SolidBrush mySolidBrushI = new SolidBrush(Color.Red);
            SolidBrush mySolidBrushS = new SolidBrush(Color.Yellow);
            SolidBrush mySolidBrushR = new SolidBrush(Color.Green);
            SolidBrush mySolidBrushRem = new SolidBrush(Color.White);

            if (checkBox2.Checked)
            {   if (radioButton5.Checked)
                {
                    if (radioButton1.Checked)
                    {
                        if (Nodes[k].S == true)
                        {
                            Nodes[k].I = true;
                            Nodes[k].S = false;
                            NI++;
                            NS--;
                            a.FillEllipse(mySolidBrushI, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                        }
                        else if (Nodes[k].I == true)
                        {
                            Nodes[k].I = false;
                            Nodes[k].S = true;
                            NS++;
                            NI--;
                            a.FillEllipse(mySolidBrushS, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                        }

                    }

                    else if (radioButton2.Checked)
                    {
                        if (Nodes[k].S == true)
                        {
                            Nodes[k].I = true;
                            Nodes[k].S = false;
                            NI++;
                            NS--;
                            a.FillEllipse(mySolidBrushI, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                        }
                        else if (Nodes[k].I == true)
                        {
                            Nodes[k].I = false;
                            Nodes[k].R = true;
                            NI--;
                            NR++;
                            a.FillEllipse(mySolidBrushR, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                        }

                        else if (Nodes[k].R == true)
                        {
                            Nodes[k].R = false;
                            Nodes[k].S = true;
                            NS++;
                            NR--;
                            a.FillEllipse(mySolidBrushS, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                        }
                    }
                }
                else if (radioButton6.Checked)
                {
                    if (Nodes[k].S == true)
                    {
                        Nodes[k].Rem = true;
                        Nodes[k].S = false;
                        NS--;
                        a.FillEllipse(mySolidBrushRem, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                    }
                    else if (Nodes[k].Rem == true)
                    {
                        Nodes[k].Rem = false;
                        Nodes[k].S = true;
                        NS++;
                        a.FillEllipse(mySolidBrushS, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                    }
                }


            }
            if (checkBox3.Checked)
            {
                if (radioButton4.Checked)
                {
                     Pen WhitePen = new Pen(Color.White, 3);
                    a.DrawLine(WhitePen, Nodes[k].X, Nodes[k].Y, Nodes[l].X, Nodes[l].Y);
                    WhitePen.Dispose();
                }
                else if (radioButton3.Checked)
                {
                    Pen WhitePen = new Pen(Color.Green, 3);
                    a.DrawLine(WhitePen, Nodes[k].X, Nodes[k].Y, Nodes[l].X, Nodes[l].Y);
                    WhitePen.Dispose();
                }
                    if (Nodes[k].I == true)
                        a.FillEllipse(mySolidBrushI, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                    else if (Nodes[k].S == true)
                        a.FillEllipse(mySolidBrushS, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                    else if (Nodes[k].Rem == true)
                        a.FillEllipse(mySolidBrushRem, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                    else if (Nodes[k].R == true)
                    a.FillEllipse(mySolidBrushR, (int)Nodes[k].X - 50 / m, (int)Nodes[k].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);

                    if (Nodes[l].I == true)
                        a.FillEllipse(mySolidBrushI, (int)Nodes[l].X - 50 / m, (int)Nodes[l].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                    else if (Nodes[l].S == true)
                        a.FillEllipse(mySolidBrushS, (int)Nodes[l].X - 50 / m, (int)Nodes[l].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                    else if (Nodes[l].Rem == true)
                        a.FillEllipse(mySolidBrushRem, (int)Nodes[l].X - 50 / m, (int)Nodes[l].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
                    else if (Nodes[l].R == true)
                        a.FillEllipse(mySolidBrushR, (int)Nodes[l].X - 50 / m, (int)Nodes[l].Y - 50 / m, (50 / m) * 2, (50 / m) * 2);
             }
            mySolidBrushS.Dispose();
            mySolidBrushI.Dispose();
            mySolidBrushR.Dispose();
            mySolidBrushRem.Dispose();
        }

        private void DrawLineChart()
        {
            var series = new Series("S");

            chart2.Series[0].Points.AddXY(NX, NS);
            chart2.Series[1].Points.AddXY(NX, NI);
            chart2.Series[2].Points.AddXY(NX, NR);

        }
        private void DrawPieChart(int value1, int value2, int value3)
        {
            //reset your chart series and legends
            chart1.Series.Clear();
            chart1.Legends.Clear();
            chart1.Palette = ChartColorPalette.None;
            chart1.PaletteCustomColors = new Color[] { Color.Gold, Color.Red, Color.Lime };


            //Add a new chart-series
            string seriesname = "MySeriesName";
            chart1.Series.Add(seriesname);
            //set the chart-type to "Pie"
            chart1.Series[seriesname].ChartType = SeriesChartType.Pie;

            //Add some datapoints so the series. in this case you can pass the values to this method
             

            chart1.Series[seriesname].Points.AddXY("S", value1);
            chart1.Series[seriesname].Points.AddXY("I", value2);
            chart1.Series[seriesname].Points.AddXY("R", value3);
            

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }


        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
    



