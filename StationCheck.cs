using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class StationCheck {
    // Config
    public int stationCheckAfterS = 10;

    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int ArrivalEventType      = 1;
    public int StationCheckEventType = 2;

    public StationCheck(DateTime dt, Station station, Tram tram) {
      DateTime = dt;
      Station = station;
      Tram = tram;
    }

    public List<Event> ScheduleArrival(List<Event> events) {
      events.Add(new Event(DateTime, ArrivalEventType, Tram, Station));
      return events;
    }

    public List<Event> ScheduleStationCheck(List<Event> events, Station station) {
      DateTime nextCheckTime  = DateTime.AddSeconds(stationCheckAfterS);
      Event    newCheckEvent  = new Event(nextCheckTime, StationCheckEventType, Tram, station);
      events.Add(newCheckEvent);
      return events;
    }

    public List<Event> ScheduleRemoveTram(List<Event> events, Uithoflijn uithoflijn) {
      events.Add(new Event(DateTime, ArrivalEventType, Tram, uithoflijn.Stations[18]));
      return events;
    }
  }
}
