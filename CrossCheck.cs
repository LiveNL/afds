using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class CrossCheck {
    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int ArrivalEventType = 1;

    public CrossCheck (Event e, Station station, Tram tram) {
      DateTime = e.DateTime;
      Station = station;
      Tram = tram;
    }

    public bool CrossIsOpen(Uithoflijn uithoflijn) {
      Cross cross;
      if (Station.Number == 8) {
        cross = uithoflijn.Crosses[0];
      } else {
        cross = uithoflijn.Crosses[1];
      }
      return cross.Open;
    }

    public List<Event> ScheduleCrossOpen(List<Event> events) {
      events.Add(new Event(DateTime.AddSeconds(60), 5, Tram));
      return events;
    }

    public List<Event> ScheduleArrival(Event e, List<Event> events) {
      events.Add(NewArrivalEvent(e));
      return events;
    }

    public bool EmptyStation(Uithoflijn uithoflijn) {
      return Station.NextStation(uithoflijn.Stations).Tram == null;
    }

    public List<Event> ScheduleStationCheck(List<Event> events, Uithoflijn uithoflijn) {
      DateTime newCheckTime  = DateTime.AddSeconds(5);
      Event    newCheckEvent = new Event(newCheckTime, 2, Tram);
      events.Add(newCheckEvent);
      return events;
    }

    public List<Event> ScheduleCrossCheck(List<Event> events) {
      events.Add(new Event(DateTime, 4, Tram));
      return events;
    }

    public Event NewArrivalEvent(Event e) {
      return new Event(e.DateTime, ArrivalEventType, Tram);
    }
  }
}
