using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Uithoflijn {
    // There are 9 stations, with two sides, therefore 18 are created
    // Next to that there are 27 trams (atm), 13 double ones, 1 stand-by
    public int STATIONS = 18;
    public int TRAMS = 1;

    public Tram[] Trams { get; set; }
    public Station[] Stations { get; set; }

    public Uithoflijn() {
      Station[] stations = new Station[STATIONS];
      for (int i = 0; i < STATIONS; i++) { stations[i] = new Station(i); }
      Stations = stations;

      Tram[] trams = new Tram[TRAMS];
      for (int i = 0; i < TRAMS; i++) { trams[i] = new Tram(i, null); }
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

      // TODO: check if order of trams still is in place? Just for debugging reasons.

      switch (eventType) {
        case 0: // departure
          // LogTramPassengers(tram);

          Departure departure = new Departure(e.DateTime, tram.Station, tram);
          departure.Station.LastDepartureEvent = e;

          // TODO: configure this based on schedule (7.00-9.00 : 15, after: max)
          events = departure.ScheduleNewTram(events, uithoflijn);
          return departure.ScheduleArrival(events, uithoflijn);
        case 1: // arrival
          Arrival arrival = new Arrival(e, tram.Station, tram);
          arrival.Station.LastArrivalEvent = e;

          // TODO: arrival.Tram.SecondInAndOut();
          return arrival.ScheduleDeparture(events);
      }
      return events;
    }

    public void LogEvent(Event e, Tram tram) {
      string eventText = "";
      switch(e.EventType) {
        case 0:
          eventText = "Depart"; break;
        case 1:
          eventText = "Arrivl"; break;
      }
      Console.WriteLine("{0} : {1} tram {2} at {3,-2} : {4}",
        e.DateTime, eventText, tram.Number, tram.Station.Number, tram.Station.StationDict()[tram.Station.Number]);
    }

    public void LogTramPassengers(Tram tram) {
      Console.WriteLine("tram {0} : {1} people", tram.Number, tram.Passengers);
    }
  }
}
