using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Uithoflijn {
    public Tram[] Trams { get; set; }
    public Station[] Stations { get; set; }

    public Uithoflijn() {
      Station[] stations = new Station[18]; // there are 9 stations, this represents both sides
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

    public void Update(Uithoflijn uithoflijn, Event eventt, List<Event> events) {
      Tram tram     = uithoflijn.Trams[eventt.TramNr];
      int eventType = eventt.EventType;

      switch (eventType) {
        case 0: // departure
          Console.WriteLine("{0} : Departure tram {1} at {2}",
              eventt.DateTime, tram.Number, tram.Station.Number);
          ScheduleArrival(eventt.DateTime, tram.Station, tram.Number, events);
          break;
        case 1: // arrival
          Console.WriteLine("{0} : Arrival tram {1} at {2}",
              eventt.DateTime, tram.Number, tram.Station.Number);

          tram.Station = tram.Station.NextStation(uithoflijn.Stations);
          ScheduleDeparture(eventt.DateTime, tram.Station, tram.Number, events);
          break;
      }
    }

    public void ScheduleArrival(DateTime cDT, Station cStation, int tramNr, List<Event> events) {
      DateTime time = cDT.AddSeconds(30); // seconds till next station (traveltime)
      events.Add(new Event(time, 1, tramNr));
      events.OrderBy(e => e.DateTime).ToList();
    }

    public void ScheduleDeparture(DateTime cDT, Station cStation, int tramNr, List<Event> events) {
      int dwellTime = CalculateDwellTime(cStation);
      DateTime time = cDT.AddSeconds(dwellTime); // seconds for dwelltime
      events.Add(new Event(time, 0, tramNr));
      events.OrderBy(e => e.DateTime).ToList();
    }

    public int CalculateDwellTime(Station station) {
      Console.WriteLine("{0} passengers at station: {1}", station.Passengers, station.Number);
      return 30;
    }
  }
}
