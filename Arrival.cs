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
      // LogDwellTime(dwellTime);
      return dwellTime;
    }

    public int sndDwellTime(DateTime dt, Uithoflijn uithoflijn) {
      int dwellTime = Probabilities.CalcSecondDwellingTime(Tram.PassengersIn(dt, 2, uithoflijn));
      int extraTime;
      int diff = (int)ScheduleCheck(dt, dwellTime);

      DateTime totalDwellTimeSoFar = dt.AddSeconds(dwellTime);
      int sec = (int)totalDwellTimeSoFar.Subtract(DateTime).TotalSeconds;

      if (diff > 0) {
        // Tram is on time, diff just needs to be bigger than 180
        if (diff > 180) {
          extraTime = diff - sec;
          Console.WriteLine("ExtraTime: {0}", extraTime);
        } else {
          extraTime = (int)(DateTime.AddSeconds(180).Subtract(totalDwellTimeSoFar)).TotalSeconds;
          Console.WriteLine("ExtraTime: {0}", extraTime);
        }
      } else if (diff < 0) {
        // Tram is to late but needs to stand still 180 sec anyway
        extraTime = (int)(DateTime.AddSeconds(180).Subtract(totalDwellTimeSoFar)).TotalSeconds;
        Console.WriteLine("ExtraTime: {0}", extraTime);
      } else {
        extraTime = 0;
      }

      return dwellTime + extraTime; //extraTime;
    }

    public double ScheduleCheck(DateTime fst, int snd) {
      int[] qStations = { 8, 9, 17, 0 };
      if (qStations.Contains(Station.Number)) {
        // DateTime after = fst.AddSeconds(snd);
        // double totalDwellTime = after.Subtract(DateTime).TotalSeconds;
        // Console.WriteLine("TOTAL DWELLTIME: {0}", totalDwellTime);

        if (Tram.Rounds == 0) {
          Tram.Rounds = Tram.Rounds + 1;
          return 0;
        } else if (DateTime > Tram.Start.AddMinutes(Tram.Schedule)) {
          double diff = (Tram.Start.AddMinutes(Tram.Schedule) - DateTime).TotalSeconds;
          Console.WriteLine("{0} : Schdule tram {1,-2} at {2,-2} is to late: {3}",
              DateTime, Tram.Number, Station.Number, diff);

          Tram.Start = Tram.Start.AddMinutes(Tram.Schedule);
          Tram.Rounds = Tram.Rounds + 1;
          return diff;
        } else {
          double diff = (Tram.Start.AddMinutes(Tram.Schedule) - DateTime).TotalSeconds;
          Console.WriteLine("{0} : Schdule tram {1,-2} at {2,-2} is on time: {3}",
              DateTime, Tram.Number, Station.Number, diff);

          Tram.Start = Tram.Start.AddMinutes(Tram.Schedule);
          Tram.Rounds = Tram.Rounds + 1;
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
