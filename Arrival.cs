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

    public List<Event> ScheduleDeparture(List<Event> events) {
      events.Add(new Event(TimeAfterDwellTime(), DepartureEventType, Tram));
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
      // LogDwellTime(dwellTime);
      return dwellTime;
    }

    public void LogDwellTime(int i) {
      Console.WriteLine("{0} : Dwelltm tram {2,-2} at {3,-2} : {1} sec",
        DateTime, i, Tram.Number, Station.Number);
    }
  }
}
