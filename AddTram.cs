using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class AddTram {
    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int StationCheckEventType = 2;

    public AddTram(Event e, Station station, Tram tram) {
      DateTime = e.DateTime;
      Station = station;
      Tram = tram;
    }

    public List<Event> ScheduleNewTram(Event e, List<Event> events, Uithoflijn uithoflijn) {
      Station firstStation = uithoflijn.Stations[0];
      StationCheck stationCheck = new StationCheck(e, firstStation, Tram);

      if (stationCheck.EmptyNextStation(uithoflijn)) {
        Tram.Start = DateTime;
        Tram.Schedule = 20;
        Console.WriteLine("First DT: {0}, Schedule: {1}", Tram.Start, Tram.Schedule);
        return stationCheck.ScheduleArrival(e, events);
      } else {
        return stationCheck.ScheduleStationCheck(events, uithoflijn);
      }
    }

    public List<Event> ScheduleAddTramEvent(List<Event> events, Uithoflijn uithoflijn, int s) {
      Tram prevTram = Tram.PrevTram(uithoflijn.Trams);

      if (prevTram.Station?.Number == 666) {
        events.Add(new Event(DateTime.AddSeconds(s), 3, prevTram, Station));
      }

      return events;
    }
  }
}
