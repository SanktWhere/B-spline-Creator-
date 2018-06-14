using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        private Bspline MyNurbs = new Bspline();
        List<double> knotVector = new List<double>();
        Graphics g;
        Graphics g1;

        

        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
            g1 = pictureBox2.CreateGraphics();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            knotVector.Add(0);
            MyNurbs.WeightedPointSeries.Add(new Point(e.X, e.Y));
            g.DrawEllipse(new Pen(Color.Blue, 4), e.X, e.Y, 5, 5);
            label1.Text += "{" + e.X + " " + e.Y + "}  ";
        }

        float koef = 375;
        private void button1_Click(object sender, EventArgs e)
        {
            koef += 0.8f;
            int deg = MyNurbs.WeightedPointSeries.Count - 1;
            for (int i = 0; i < MyNurbs.WeightedPointSeries.Count; i++)
            {
                knotVector.Add(1);
            }
            double p = koef + MyNurbs.WeightedPointSeries.Count;
            PlotPoints(MyNurbs.BSplineCurve(MyNurbs.WeightedPointSeries, deg, knotVector, 0.05));
            DrawPoints(MyNurbs.l, (float)p, 500, 0, pictureBox2.Height - 5, new Pen(Color.Red));

            DrawT(MyNurbs.l, (float)p, 1, 0, pictureBox2.Height, new Pen(Color.Black));
            button1.Hide();
        }

        List<PointF> res = new List<PointF>();
        public void DrawT(List<PointF> p, float koefX, float koefY, int x, int y, Pen pen)
        {
            for (int i = 0; i < p.Count - 3; i += 3)
            {
                float x1 = p[i + 3].X * koefX;
                float y1 = y + 10;
                float x2 = p[i + 3].X * koefX + x;
                float y2 = y - 10;

                Label label2 = new Label();
                label2.Font = new Font(label1.Font.FontFamily, 6, label1.Font.Style);
                label2.Location = new Point((int)x2 + pictureBox2.Location.X - 10, (int)y2 + 35);
                label2.Size = new Size(20, 12);
                label2.Text = p[i + 3].X.ToString();
                this.Controls.Add(label2);

                g1.DrawLine(pen, x1, y1, x2, y2);
            }
        }
        private void DrawPoints(List<PointF> p, float koefX, float koefY, int x, int y, Pen pen)
        {

            List<PointF> res = new List<PointF>();
            List<PointF> res1 = new List<PointF>();
            res1 = p;

            int k = MyNurbs.WeightedPointSeries.Count;
            int i = 1;
            int c = 0;
            List<int> usesdIndex = new List<int>();
            while (i <= k)
            {
                if (c < p.Count - k)
                {
                    res.Add(new PointF(p[c].X * koefX + x, -p[c].Y * koefY + y));
                    res.Add(new PointF(p[c + k].X * koefX + x, -p[c + k].Y * koefY + y));
                    g1.DrawLine(pen, p[c].X * koefX + x, -p[c].Y * koefY + y, p[c + k].X * koefX + x, -p[c + k].Y * koefY + y);
                    usesdIndex.Add(c);
                    usesdIndex.Add(c + k);
                    c += k;
                }
                else
                {
                    c = 0;
                    c += i;
                    i++;
                }
            }
            res.Add(new PointF(0, 0));
        }

        List<PointF> resultPoints = new List<PointF>();
        private List<PointF> PlotPoints(List<PointF> p)
        {
            for (int i = 0; i <= p.Count - 2; i++)
            {
                resultPoints.Add(new PointF(p[i].X, p[i].Y));
                resultPoints.Add(new PointF(p[i + 1].X, p[i + 1].Y));
                g.DrawLine(new Pen(Color.Red), p[i].X, p[i].Y, p[i + 1].X, p[i + 1].Y);
            }
            return resultPoints;
        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if ((res.Contains(new PointF(e.X, e.Y))) || (res.Contains(new PointF(e.X + 10, e.Y))) || (res.Contains(new PointF(e.X, e.Y + 10))) || (res.Contains(new PointF(e.X - 10, e.Y))) || (res.Contains(new PointF(e.X, e.Y - 10))))
            {
                DrawPoints(MyNurbs.l, koef + MyNurbs.WeightedPointSeries.Count, 500, 0, pictureBox2.Height - 5, new Pen(Color.Blue));
            }
        }

        public bool poointInListPoint(PointF p, List<PointF> listPoint)
        {
            for (int i = 0; i < listPoint.Count; i++)
            {
                if (((listPoint[i].X == p.X) && (listPoint[i].Y == p.Y)) ||
                    ((listPoint[i].Y - p.Y <= 10) && (listPoint[i].X - p.X <= 10)) ||
                    ((listPoint[i].Y - p.Y <= -10) && (listPoint[i].X - p.X <= -10)))
                {
                    return true;
                }
            }
            return false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        int clickCount = 0;
        PointF p1, p2;

        private void button3_Click(object sender, EventArgs e)
        {
            MyNurbs.WeightedPointSeries.Clear();
            knotVector.Clear();
            resultPoints.Clear();
            MyNurbs.l.Clear();
            g.Clear(Color.WhiteSmoke);
            g1.Clear(Color.WhiteSmoke);
            label1.Text = " ";
            button1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clickCount++;
            if (clickCount == 1)
            {
                p1 = new PointF((float)Convert.ToDouble(textBox1.Text) * (koef + MyNurbs.WeightedPointSeries.Count), pictureBox2.Location.Y - 20);
                p2 = new PointF((float)Convert.ToDouble(textBox1.Text) * (koef + MyNurbs.WeightedPointSeries.Count), pictureBox2.Height + 100);

            }
            else
            {
                g1.DrawLine(new Pen(Color.White), p1, p2);
                p1 = new PointF((float)Convert.ToDouble(textBox1.Text) * (koef + MyNurbs.WeightedPointSeries.Count), pictureBox2.Location.Y - 20);
                p2 = new PointF((float)Convert.ToDouble(textBox1.Text) * (koef + MyNurbs.WeightedPointSeries.Count), pictureBox2.Height + 100);
            }

            float x1 = (float)Convert.ToDouble(textBox1.Text) * (koef + MyNurbs.WeightedPointSeries.Count);
            float y1 = pictureBox2.Location.Y - 20;
            float x2 = (float)Convert.ToDouble(textBox1.Text) * (koef + MyNurbs.WeightedPointSeries.Count);
            float y2 = pictureBox2.Height + 100;
            g1.DrawLine(new Pen(Color.Green), x1, y1, x2, y2);

            int x = (int)(Convert.ToDouble(textBox1.Text) * 39);
            int y = (int)(Convert.ToDouble(textBox1.Text) * 39);
            if (resultPoints.Count == 0)
            {
                MessageBox.Show("Постройте кривую");
            }
            else if ((Convert.ToDouble(textBox1.Text) >= 0) && (Convert.ToDouble(textBox1.Text) <= 1))
            {
                 g.DrawEllipse(new Pen(Color.Green, 5), resultPoints[x].X, resultPoints[y].Y, 5, 5);
            }
            else
            {
                MessageBox.Show("Введите число в промужутке от 0 до 1");
            }
            

        }
    }
}
