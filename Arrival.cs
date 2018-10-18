using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Arrival {
    public DateTime DateTime { get; set; }
    public Station Station { get; set; }
    public Tram Tram { get; set; }
    public int DepartureEventType = 0;

    public Arrival(Event e, Station station, Tram tram) {
      DateTime = e.DateTime;
      Station = station;
      Tram = tram;
    }

    public List<Event> ScheduleDeparture(List<Event> events) {
      events.Add(NewDepartureEvent());
      return events;
    }

    public Event NewDepartureEvent() {
      return new Event(TimeAfterDwellTime(), DepartureEventType, Tram);
    }

    public DateTime TimeAfterDwellTime() {
      // NOTE: do we need 'this'?
      DateTime expectedDeparture = this.DateTime.AddSeconds(DwellTime());
      Tram.ExpectedDeparture = expectedDeparture;
      return expectedDeparture;
    }

    public int DwellTime() {
      int dwellTime = Probabilities.CalcDwellingTime(Tram.PassengersIn(), Tram.PassengersOut());
      // LogDwellTime(dwellTime);
      return dwellTime;
    }

    public void LogDwellTime(int i) {
      Console.WriteLine("{0} : Dwelltime {1} sec tram {2} at {3}",
        DateTime, i, Tram.Number, Station.Number);
    }
  }
}
