using System;

namespace afds {
  public class Event {
    public DateTime DateTime  { get; set; }
    public int      EventType { get; set; }
    public Tram     Tram      { get; set; }
    public Station  Station   { get; set; }

    public Event(DateTime dateTime, int eventType, Tram tram, Station station) {
      DateTime  = dateTime;
      EventType = eventType;
      Tram      = tram;
      Station   = station;
    }
  }
}
