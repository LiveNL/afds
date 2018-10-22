using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Uithoflijn {

    // Config
    // There are 9 stations, with two sides, therefore 18 are created
    // Next to that there are 27 trams (atm), 13 double ones, 1 stand-by
    public int STATIONS           = 19; // 18 + 1 (depot)
    public int TRAMS              = 13;
    public int EarlyTramsInterval = 900;
    public int MaxFTramsInterval  = 300;
    public int EarlySchedule      = 30;
    public int MaxFSchedule       = 22;
    public DateTime LastRemoval   = DateTime.Parse("6:45:00 PM");

    public Tram[] Trams { get; set; }
    public Station[] Stations { get; set; }
    public Cross[] Crosses { get; set; }

    public Uithoflijn() {
      Station[] stations = new Station[STATIONS];
      for (int i = 0; i < STATIONS; i++) { stations[i] = new Station(i); }
      stations[18] = new Station(666);
      Stations = stations;

      Tram[] trams = new Tram[TRAMS];
      for (int i = 0; i < TRAMS; i++) { trams[i] = new Tram(i, stations[18]); }
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

          tram.LastStation                     = departureStation;
          tram.Station                         = null;
          departure.Station.LastDepartureEvent = e;
          departure.Station.Tram               = null;

          // ScheduleStationCheck checks after traveltime
          return departure.ScheduleStationCheck(events, uithoflijn);


        case 1: // arrival
          Station arrivalStation = e.Station;
          LogEvent(e, tram, arrivalStation);
          Arrival arrival = new Arrival(e, arrivalStation, tram);

          tram.Station                     = arrivalStation;
          tram.LastStation                 = arrivalStation;
          arrival.Station.LastArrivalEvent = e;
          arrival.Station.Tram             = tram;

          if (arrival.Station.Number == 666) { return events; }

          return arrival.ScheduleDeparture(events, uithoflijn);


        case 2: // station check
          Station stationToCheck = e.Station;
          LogEvent(e, tram, stationToCheck);
          StationCheck stationCheck = new StationCheck(e.DateTime, stationToCheck, tram);

          if (e.DateTime > DateTime.Parse("7:00:00 PM")) {
            tram.Schedule = EarlySchedule;
            bool ok = (LastRemoval == null || (e.DateTime - LastRemoval).TotalSeconds > 900);
            if ((stationToCheck.Number == 17 || stationToCheck.Number == 0) && (tramsInDepot(uithoflijn) < 9) && ok) {
              if (stationToCheck.Tram == null) {
                LastRemoval = e.DateTime;
                return stationCheck.ScheduleRemoveTram(events, uithoflijn);
              } else {
                return stationCheck.ScheduleStationCheck(events, stationToCheck);
              }
            }
          } else if (e.DateTime > DateTime.Parse("7:00:00 AM")) {
            tram.Schedule = MaxFSchedule;
          }

          int last = tram.LastStation.Number;
          int check = stationToCheck.Number;
          CrossCheck stationCrossCheck = new CrossCheck(e, stationToCheck, tram);

          bool A40SecondDistance;
          if (stationToCheck.LastDepartureEvent != null) {
             A40SecondDistance = (e.DateTime - stationToCheck.LastDepartureEvent.DateTime).TotalSeconds > 40;
          } else { A40SecondDistance = true; }

          if (last == 8 && check == 10) {
            return stationCrossCheck.ScheduleCrossCheck(events, stationToCheck);

          } else if (last == 17 && check == 1) {
            return stationCrossCheck.ScheduleCrossCheck(events, stationToCheck);

          } else if (last == 7 && check == 8 && stationToCheck.Tram != null) {
            return stationCrossCheck.ScheduleCrossCheck(events, stationToCheck.NextStation(uithoflijn.Stations));

          } else if (last == 16 && check == 17 && stationToCheck.Tram != null) {
            return stationCrossCheck.ScheduleCrossCheck(events, stationToCheck.NextStation(uithoflijn.Stations));

          } else if (stationToCheck.Tram == null && A40SecondDistance) {
            return stationCheck.ScheduleArrival(events);
          } else {
            return stationCheck.ScheduleStationCheck(events, e.Station);
          }

        case 3: // add tram
          LogEvent(e, tram, tram.Station);

          if (e.DateTime == DateTime.Parse("6:00:00 AM")) {
            AddTram addTram = new AddTram(e.DateTime, uithoflijn.Stations[0], uithoflijn.Trams[0]);
            addTram.ScheduleNewTram(events, uithoflijn, EarlySchedule);

            AddTram addTram1 = new AddTram(e.DateTime, uithoflijn.Stations[9], uithoflijn.Trams[1]);
            addTram1.ScheduleNewTram(events, uithoflijn, EarlySchedule);

            AddTram addTram2 = new AddTram(e.DateTime.AddMinutes(15), uithoflijn.Stations[0], uithoflijn.Trams[2]);
            addTram2.ScheduleNewTram(events, uithoflijn, EarlySchedule);

            AddTram addTram3 = new AddTram(e.DateTime.AddMinutes(15), uithoflijn.Stations[9], uithoflijn.Trams[3]);
            addTram3.ScheduleNewTram(events, uithoflijn, EarlySchedule);

            addTram3.ScheduleAddTramEvent(events, uithoflijn, 2700);

          } else if (DateTime.Parse("7:00:00 AM") <= e.DateTime && e.DateTime <= DateTime.Parse("7:00:00 PM")) {
            AddTram addTram = new AddTram(e.DateTime, uithoflijn.Stations[0], tram);
            addTram.ScheduleNewTram(events, uithoflijn, MaxFSchedule);
            addTram.ScheduleAddTramEvent(events, uithoflijn, MaxFTramsInterval);
          }
          return events;


        case 4: // cross check
          LogEvent(e, tram, tram.LastStation);
          CrossCheck crossCheck = new CrossCheck(e, tram.LastStation, tram);

          if (crossCheck.CrossIsOpen(uithoflijn) && tram.LastStation.Number == 7) {
            crossCheck.ScheduleArrival(events, uithoflijn.Stations[9]);
            uithoflijn.Crosses[0].Open = false; // for 1 minute
            return crossCheck.ScheduleCrossOpen(events);

          } else if (crossCheck.CrossIsOpen(uithoflijn) && tram.LastStation.Number == 16) {
            crossCheck.ScheduleArrival(events, uithoflijn.Stations[0]);
            uithoflijn.Crosses[1].Open = false; // for 1 minute
            return crossCheck.ScheduleCrossOpen(events);

          } else if (crossCheck.CrossIsOpen(uithoflijn) && tram.LastStation.Number == 8) {
            crossCheck.ScheduleArrival(events, uithoflijn.Stations[10]);
            uithoflijn.Crosses[0].Open = false; // for 1 minute
            return crossCheck.ScheduleCrossOpen(events);

          } else if (crossCheck.CrossIsOpen(uithoflijn) && tram.LastStation.Number == 17) {
            crossCheck.ScheduleArrival(events, uithoflijn.Stations[1]);
            uithoflijn.Crosses[1].Open = false; // for 1 minute
            return crossCheck.ScheduleCrossOpen(events);

          } else {
            // Cross is Closed
            StationCheck crossStationCheck = new StationCheck(e.DateTime, tram.LastStation.NextStation(uithoflijn.Stations), tram);
            return crossStationCheck.ScheduleStationCheck(events, tram.LastStation.NextStation(uithoflijn.Stations));
          }


          case 5: // reopen cross
            LogEvent(e, tram, e.Station);

            int[] cross0Stations = { 7, 8 };
            int[] cross1Stations = { 16, 17 };

            if (cross0Stations.Contains(e.Station.Number)) {
              uithoflijn.Crosses[0].Open = true;
            } else if (cross1Stations.Contains(e.Station.Number)) {
              uithoflijn.Crosses[1].Open = true;
            }
            return events;

      }
      return events;
    }

    public int tramsInDepot(Uithoflijn uithoflijn) {
      int depotTrams = 0;
      foreach (Tram tram in uithoflijn.Trams) {
        if (tram.Station?.Number == 666) {
          depotTrams++;
        }
      }
      return depotTrams;
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
