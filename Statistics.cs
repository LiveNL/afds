using System;
using System.Collections.Generic;

namespace afds {
    public class Statistics {

        private static double[] _WaitingTime; //0: All Day, 1: Maximum Frequency, 2: Morning Rush, 3: Evening Rush

        public static void WaitingTime(double wt, DateTime dt) {
            _WaitingTime[0] += wt;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                _WaitingTime[1] += wt;
                if (dt < DateTime.Parse("9:00:00 AM")) _WaitingTime[2] += wt;
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) _WaitingTime[3] += wt;
            }
        }
        
        private static int[] _Passengers; //0: All Day, 1: Maximum Frequency, 2: Morning Rush, 3: Evening Rush
        public static void Passengers(int p, DateTime dt) {
            _Passengers[0] += p;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                _Passengers[1] += p;
                if (dt < DateTime.Parse("9:00:00 AM")) _Passengers[2] += p;
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) _Passengers[3] += p;
            }
        }

        private static double[] _MaxWait; //0: All Day, 1: Maximum Frequency, 2: Morning Rush, 3: Evening Rush

        public static void MaxWait(double w, DateTime dt) {
            if (w > _MaxWait[0]) _MaxWait[0] = w;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                if (w > _MaxWait[1]) _MaxWait[1] = w;
                if (dt < DateTime.Parse("9:00:00 AM")) {
                    if (w > _MaxWait[2]) _MaxWait[2] = w;
                }
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) {
                    if (w > _MaxWait[3]) {
                        _MaxWait[3] = w;
                    }
                }
            }
        }

        private static double[] _Delay;
        private static int[] _OneMin;

        public static void Delay(double d, DateTime dt) {
            _Delay[0] += d;
            if (d >= 60.0) _OneMin[0]++;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                _Delay[1] += d;
                if (d >= 60.0) _OneMin[1]++;
                if (dt < DateTime.Parse("9:00:00 AM")) {
                    _Delay[2] += d;
                    _OneMin[2]++;
                }
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) {
                    _Delay[3] += d;
                    _OneMin[3]++;
                }
            }
        }

        private static int[] _DelayChecks;
        public static void DelayChecks(int d, DateTime dt) {
            _DelayChecks[0] += d;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                _DelayChecks[1] += d;
                if (dt < DateTime.Parse("9:00:00 AM")) _DelayChecks[2] += d;
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) _DelayChecks[3] += d;
            }
        }

        private static double[] _MaxDelay;
        public static void MaxDelay(double d, DateTime dt) {
            if (d > _MaxDelay[0]) _MaxDelay[0] = d;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                if (d > _MaxDelay[1]) _MaxDelay[1] = d;
                if (dt < DateTime.Parse("9:00:00 AM")) {
                    if (d > _MaxDelay[2]) _MaxDelay[2] = d;
                }
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) {
                    if (d > _MaxDelay[3]) {
                        _MaxDelay[3] = d;
                    }
                }
            }
        }

        public static void InitStatistics() {
            _Passengers = new int[] { 0, 0, 0, 0 };
            _WaitingTime = new double[] { 0.0, 0.0, 0.0, 0.0 };
            _MaxWait = new double[] { 0.0, 0.0, 0.0, 0.0 };

            _DelayChecks = new int[] { 0, 0, 0, 0 };
            _OneMin = new int[] { 0, 0, 0, 0 };
            _Delay = new double[] { 0.0, 0.0, 0.0, 0.0 };
            _MaxDelay = new double[] { 0.0, 0.0, 0.0, 0.0 };
        }

        public static string Results() {
            string res = "";
            
            res += "WAITING TIMES FOR PASSENGERS:\n";
            res += "\tAll Day (07:00 - 21:30)\n";
            res += string.Format("\t\tAverage Waiting Time: {0} seconds\n", ((int)(_WaitingTime[0] / _Passengers[0])).ToString());
            res += string.Format("\t\tMaximum Waiting Time: {0} seconds\n", ((int)_MaxWait[0]).ToString());
            res += "\tMaximum Frequency (07:00 - 19:00)\n";
            res += string.Format("\t\tAverage Waiting Time: {0} seconds\n", ((int)(_WaitingTime[1] / _Passengers[1])).ToString());
            res += string.Format("\t\tMaximum Waiting Time: {0} seconds\n", ((int)_MaxWait[1]).ToString());
            res += "\tMorning Rush (07:00 - 09:00)\n";
            res += string.Format("\t\tAverage Waiting Time: {0} seconds\n", ((int)(_WaitingTime[2] / _Passengers[2])).ToString());
            res += string.Format("\t\tMaximum Waiting Time: {0} seconds\n", ((int)_MaxWait[2]).ToString());
            res += "\tEvening Rush (16:00 - 18:00)\n";
            res += string.Format("\t\tAverage Waiting Time: {0} seconds\n", ((int)(_WaitingTime[3] / _Passengers[3])).ToString());
            res += string.Format("\t\tMaximum Waiting Time: {0} seconds\n", ((int)_MaxWait[3]).ToString());

            res += "\n";

            res += "DELAYS FOR TRAMS:\n";
            res += "\tAll Day (07:00 - 21:30)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_Delay[0] / _DelayChecks[0])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelay[0]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMin[0] / _DelayChecks[0] * 100.0)).ToString());
            res += "\tMaximum Frequency (07:00 - 19:00)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_Delay[1] / _DelayChecks[1])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelay[1]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMin[1] / _DelayChecks[1] * 100.0)).ToString());
            res += "\tMorning Rush (07:00 - 09:00)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_Delay[2] / _DelayChecks[2])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelay[2]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMin[2] / _DelayChecks[2] * 100.0)).ToString());
            res += "\tEvening Rush (16:00 - 18:00)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_Delay[3] / _DelayChecks[3])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelay[3]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMin[3] / _DelayChecks[3] * 100.0)).ToString());

            res += "\n";

            res += "OTHER INFORMATION:\n";
            res += string.Format("\tNumber of passengers handled: {0}", _Passengers[0].ToString());

            return(res);
        }
    }
}
