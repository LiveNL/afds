using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Departure {
    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int StationCheckEventType = 2;
    public int Q                     = 300; // TODO: vary this Q
    public int Interval              = 180; // TODO: vary this Interval

    public Departure(DateTime dt, Station station, Tram tram) {
      DateTime = dt;
      Station  = station;
      Tram     = tram;
    }

    public List<Event> ScheduleStationCheck(List<Event> events, Uithoflijn uithoflijn) {
      events.Add(NewCheckStationEvent(uithoflijn));
      return events;
    }

    public Event NewCheckStationEvent(Uithoflijn uithoflijn) {
      DateTime timeAfterTravelTime = DateTime.AddSeconds(TravelTime());
      return new Event(timeAfterTravelTime, StationCheckEventType, Tram);
    }

    public int TravelTime() {
      int travelTime;

      if (Station.Number < 8) {
        travelTime = Probabilities.CalcRunTime(Probabilities.Runtimes_a[Station.Number]);
      } else if (Station.Number == 8 || Station.Number == 17){
        travelTime = Q;
      } else {
        travelTime = Probabilities.CalcRunTime(Probabilities.Runtimes_b[Station.Number - 9]);
      }
      LogTravelTime(travelTime);
      return travelTime;
    }

    public List<Event> ScheduleNewTram(List<Event> events, Uithoflijn uithoflijn) {
      // TODO: maybe make this just new trams instead of next trams
      Tram prevTram = Tram.PrevTram(uithoflijn.Trams);

      if (Station.Number == 0 && prevTram.Station?.Number == 666) {
        prevTram.Station     = uithoflijn.Stations[0];
        prevTram.LastStation = uithoflijn.Stations[0];
        events.Add(new Event(DateTime.AddSeconds(Interval), 0, prevTram));
      }
      return events;
    }

    public void LogTravelTime(int i) {
      Console.WriteLine("{0} : Travel tram {2,-2} at {3,-2} : {1} sec",
        DateTime, i, Tram.Number, Station.Number);
    }
  }
}
