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

    public AddTram(DateTime dt, Station station, Tram tram) {
      DateTime = dt;
      Station = station;
      Tram = tram;
    }

    public List<Event> ScheduleNewTram(List<Event> events, Uithoflijn uithoflijn, int schedule) {
      Station firstStation = Station;
      StationCheck stationCheck = new StationCheck(DateTime, firstStation, Tram);

      Tram.Start = DateTime;
      Tram.Schedule = schedule;

      return stationCheck.ScheduleArrival(events);
    }

    public List<Event> ScheduleAddTramEvent(List<Event> events, Uithoflijn uithoflijn, int s) {
      Tram prevTram = Tram.PrevTram(uithoflijn.Trams);

      if (prevTram.Station?.Number == 666) {
        events.Add(new Event(DateTime.AddSeconds(s), AddTramEventType, prevTram, uithoflijn.Stations[0]));
      }

      return events;
    }
  }
}
