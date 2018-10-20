using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Uithoflijn {
    // There are 9 stations, with two sides, therefore 18 are created
    // Next to that there are 27 trams (atm), 13 double ones, 1 stand-by
    public int STATIONS = 18;
    public int TRAMS    = 13;

    public Tram[] Trams { get; set; }
    public Station[] Stations { get; set; }
    public Cross[] Crosses { get; set; }

    public Uithoflijn() {
      Station[] stations = new Station[STATIONS];
      for (int i = 0; i < STATIONS; i++) { stations[i] = new Station(i); }
      Stations = stations;

      Tram[] trams = new Tram[TRAMS];
      Station depot = new Station(666);
      for (int i = 0; i < TRAMS; i++) { trams[i] = new Tram(i, depot); }
      Trams = trams;

      Cross[] crosses = new Cross[2];
      for (int i = 0; i < 2; i++) { crosses[i] = new Cross(i); }
      Crosses = crosses;

      Console.WriteLine("New UithofLijn created!");
    }

    public Uithoflijn (Tram[] trams, Station[] stations) {
      Stations = stations;
      Trams = trams;
    }

    public List<Event> Update(Uithoflijn uithoflijn, Event e, List<Event> events) {
      Tram tram     = uithoflijn.Trams[e.Tram.Number];
      int eventType = e.EventType;

      switch (eventType) {
        case 0: // departure
          Station departureStation = e.Station;
          LogEvent(e, tram, departureStation);
          Departure departure = new Departure(e.DateTime, departureStation, tram);

          if (departureStation.Number == 8 && (departure.CrossIsOpen(uithoflijn) == false)){
            return departure.ScheduleStationCheck(events, uithoflijn);
          }

          tram.LastStation                     = departureStation;
          tram.Station                         = null;
          departure.Station.LastDepartureEvent = e;
          departure.Station.Tram               = null;

          return departure.ScheduleStationCheck(events, uithoflijn);


        case 1: // arrival
          Station arrivalStation = e.Station;
          LogEvent(e, tram, arrivalStation);
          Arrival arrival = new Arrival(e, arrivalStation, tram);

          tram.Station                     = arrivalStation;
          tram.LastStation                 = arrivalStation;
          arrival.Station.LastArrivalEvent = e;
          arrival.Station.Tram             = tram;

          return arrival.ScheduleDeparture(events);


        case 2: // station check
          Station stationToCheck = e.Station;
          LogEvent(e, tram, stationToCheck);
          StationCheck stationCheck = new StationCheck(e, stationToCheck, tram);

          if (stationToCheck.Tram == null) {
            return stationCheck.ScheduleArrival(e, events);
          } else if (stationToCheck.Number == 8) { // and implicitly isn't empty
            return stationCheck.ScheduleCrossCheck(events);
          } else {
            return stationCheck.ScheduleStationCheck(events, uithoflijn);
          }


        case 3: // add tram
          LogEvent(e, tram, tram.Station);
          AddTram addTram = new AddTram(e, tram.Station, tram);

          if (e.DateTime < DateTime.Parse("7:00:00 AM")) {
            addTram.ScheduleNewTram(e, events, uithoflijn);
            addTram.ScheduleAddTramEvent(events, uithoflijn, 900);
          } else if (DateTime.Parse("7:00:00 AM") <= e.DateTime && e.DateTime <= DateTime.Parse("7:00:00 PM")) {
            addTram.ScheduleNewTram(e, events, uithoflijn);
            addTram.ScheduleAddTramEvent(events, uithoflijn, 180);
          } else {
            // Remove Trams till 9:30
          }
          return events;


        case 4: // cross check
          LogEvent(e, tram, tram.LastStation);
          CrossCheck crossCheck = new CrossCheck(e, tram.LastStation, tram);

          if (crossCheck.CrossIsOpen(uithoflijn)) {
            crossCheck.ScheduleArrival(events, uithoflijn.Stations[9]);
            uithoflijn.Crosses[0].Open = false; // for 1 minute
            return crossCheck.ScheduleCrossOpen(events);
          } else {
            return crossCheck.ScheduleStationCheck(events, uithoflijn);
          }


          case 5: // reopen cross
            LogEvent(e, tram, tram.LastStation);
            uithoflijn.Crosses[0].Open = true;
            return events;
      }
      return events;
    }

    public void LogEvent(Event e, Tram tram, Station station) {
      string eventText = "";
      switch(e.EventType) {
        case 0:
          eventText = "Dprture"; break;
        case 1:
          eventText = "Arrival"; break;
        case 2:
          eventText = "StCheck"; break;
        case 3:
          eventText = "NewTram"; break;
        case 4:
          eventText = "CrssChk"; break;
        case 5:
          eventText = "Opencrs"; break;
      }

      // Console.WriteLine("Event: {0}", e.EventType);
      // Console.WriteLine("TRAM: {0}", tram.Number);
      // Console.WriteLine("Station: {0}", station.Number);
      Console.WriteLine("{0} : {1} tram {2,-2} at {3,-2} : {4}",
        e.DateTime, eventText, tram.Number, station.Number, station.StationDict()[station.Number]);
    }
  }
}
