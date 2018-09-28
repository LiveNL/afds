using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Arrival {
    public DateTime DateTime { get; set; }
    public Station Station { get; set; }
    public Tram Tram { get; set; }
    public int DepartureEventType = 0;

    public Arrival(DateTime dt, Station station, Tram tram) {
      DateTime = dt;
      Station = station;
      Tram = tram;
    }

    public void Schedule(List<Event> events) {
      events.Add(NewDepartureEvent());
      events.OrderBy(e => e.DateTime).ToList();
    }

    public DateTime TimeAfterDwellTime() {
      return this.DateTime.AddSeconds(240);
    }

    public Event NewDepartureEvent() {
      return new Event(TimeAfterDwellTime(), DepartureEventType, Tram);
    }
  }
}
