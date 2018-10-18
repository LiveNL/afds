using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Uithoflijn {
    // There are 9 stations, with two sides, therefore 18 are created
    // Next to that there are 27 trams (atm), 13 double ones, 1 stand-by
    public int STATIONS = 18;
    public int TRAMS = 13;

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
      // LogTramPassengers(tram);

      switch (eventType) {
        case 0: // departure
          Departure departure = new Departure(e.DateTime, tram.Station, tram);
          departure.Station.LastDepartureEvent = e;
          departure.Station.Tram = null;
          events = departure.ScheduleNewTram(events, uithoflijn);
          return departure.ScheduleArrival(events, uithoflijn);
        case 1: // arrival
          Arrival arrival = new Arrival(e, tram.Station, tram);
          arrival.Station.LastArrivalEvent = e;
          arrival.Station.Tram = tram;
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
      Console.WriteLine("{0} : {1} tram {2,-2} at {3,-2} : {4}",
        e.DateTime, eventText, tram.Number, tram.Station.Number, tram.Station.StationDict()[tram.Station.Number]);
    }

    public void LogTramPassengers(Tram tram) {
      Console.WriteLine("tram {0} : {1} people", tram.Number, tram.Passengers);
    }
  }
}

// TODO: check if order of trams still is in place? Just for debugging reasons.
// if (tram.NextTram(uithoflijn.Trams).Station?.Number == tram.Station?.Number)  {
//   Console.WriteLine("GODVER");
//   foreach (Tram t in uithoflijn.Trams) {
//     Console.WriteLine("{0}: {1} | ", t.Number, t.Station?.Number);
//   }
// }
