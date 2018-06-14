using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    public class Bspline
    {

        private List<Point> pWeightedPointSeries = new List<Point>();

        public List<Point> WeightedPointSeries
        {
            get { return pWeightedPointSeries; }
            set
            {
                value = pWeightedPointSeries;
            }
        }

        private bool pIsBSpline = true;

        public bool IsBSpline
        {
            get { return pIsBSpline; }
            set
            {
                value = pIsBSpline;
            }
        }


        private double Nip(int i, int p, List<double> U, double u)
        {
            double[] N = new double[p + 1];
            double saved, temp;

            int m = U.Count;
            if ((i == 0 && u == U[0]) || (i == (m - p - 1) && u == U[m]))
                return 1;

            if (u < U[i] || u >= U[i + p + 1])
                return 0;

            for (int j = 0; j <= p; j++)
            {
                if (u >= U[i + j] && u < U[i + j + 1])
                    N[j] = 1d;
                else
                    N[j] = 0d;
            }

            for (int k = 1; k <= p; k++)
            {
                if (N[0] == 0)
                    saved = 0d;
                else
                    saved = ((u - U[i]) * N[0]) / (U[i + k] - U[i]);

                for (int j = 0; j < p - k + 1; j++)
                {
                    double Uleft = U[i + j + 1];
                    double Uright = U[i + j + k + 1];

                    if (N[j + 1] == 0)
                    {
                        N[j] = saved;
                        saved = 0d;
                    }
                    else
                    {
                        temp = N[j + 1] / (Uright - Uleft);
                        N[j] = saved + (Uright - u) * temp;
                        saved = (u - Uleft) * temp;
                    }
                }
            }
            return N[0];
        }

        public List<PointF> BSplineCurve(List<Point> Points, int Degree, List<double> KnotVector, double StepSize)
        {
            List<PointF> Result = new List<PointF>();
            if (Points.Count == 0)
            {
                return Result;
            }
            for (double i = 0; i < 1; i += StepSize)
            {
                if (this.IsBSpline)
                    Result.Add(BSplinePoint(Points, Degree, KnotVector, i));

            }

            ////////////if (!Result.Contains(Points[Points.Count - 1]))
                Result.Add(Points[Points.Count - 1]);

            return Result;
        }

        public List<PointF> l = new List<PointF>();
        PointF BSplinePoint(List<Point> Points, int degree, List<double> KnotVector, double t)
        {

            float x, y;
            double temp = 0;
            x = 0;
            y = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                temp = Nip(i, degree, KnotVector, t);
                l.Add(new PointF((float)t, (float)temp));
                x += Points[i].X * (float)temp;
                y += Points[i].Y * (float)temp;
            }

            return new PointF(x, y);
        }
    }
}
