using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Task1HotCup
{
    static class Program
    {
        public static double Func(double tempOut, double tempIn, double gamma, double delta)
        {
            return tempIn + delta * gamma * (tempOut - tempIn);
        }
        public static double FindTime(double tempOut, double t, double gamma, double delta, double tempFin)
        {
            double countTimeInMinutes = 0;
            while (t > tempFin)
            {
                t = Func(tempOut, t, gamma, delta);
                countTimeInMinutes += delta;
            }
            return countTimeInMinutes;
        }
        public static double FindGamma(double tempOut, double[] temperatures, double delta)
        {
            double theorTime = 0;
            double currentSquareDiff = 0;
            double res = 0;
            double squareTimeDiff = double.MaxValue;
            for (double gamma = 0.001; gamma < 0.1; gamma += 0.001)
            {
                for (int i = 0; i < temperatures.Length; i++)
                {
                    theorTime = FindTime(tempOut, temperatures[0], gamma, delta, temperatures[i]);
                    currentSquareDiff += (i  - theorTime) * (i  -theorTime);
                }
                if (currentSquareDiff < squareTimeDiff)
                {
                    squareTimeDiff = currentSquareDiff;
                    res = gamma;
                }
                currentSquareDiff = 0;
            }
            return res;
        }

        public static double FindTemp(double tempOut, double tempIn, double time, double delta, double gamma, double sratrTime)
        {
            double countTime = sratrTime;
            while (countTime < time)
            {
                tempIn = Func(tempOut, tempIn, gamma, delta);
                countTime += delta;
            }
            return tempIn;

        }

        public static double[] GetTheoreticalArray (double tempOut, double tempIn, double delta, double gamma, int n)
        {
            var theoreticalArray = new double[n];
            theoreticalArray[0] = tempIn;
            for (int i=1; i<n; i++)
                theoreticalArray[i] = FindTemp(tempOut, theoreticalArray[i-1], i , delta, gamma, i-1);
            return theoreticalArray;
        }     
        
        public static double FindDelta(double tempOut, double tempIn, double time, double gamma, double tempAn, double left, double right)
        {
            double T;
            while (left < right)
            {
                var middle = Math.Round((left + right) / 2, 4);
                T = FindTemp(tempOut, tempIn, time, middle, gamma, 0);
                if (0.001 <= Math.Round((Math.Abs(tempAn - T) / tempAn), 4))
                    right = Math.Round(middle, 4);
                else left = middle+0.0001;
            }
            T = FindTemp(tempOut, tempIn, time, right, gamma, 0);
            if (Math.Abs((Math.Abs(tempAn - T) / tempAn) - 0.001)<1e-4)
                return right;
            else throw new Exception("коэфициент delta не найден");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            
        }

    }
}
