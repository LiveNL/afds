using System;
using System.Collections.Generic;
using System.IO;
using MathNet.Numerics.Random;

namespace afds {
    public class Probabilities {
        //Declaring global variables.
        private static MersenneTwister random;
        static int[] runtimes_a;
        static int[] runtimes_b;
        static Dictionary<string, double[]> rates_a;
        static Dictionary<string, double[]> rates_b;

        public static int[] Runtimes_a { get => runtimes_a; set => runtimes_a = value; }
        public static int[] Runtimes_b { get => runtimes_b; set => runtimes_b = value; }
        public static Dictionary<string, double[]> Rates_a { get => rates_a; set => rates_a = value; }
        public static Dictionary<string, double[]> Rates_b { get => rates_b; set => rates_b = value; }


        public static void InitProbabilities()
        {
            //Initializing global variables.
            random = new MersenneTwister(12);
            // TODO: fix extra station 666 (as the bus has only 12)
            // TODO: remove 666, just turn around
            Runtimes_a = new int[] { 110, 78, 82, 60, 100, 59, 243, 135 };
            Runtimes_b = new int[] { 134, 243, 59, 101, 60, 86, 78, 113 };
            const string Filepath = "./rates_a.csv";
            Rates_a = ReadCsv(Filepath);
            const string Filepath1 = "./rates_b.csv";
            Rates_b = ReadCsv(Filepath1);
        }

        //The functions that are directly called in the simulation.
        public static int CalcRunTime(int mean)
        {
            double stdev = 0.0909186 - 0.0001444 * mean;
            return (int)CalcLogNormal(Math.Log((double)mean), stdev);
        }

        public static int CalcDwellingTime(int passengers_in, int passengers_out)
        {
            double mean = 12.5 + 0.22 * passengers_in + 0.13 * passengers_out;
            double min = 0.8 * mean;
            double gamma = GenerateGammaValue(2, mean / 2);
            if (gamma < min) return (int)min;
            else return (int)gamma;
        }

        static int CalcSecondDwellingTime(int passengers_in)
        {
            double mean = 0.22 * passengers_in;
            double min = 0.8 * mean;
            double gamma = GenerateGammaValue(2, mean / 2);
            if (gamma < min) return (int)min;
            else return (int)gamma;
        }

        public static int GeneratePassengerArrivals(DateTime begin, DateTime end, double[] rates)
        {
            int res = 0;
            int begin_i = TimeToIndex(begin);
            int end_i = TimeToIndex(end);
            double seconds = begin.Second + (60.0 * begin.Minute);
            if (rates[begin_i] == 0) return 0;
            int j = begin_i;
            while (seconds <= (double)end.Second + 60.0 * end.Minute + 3600.0 * (end.Hour - begin.Hour))
            {
                double exp = CalcExp(rates[j]);

                if (seconds + exp > (double)end.Second + 60.0 * end.Minute + 3600.0 * (end.Hour - begin.Hour))
                {
                    double end_time = (double)end.Second + 60.0 * end.Minute + 3600.0 * (end.Hour - begin.Hour);
                    double fraction = (end_time - seconds) / exp;
                    if (fraction >= random.NextDouble())
                        res++;
                    break;
                }

                if ((int)((seconds + exp) / 900) > (int)(seconds / 900))
                {
                    j++;
                    double goto_time = (double)((int)(seconds + exp) / 900) * 900.0;
                    double fraction = (goto_time - seconds) / exp;
                    if (fraction < random.NextDouble())
                        res--;
                    seconds = goto_time;
                }

                res++;
                seconds += exp;
            }

            return res;
        }

        //The probability functions that are used in determining the random variables.
        static double GenerateGammaValue(int k, double theta)
        {
            //calculate for theta equals 1, then multiply by theta
            double d = k - 1 / 3;
            double c = 1 / Math.Sqrt(9 * d);
            while(true)
            {
                double x = CalcNormal();
                double u = random.NextDouble();
                double v = Math.Pow((1 + c * x), 3);
                if (v > 0 && Math.Log(u) < 0.5 * (x * x) + d - d * v + d * Math.Log(v))
                    return d * v * theta;
            }
        }

        static double CalcLogNormal(double mu, double sigma)
        {
            double y = CalcNormal(mu, sigma);
            return (Math.Pow(Math.E, y));
        }

        static double CalcNormal(double mean = 0, double std = 1)
        {
            //Calculated using the Box Muller transform
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();
            return (Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2)) * std + mean;
        }

        static double CalcExp(double rate)
        {
            double u = random.NextDouble();
            return -(1.0 / rate) * Math.Log(u);
        }

        //Other support functions used.
        static int TimeToIndex(DateTime time)
        {
            //06:00-06:14 -> 0
            //06:15-06:29 -> 1
            //...
            //21:15-21:29 -> 61
            //exception: 21:30 -> 61
            DateTime quarter = time.AddMinutes(-(time.Minute % 15));
            TimeSpan diff = quarter - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0);
            int res = (int)diff.TotalMinutes / 15;
            return res >= 61 ? 61 : res;
        }

        static Dictionary<string, double[]> ReadCsv(string filepath)
        {
            Dictionary<string, double[]> res = new Dictionary<string, double[]>();
            using (var reader = new StreamReader(filepath))
            {
                List<string> list = new List<string>();
                var first_line = reader.ReadLine();
                var names = first_line.Split(';');
                for (int i = 1; i < names.Length; i++)
                {
                    res.Add(names[i], new double[62]);
                }
                int n = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');
                    for (int i = 1; i < values.Length; i++)
                    {
                        res[names[i]][n] = double.Parse(values[i]);
                    }
                    n++;
                }
            }
            return res;
        }
    }
}
