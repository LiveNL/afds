using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class AddTram {
    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int StationCheckEventType = 2;
    public int AddTramEventType      = 3;

    // Config
    public int Schedule = 22;

    public AddTram(Event e, Station station, Tram tram) {
      DateTime = e.DateTime;
      Station = station;
      Tram = tram;
    }

    public List<Event> ScheduleNewTram(Event e, List<Event> events, Uithoflijn uithoflijn) {
      Station firstStation = uithoflijn.Stations[0];
      StationCheck stationCheck = new StationCheck(e, firstStation, Tram);

      if (firstStation.Tram == null) {
        Tram.Start = DateTime;
        Tram.Schedule = Schedule;
        return stationCheck.ScheduleArrival(e, events);
      } else {
        return stationCheck.ScheduleStationCheck(events, firstStation);
      }
    }

    public List<Event> ScheduleAddTramEvent(List<Event> events, Uithoflijn uithoflijn, int s) {
      Tram prevTram = Tram.PrevTram(uithoflijn.Trams);

      if (prevTram.Station?.Number == 666) {
        events.Add(new Event(DateTime.AddSeconds(s), AddTramEventType, prevTram, Station));
      }

      return events;
    }
  }
}
