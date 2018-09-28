using System;

namespace afds {
  public class Event {
    public DateTime DateTime { get; set; }
    public int EventType { get; set; }
    public Tram Tram { get; set; }

    public Event(DateTime dateTime, int eventType, Tram tram) {
      DateTime  = dateTime;
      EventType = eventType; // either 0 or 1 (departure/arrival)
      Tram      = tram;
    }
  }
}
