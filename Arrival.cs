using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Arrival {
    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int DepartureEventType = 0;

    public Arrival(Event e, Station station, Tram tram) {
      DateTime = e.DateTime;
      Station = station;
      Tram = tram;
    }

    public List<Event> ScheduleDeparture(List<Event> events, Uithoflijn uithoflijn) {
      events.Add(new Event(TimeAfterDwellTime(uithoflijn), DepartureEventType, Tram, Station));
      return events;
    }

    public DateTime TimeAfterDwellTime(Uithoflijn uithoflijn) {
      DateTime fstDwellTime = DateTime.AddSeconds(DwellTime(uithoflijn));
      DateTime expectedDeparture = fstDwellTime.AddSeconds(sndDwellTime(fstDwellTime, uithoflijn));
      return expectedDeparture;
    }

    public int DwellTime(Uithoflijn uithoflijn) {
      int dwellTime = Probabilities.CalcDwellingTime(Tram.PassengersIn(DateTime, 1, uithoflijn),
                                                     Tram.PassengersOut(DateTime));
      return dwellTime;
    }

    public int sndDwellTime(DateTime dt, Uithoflijn uithoflijn) {
      int dwellTime = Probabilities.CalcSecondDwellingTime(Tram.PassengersIn(dt, 2, uithoflijn));
      int extraTime;
      int diff = (int)ScheduleCheck(dt, dwellTime);

      DateTime totalDwellTimeSoFar = dt.AddSeconds(dwellTime);
      int sec = (int)totalDwellTimeSoFar.Subtract(DateTime).TotalSeconds;

      if (diff > 0) { // Tram is on time, diff just needs to be bigger than 180
        if (diff > 180) {
          extraTime = diff - sec;
        } else {
          extraTime = (int)(DateTime.AddSeconds(180).Subtract(totalDwellTimeSoFar)).TotalSeconds;
        }
      } else if (diff < 0) { // Tram is to late but needs to stand still 180 sec anyway
        if (diff < -180) {
          extraTime = 0;
        } else {
          extraTime = 180 - (diff * - 1);
        }
      } else { extraTime = 0; }

      return dwellTime + extraTime; //extraTime;
    }

    public double ScheduleCheck(DateTime fst, int snd) {
      int[] qStations = { 8, 9, 17, 0 };

      if (qStations.Contains(Station.Number)) {
        Statistics.DelayChecks = 1;
        double diff = (Tram.Start.AddMinutes(Tram.Schedule) - DateTime).TotalSeconds;

        if (Tram.Rounds == 0) {
          Tram.Rounds = Tram.Rounds + 1;
          Statistics.Delay = 0;
          return 0;
        } else if (DateTime > Tram.Start.AddMinutes(Tram.Schedule)) {
          Console.WriteLine("{0} : Schdule tram {1,-2} at {2,-2} is to late: {3}", DateTime, Tram.Number, Station.Number, diff);
          Tram.Start = Tram.Start.AddMinutes(Tram.Schedule);
          Tram.Rounds = Tram.Rounds + 1;

          // Statistics
          double positive_delay = (diff * -1);
          Statistics.MaxDelay = positive_delay;
          Statistics.Delay = positive_delay;

          return diff;
        } else {
          Console.WriteLine("{0} : Schdule tram {1,-2} at {2,-2} is on time: {3}", DateTime, Tram.Number, Station.Number, diff);

          // Statistics
          Tram.Start = Tram.Start.AddMinutes(Tram.Schedule);
          Tram.Rounds = Tram.Rounds + 1;
          Statistics.Delay = 0;

          return diff;
        }
      }
      return 0;
    }

    public void LogDwellTime(int i) {
      Console.WriteLine("{0} : Dwelltm tram {2,-2} at {3,-2} : {1} sec",
        DateTime, i, Tram.Number, Station.Number);
    }
  }
}
