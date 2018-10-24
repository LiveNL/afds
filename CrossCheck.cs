using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class CrossCheck {
    // Config
    public int OpenCrossAfterS      = 60;
    public int nextCrossCheckAfterS = 2;

    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int ArrivalEventType     = 1;
    public int CrossCheckEventType  = 4;
    public int OpenCrossEventType   = 5;

    public CrossCheck (Event e, Station station, Tram tram) {
      DateTime = e.DateTime;
      Station = station;
      Tram = tram;
    }

    public bool CrossIsOpen(Uithoflijn uithoflijn) {
      if (Station.Number == 7  || Station.Number == 8) { return uithoflijn.Crosses[0].Open; }
      if (Station.Number == 16 || Station.Number == 17){ return uithoflijn.Crosses[1].Open; }
      return false;
    }

    public List<Event> ScheduleCrossOpen(List<Event> events) {
      DateTime openCrossDT    = DateTime.AddSeconds(OpenCrossAfterS);
      Event    openCrossEvent = new Event(openCrossDT, OpenCrossEventType, Tram, Station);
      events.Add(openCrossEvent);
      return events;
    }

    public List<Event> ScheduleArrival(List<Event> events, Station station) {
      Event arrivalEvent = new Event(DateTime, ArrivalEventType, Tram, station);
      events.Add(arrivalEvent);
      return events;
    }

    public List<Event> ScheduleCrossCheck(List<Event> events, Station station) {
      DateTime nextCrossCheck = DateTime.AddSeconds(nextCrossCheckAfterS);
      Event    newCheckEvent  = new Event(nextCrossCheck, CrossCheckEventType, Tram, station);
      events.Add(newCheckEvent);
      return events;
    }
  }
}
