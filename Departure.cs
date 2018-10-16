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
      Tram nextTram = Tram.NextTram(uithoflijn.Trams);

      if (Tram.Station.Number == 0 && nextTram.Station == null) {
        nextTram.Station = uithoflijn.Stations[0];
        // TODO: maybe set this on 0, such that it is an departure instead of arrival,
        // in this way it just makes it possible to gain passengers at the beginning
        events.Add(new Event(this.DateTime.AddSeconds(900), 1, nextTram));
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
