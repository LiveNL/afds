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
      Station depot = new Station(666);
      for (int i = 0; i < TRAMS; i++) { trams[i] = new Tram(i, depot); }
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
      // LogTramPassengers(tram);

      switch (eventType) {
        case 0: // departure
          Station departureStation = tram.Station;
          LogEvent(e, tram, departureStation);
          Departure departure = new Departure(e.DateTime, departureStation, tram);

          tram.LastStation = departureStation;
          tram.Station = null;

          departure.Station.LastDepartureEvent = e;
          departure.Station.Tram = null;

          events = departure.ScheduleNewTram(events, uithoflijn);
          return departure.ScheduleStationCheck(events, uithoflijn);

        case 1: // arrival
          Station arrivalStation = tram.LastStation.NextStation(uithoflijn.Stations);
          LogEvent(e, tram, arrivalStation);
          Arrival arrival = new Arrival(e, arrivalStation, tram);

          tram.Station = arrivalStation;
          tram.LastStation = arrivalStation;

          arrival.Station.LastArrivalEvent = e;
          arrival.Station.Tram = tram;

          return arrival.ScheduleDeparture(events);

        case 2: // station check
          Station stationToCheck = tram.LastStation.NextStation(uithoflijn.Stations);
          LogEvent(e, tram, stationToCheck);
          StationCheck stationCheck = new StationCheck(e, stationToCheck, tram);

          if (stationCheck.EmptyStation(uithoflijn)) {
            return stationCheck.ScheduleArrival(e, events);
          } else {
            return stationCheck.ScheduleStationCheck(events, uithoflijn);
          }
      }
      return events;
    }

    public void LogEvent(Event e, Tram tram, Station station) {
      string eventText = "";
      switch(e.EventType) {
        case 0:
          eventText = "Depart"; break;
        case 1:
          eventText = "Arrivl"; break;
        case 2:
          eventText = "SCheck"; break;
      }
      Console.WriteLine("{0} : {1} tram {2,-2} at {3,-2} : {4}",
        e.DateTime, eventText, tram.Number, station.Number, station.StationDict()[station.Number]);
    }

    public void LogTramPassengers(Tram tram) {
      Console.WriteLine("tram {0} : {1} people", tram.Number, tram.Passengers);
    }
  }
}
