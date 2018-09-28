using System;

namespace afds {
  public class Event {
    public DateTime DateTime { get; set; }
    public int EventType { get; set; }
    public int TramNr { get; set; }

    public Event(DateTime dateTime, int eventType, int tramNr) {
      DateTime  = dateTime;
      EventType = eventType; // either 0 or 1 (departure/arrival)
      TramNr    = tramNr; // either 0 or 1 (departure/arrival)
    }
  }
}
