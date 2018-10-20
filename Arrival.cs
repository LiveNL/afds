using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Arrival {
    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int DepartureEventType = 0;
    public int Q                  = 300;

    public Arrival(Event e, Station station, Tram tram) {
      DateTime = e.DateTime;
      Station = station;
      Tram = tram;
    }

    public List<Event> ScheduleDeparture(List<Event> events) {
      events.Add(new Event(TimeAfterDwellTime(), DepartureEventType, Tram, Station));
      return events;
    }

    public DateTime TimeAfterDwellTime() {
      DateTime fstDwellTime = DateTime.AddSeconds(DwellTime());
      DateTime expectedDeparture = fstDwellTime.AddSeconds(sndDwellTime(fstDwellTime));
      return expectedDeparture;
    }

    public int DwellTime() {
      int dwellTime = Probabilities.CalcDwellingTime(Tram.PassengersIn(DateTime, 1),
                                                     Tram.PassengersOut(DateTime));
      // LogDwellTime(dwellTime);
      return dwellTime;
    }

    public int sndDwellTime(DateTime dt) {
      int dwellTime = Probabilities.CalcSecondDwellingTime(Tram.PassengersIn(dt, 2));
      int qTime = (int)extendedDwellTime(dt, dwellTime);
      // LogDwellTime(qTime);
      return dwellTime + qTime;
    }

    public double extendedDwellTime(DateTime fst, int snd) {
      int[] qStations = { 8, 9, 17, 0 };
      if (qStations.Contains(Station.Number)) {
        DateTime after = fst.AddSeconds(snd);
        double totalDwellTime = after.Subtract(DateTime).TotalSeconds;
        return Q - totalDwellTime;
      } else {
        return 0;
      }
    }

    public void LogDwellTime(int i) {
      Console.WriteLine("{0} : Dwelltm tram {2,-2} at {3,-2} : {1} sec",
        DateTime, i, Tram.Number, Station.Number);
    }
  }
}
