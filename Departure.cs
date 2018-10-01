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

    public List<Event> ScheduleNewDeparture(List<Event> events, Uithoflijn uithoflijn) {
      if (Tram.Station.Number == 0) {
        events.Add(new Event(this.DateTime.AddSeconds(900), 0, Tram.NextTram(uithoflijn.Trams)));
      }

      return events;
    }

    public List<Event> ScheduleArrival(List<Event> events, Uithoflijn uithoflijn) {
      events.Add(NewArrivalEvent(uithoflijn));
      return events;
    }

    public Event NewArrivalEvent(Uithoflijn uithoflijn) {
      Tram.Station = Tram.Station.NextStation(uithoflijn.Stations);
      return new Event(TimeAfterTravelTime(), ArrivalEventType, Tram);
    }

    public DateTime TimeAfterTravelTime() {
      return this.DateTime.AddSeconds(135);
    }
  }
}
