using System;
using System.Collections.Generic;

namespace afds {
    public class Statistics {

        static int n = 100;

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
                    if (d >= 60.0) _OneMin[2]++;
                }
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) {
                    _Delay[3] += d;
                    if (d >= 60.0) _OneMin[3]++;
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

         private static double[] _DelayPR;
        private static int[] _OneMinPR;

        public static void DelayPR(double d, DateTime dt) {
            _DelayPR[0] += d;
            if (d >= 60.0) _OneMinPR[0]++;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                _DelayPR[1] += d;
                if (d >= 60.0) _OneMinPR[1]++;
                if (dt < DateTime.Parse("9:00:00 AM")) {
                    _DelayPR[2] += d;
                    if (d >= 60.0) _OneMinPR[2]++;
                }
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) {
                    _DelayPR[3] += d;
                    if (d >= 60.0) _OneMinPR[3]++;
                }
            }
        }

        private static int[] _DelayChecksPR;
        public static void DelayChecksPR(int d, DateTime dt) {
            _DelayChecksPR[0] += d;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                _DelayChecksPR[1] += d;
                if (dt < DateTime.Parse("9:00:00 AM")) { _DelayChecksPR[2] += d; }
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) _DelayChecksPR[3] += d;
            }
        }

        private static double[] _MaxDelayPR;
        public static void MaxDelayPR(double d, DateTime dt) {
            if (d > _MaxDelayPR[0]) _MaxDelayPR[0] = d;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                if (d > _MaxDelayPR[1]) _MaxDelayPR[1] = d;
                if (dt < DateTime.Parse("9:00:00 AM")) {
                    if (d > _MaxDelayPR[2]) _MaxDelayPR[2] = d;
                }
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) {
                    if (d > _MaxDelayPR[3]) {
                        _MaxDelayPR[3] = d;
                    }
                }
            }
        }

        private static double[] _DelayCS;
        private static int[] _OneMinCS;

        public static void DelayCS(double d, DateTime dt) {
            _DelayCS[0] += d;
            if (d >= 60.0) _OneMinCS[0]++;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                _DelayCS[1] += d;
                if (d >= 60.0) _OneMinCS[1]++;
                if (dt < DateTime.Parse("9:00:00 AM")) {
                    _DelayCS[2] += d;
                    if (d >= 60.0) _OneMinCS[2]++;
                }
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) {
                    _DelayCS[3] += d;
                    if (d >= 60.0) _OneMinCS[3]++;
                }
            }
        }

        private static int[] _DelayChecksCS;
        public static void DelayChecksCS(int d, DateTime dt) {
            _DelayChecksCS[0] += d;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                _DelayChecksCS[1] += d;
                if (dt < DateTime.Parse("9:00:00 AM")) _DelayChecksCS[2] += d;
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) _DelayChecksCS[3] += d;
            }
        }

        private static double[] _MaxDelayCS;
        public static void MaxDelayCS(double d, DateTime dt) {
            if (d > _MaxDelayCS[0]) _MaxDelayCS[0] = d;
            if (dt >= DateTime.Parse("7:00:00 AM") && dt < DateTime.Parse("7:00:00 PM")) {
                if (d > _MaxDelayCS[1]) _MaxDelayCS[1] = d;
                if (dt < DateTime.Parse("9:00:00 AM")) {
                    if (d > _MaxDelayCS[2]) _MaxDelayCS[2] = d;
                }
                else if (dt >= DateTime.Parse("4:00:00 PM") && dt < DateTime.Parse("6:00:00 PM")) {
                    if (d > _MaxDelayCS[3]) {
                        _MaxDelayCS[3] = d;
                    }
                }
            }
        }

        private static int[] _AllWaitingTime = new int[n];
        private static int[] _AllDelay = new int[n];
        private static int[] _AllDelayPR = new int[n];
        private static int[] _AllDelayCS = new int[n];
        private static int[] _AllOneTime = new int[n];
        private static int[] _AllOneTimePR = new int[n];
        private static int[] _AllOneTimeCS = new int[n];

        public static void InitStatistics() {
            _Passengers = new int[] { 0, 0, 0, 0 };
            _WaitingTime = new double[] { 0.0, 0.0, 0.0, 0.0 };
            _MaxWait = new double[] { 0.0, 0.0, 0.0, 0.0 };

            _DelayChecks = new int[] { 0, 0, 0, 0 };
            _OneMin = new int[] { 0, 0, 0, 0 };
            _Delay = new double[] { 0.0, 0.0, 0.0, 0.0 };
            _MaxDelay = new double[] { 0.0, 0.0, 0.0, 0.0 };

            _DelayChecksPR = new int[] { 0, 0, 0, 0 };
            _OneMinPR = new int[] { 0, 0, 0, 0 };
            _DelayPR = new double[] { 0.0, 0.0, 0.0, 0.0 };
            _MaxDelayPR = new double[] { 0.0, 0.0, 0.0, 0.0 };

            _DelayChecksCS = new int[] { 0, 0, 0, 0 };
            _OneMinCS = new int[] { 0, 0, 0, 0 };
            _DelayCS = new double[] { 0.0, 0.0, 0.0, 0.0 };
            _MaxDelayCS = new double[] { 0.0, 0.0, 0.0, 0.0 };
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

            res += "DELAYS FOR TRAMS (P+R DE UITHOF):\n";
            res += "\tAll Day (07:00 - 21:30)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_DelayPR[0] / _DelayChecksPR[0])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelayPR[0]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMinPR[0] / _DelayChecksPR[0] * 100.0)).ToString());
            res += "\tMaximum Frequency (07:00 - 19:00)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_DelayPR[1] / _DelayChecksPR[1])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelayPR[1]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMinPR[1] / _DelayChecksPR[1] * 100.0)).ToString());
            res += "\tMorning Rush (07:00 - 09:00)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_DelayPR[2] / _DelayChecksPR[2])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelayPR[2]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMinPR[2] / _DelayChecksPR[2] * 100.0)).ToString());
            res += "\tEvening Rush (16:00 - 18:00)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_DelayPR[3] / _DelayChecksPR[3])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelayPR[3]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMinPR[3] / _DelayChecksPR[3] * 100.0)).ToString());

            res += "\n";

            res += "DELAYS FOR TRAMS (CENTRAAL STATION):\n";
            res += "\tAll Day (07:00 - 21:30)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_DelayCS[0] / _DelayChecksCS[0])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelayCS[0]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMinCS[0] / _DelayChecksCS[0] * 100.0)).ToString());
            res += "\tMaximum Frequency (07:00 - 19:00)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_DelayCS[1] / _DelayChecksCS[1])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelayCS[1]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMinCS[1] / _DelayChecksCS[1] * 100.0)).ToString());
            res += "\tMorning Rush (07:00 - 09:00)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_DelayCS[2] / _DelayChecksCS[2])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelayCS[2]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMinCS[2] / _DelayChecksCS[2] * 100.0)).ToString());
            res += "\tEvening Rush (16:00 - 18:00)\n";
            res += string.Format("\t\tAverage Delay: {0} seconds\n", ((int)(_DelayCS[3] / _DelayChecksCS[3])).ToString());
            res += string.Format("\t\tMaximum Delay: {0} seconds\n", ((int)_MaxDelayCS[3]).ToString());
            res += string.Format("\t\tAt least 1m Delay: {0}%\n", ((int)((double)_OneMinCS[3] / _DelayChecksCS[3] * 100.0)).ToString());

            res += "\n";

            res += "OTHER INFORMATION:\n";
            res += string.Format("\tNumber of passengers handled: {0}\n", _Passengers[0].ToString());

            return(res);
        }

        public static void UpdateAll(int i) {
            _AllWaitingTime[i] = (int)(_WaitingTime[0] / _Passengers[0]);
            _AllDelay[i] = (int)(_Delay[0] / _DelayChecks[0]);
            _AllDelayPR[i] = (int)(_DelayPR[0] / _DelayChecksPR[0]);
            _AllDelayCS[i] = (int)(_DelayCS[0] / _DelayChecksCS[0]);
            _AllOneTime[i] = ((int)((double)_OneMin[0] / _DelayChecks[0] * 100.0));
            _AllOneTimeCS[i] = ((int)((double)_OneMinCS[0] / _DelayChecksCS[0] * 100.0));
            _AllOneTimePR[i] = ((int)((double)_OneMinPR[0] / _DelayChecksPR[0] * 100.0));
        }

        public static string ResultsAll() {
            string res = "WaitingTime;Delay;DelayPR;DelayCS;OneMin;OneMinCS;OneMinPR;\n";
            for (int i = 0; i < n; i++) {
                res += string.Format("{0};{1};{2};{3};{4};{5};{6};\n", _AllWaitingTime[i].ToString(), _AllDelay[i].ToString(), _AllDelayPR[i].ToString(), _AllDelayCS[i].ToString(), _AllOneTime[i].ToString(), _AllOneTimePR[i].ToString(), _AllOneTimeCS[i].ToString());
            }
            return res;
        }
    }
}
