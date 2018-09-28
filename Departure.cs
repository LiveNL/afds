using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Departure {
    public DateTime DateTime { get; set; }
    public Station Station { get; set; }
    public Tram Tram { get; set; }
    public int ArrivalEventType = 1;

    public Departure(DateTime dt, Station station, Tram tram) {
      DateTime = dt;
      Station = station;
      Tram = tram;
    }

    public void Schedule(List<Event> events, Uithoflijn uithoflijn) {
      events.Add(NewArrivalEvent(uithoflijn));
      events.OrderBy(e => e.DateTime).ToList();
    }

    public DateTime TimeAfterTravelTime() {
      return this.DateTime.AddSeconds(135);
    }

    public Event NewArrivalEvent(Uithoflijn uithoflijn) {
      Tram.Station = Tram.Station.NextStation(uithoflijn.Stations);
      return new Event(TimeAfterTravelTime(), ArrivalEventType, Tram);
    }
  }
}
