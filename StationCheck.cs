using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class StationCheck {
    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int ArrivalEventType = 1;

    public StationCheck(Event e, Station station, Tram tram) {
      DateTime = e.DateTime;
      Station = station;
      Tram = tram;
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

    public Event NewArrivalEvent(Event e) {
      return new Event(e.DateTime, ArrivalEventType, Tram);
    }
  }
}
