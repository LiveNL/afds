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
            return(res);
        }
    }
}