using System;
using System.Collections.Generic;

namespace afds {
    public class Statistics {

        private static double _WaitingTime;
        public static double WaitingTime {
            get {
                return _WaitingTime;
            }
            set {
                _WaitingTime += value;
            }
         }
        private static int _Passengers;
        public static int Passengers {
            get {
                return _Passengers;
            }
            set {
                _Passengers += value;
            }
        }
        private static double _MaxWait;
        public static double MaxWait {
            get {
                return _MaxWait;
            }
            set {
                if (value > _MaxWait)
                    _MaxWait = value;
            }
        }

        private static double _Delay;
        public static double Delay {
          get { return _Delay; }
          set { _Delay += value; }
        }

        private static double _DelayChecks;
        public static double DelayChecks {
          get { return _DelayChecks; }
          set { _DelayChecks += value; }
        }

        private static double _MaxDelay;
        public static double MaxDelay {
          get { return _MaxDelay; }
          set { if (value > _MaxDelay) _MaxDelay = value; }
        }

        public static void InitStatistics() {
            _Passengers = 0;
            _WaitingTime = 0;
            _MaxWait = 0;
        }

        public static string Results() {
            string res = "";
            double avg_wait = _WaitingTime / _Passengers;
            res += "The average waiting time is: " + avg_wait.ToString() + " seconds.\n";
            res += "The maximum waiting time is: " + _MaxWait.ToString() + " seconds.\n";
            res += "The total number of passengers is: " + _Passengers.ToString() + " people.\n";

            double avg_delay = _Delay / _DelayChecks;
            res += "The average delay time is: " + Convert.ToInt32(avg_delay).ToString() + " seconds.\n";
            res += "The maximum delay time is: " + _MaxDelay.ToString() + " seconds.\n";
            res += "The total number of delayChecks is: " + _DelayChecks.ToString() + "\n";

            return(res);
        }
    }
}
