using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Uithoflijn {
    public Tram[] Trams { get; set; }
    public Station[] Stations { get; set; }

    public Uithoflijn() {
      // there are 9 stations, with two sides, therefore 18 are created
      Station[] stations = new Station[18];
      for (int i = 0; i < 18; i++) { stations[i] = new Station(i); }
      Stations = stations;

      Tram[] trams = new Tram[13];
      for (int i = 0; i <= 12; i++) { trams[i] = new Tram(i, stations[0]); }
      Trams = trams;

      Console.WriteLine("New UithofLijn created!");
    }

    public Uithoflijn (Tram[] trams, Station[] stations) {
      Stations = stations;
      Trams = trams;
    }

    public List<Event> Update(Uithoflijn uithoflijn, Event e, List<Event> events) {
      Tram tram     = uithoflijn.Trams[e.Tram.Number];
      int eventType = e.EventType;
      LogEvent(e, tram);

      switch (eventType) {
        case 0: // departure
          Departure departure = new Departure(e.DateTime, tram.Station, tram);
          events = departure.ScheduleNewDeparture(events, uithoflijn);
          return departure.ScheduleArrival(events, uithoflijn);
        case 1: // arrival
          Arrival arrival = new Arrival(e.DateTime, tram.Station, tram);
          return arrival.ScheduleDeparture(events);
      }
      return events;
    }

    public void LogEvent(Event e, Tram tram) {
      string eventText = "";
      switch(e.EventType) {
        case 0:
          eventText = "Departure";
          break;
        case 1:
          eventText = "Arrival";
          break;
      }
      Console.WriteLine("{0} : {1} tram {2} at {3}", e.DateTime, eventText, tram.Number, tram.Station.Number);
    }
  }
}
