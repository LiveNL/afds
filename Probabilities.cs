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
        static Dictionary<string, double[]> exit_rates_a;
        static Dictionary<string, double[]> exit_rates_b;

        public static int[] Runtimes_a { get => runtimes_a; set => runtimes_a = value; }
        public static int[] Runtimes_b { get => runtimes_b; set => runtimes_b = value; }
        public static Dictionary<string, double[]> Rates_a { get => rates_a; set => rates_a = value; }
        public static Dictionary<string, double[]> Rates_b { get => rates_b; set => rates_b = value; }
        public static Dictionary<string, double[]> Exit_rates_a { get => exit_rates_a; set => exit_rates_a = value; }
        public static Dictionary<string, double[]> Exit_rates_b { get => exit_rates_b; set => exit_rates_b = value; }

        public static void InitProbabilities()
        {
            //Initializing global variables.
            random = new MersenneTwister(12);
            Runtimes_a = new int[] { 110, 78, 82, 60, 100, 59, 243, 135 };
            Runtimes_b = new int[] { 134, 243, 59, 101, 60, 86, 78, 113 };
            const string Filepath = "./data/rates_a.csv";
            Rates_a = ReadCsv(Filepath);
            const string Filepath1 = "./data/rates_b.csv";
            Rates_b = ReadCsv(Filepath1);
            const string Filepath2 = "./data/new_exit_rates_a.csv";
            Exit_rates_a = ReadCsv(Filepath2);
            const string Filepath3 = "./data/new_exit_rates_b.csv";
            Exit_rates_b = ReadCsv(Filepath3);

            // Verification of artificial input
            //
            // const string FilePathV1 = "./input-data-passengers-01.csv";
            // Rates_a = ReadValidationCsv(FilePathV1, 0, 0);
            // Rates_b = ReadValidationCsv(FilePathV1, 1, 0);

            // Exit_rates_a = ReadValidationCsv(FilePathV1, 0, 1);
            // Exit_rates_b = ReadValidationCsv(FilePathV1, 1, 1);
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
            else return (int)gamma + CalcDoorJam(1);
        }

        public static int CalcSecondDwellingTime(int passengers_in)
        {
            double mean = 0.22 * passengers_in;
            double min = 0.8 * mean;
            double gamma = GenerateGammaValue(2, mean / 2);
            if (gamma < min) return (int)min;
            else return (int)gamma;
        }

        public static List<DateTime> GeneratePassengerArrivals(DateTime begin, DateTime end, double[] rates)
        {
            List<DateTime> passengers = new List<DateTime>();
            int begin_i = TimeToIndex(begin);
            int end_i = TimeToIndex(end);
            double seconds = begin.Second + (60.0 * begin.Minute);
            if (rates[begin_i] == 0) {
                double goto_time = (double)((int)(seconds) / 900) * 900.0;
                seconds = goto_time;
            }
            int j = begin_i;
            while (seconds < (double)end.Second + 60.0 * end.Minute + 3600.0 * (end.Hour - begin.Hour) && j <= 61)
            {
                if (rates[j] == 0)
                {
                    double goto_time = (double)((int)(seconds) / 900) * 900.0 + 900.0;
                    seconds = goto_time;
                    j++;
                    continue;
                }
                double exp = CalcExp(rates[j]);

                if (seconds + exp > (double)end.Second + 60.0 * end.Minute + 3600.0 * (end.Hour - begin.Hour))
                {
                    double end_time = (double)end.Second + 60.0 * end.Minute + 3600.0 * (end.Hour - begin.Hour);
                    double fraction = (end_time - seconds) / exp;
                    if (fraction >= random.NextDouble())
                        passengers.Add(begin.AddSeconds(seconds - (begin.Second + (60.0 * begin.Minute))));
                    break;
                }

                if ((int)((seconds + exp) / 900) > (int)(seconds / 900))
                {
                    j++;
                    double goto_time = (double)((int)(seconds + exp) / 900) * 900.0;
                    double fraction = (goto_time - seconds) / exp;
                    seconds = goto_time;
                    if (fraction >= random.NextDouble())
                        passengers.Add(begin.AddSeconds(seconds - (begin.Second + (60.0 * begin.Minute))));
                    seconds = goto_time;
                    continue;
                }

                seconds += exp;
                passengers.Add(begin.AddSeconds(seconds - (begin.Second + (60.0 * begin.Minute))));
            }

            return passengers;
        }

        public static int CalcExit(DateTime time, string stop, char dir, int occupation)
        {
            if (dir == 'a' && stop == "Centraal Station")
                return occupation;
            if (dir == 'b' && stop == "P+R De Uithof")
                return occupation;
            if (dir == 'a' && Array.IndexOf(new string[] { "P+R De Uithof", "WKZ", "UMC", "Heidelberglaan" }, stop) > -1)
                return 0;
            if (dir == 'b' && stop == "Centraal Station")
                return 0;

            double mean;
            if (dir == 'a')
                mean = Exit_rates_a[stop][TimeToIndex(time)] * occupation;
            else
                mean = Exit_rates_b[stop][TimeToIndex(time)] * occupation;

            double stdev = 0.06756 * mean;
            int norm = (int)CalcNormal(mean, stdev);
            if (norm > occupation)
                return occupation;
            return norm > 0 ? norm : 0;
        }

        public static int CalcDoorJam(int x)
        {
            if (random.NextDouble() * 100 < x)
                return 60;
            else return 0;
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
            TimeSpan diff = quarter.TimeOfDay - new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 0, 0).TimeOfDay;
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

        static Dictionary<string, double[]> ReadValidationCsv(string filepath, int InOut, int Dir) {
          Dictionary<string, double[]> res = new Dictionary<string, double[]>();

          using (var reader = new StreamReader(filepath)) {
            List<string> list = new List<string>();
            var first_line    = reader.ReadLine(); // omit this line

            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                var values = line.Split(';');

                string name    = StationName(values[0]);
                int dir        = Int32.Parse(values[1]);
                int from       = Int32.Parse(values[2]);
                double to      = double.Parse(values[3]);
                double passIn  = double.Parse(values[4]);
                double passOut = double.Parse(values[5]);

                if (Dir == dir) {
                  if (!res.ContainsKey(name)) { res.Add(name, new double[62]); }

                  if (InOut == 0) {
                    WriteX(from, to, res, name, passIn);
                  } else {
                    WriteX(from, to, res, name, passOut);
                  }
                }
            }
          }
          return res;
        }

        static Dictionary<string, double[]> WriteX(int begin, double end, Dictionary<string, double[]> res, string name, double p) {
          string bS = $"{begin}:00:00";
          string eS;

          if (end == 21.5) {
            eS = "21:30:00";
          } else {
            eS = $"{end}:00:00";
          }

          DateTime beginDt = DateTime.Parse(bS.ToString());
          DateTime endDt   = DateTime.Parse(eS.ToString());

          double diff = (double)(endDt - beginDt).TotalSeconds;

          for (DateTime dt = beginDt; dt < endDt; dt = dt.AddMinutes(15)) {
            int n = TimeToIndex(dt);
            double v = (p / diff);
            res[name][n] = (double)v;
          }

          return res;
        }

        static string StationName(string name) {
          var map = new Dictionary<string, string>();
          map.Add("P+R Uithof",                    "P+R De Uithof");
          map.Add("WKZ",                           "WKZ");
          map.Add("UMC",                           "UMC");
          map.Add("Heidelberglaan",                "Heidelberglaan");
          map.Add("Padualaan",                     "Padualaan");
          map.Add("Kromme Rijn",                   "Kromme Rijn");
          map.Add("Galgenwaard",                   "Galgenwaard");
          map.Add("Vaartscherijn",                 "Vaartsche Rijn");
          map.Add("Centraal Station Centrumzijde", "Centraal Station");
          return map[name];
        }
    }
}
