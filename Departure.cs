using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Departure {
    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int StationCheckEventType = 2;
    public int DepartureEventType    = 0;
    public int Q                     = 300; // TODO: vary this Q

    public Departure(DateTime dt, Station station, Tram tram) {
      DateTime = dt;
      Station  = station;
      Tram     = tram;
    }

    public List<Event> ScheduleStationCheck(List<Event> events, Uithoflijn uithoflijn) {
      events.Add(NewCheckStationEvent(uithoflijn));
      return events;
    }

    public bool CrossIsOpen(Uithoflijn uithoflijn) {
      if (Station.Number == 8) {
        return uithoflijn.Crosses[0].Open;
      } else {
        return uithoflijn.Crosses[1].Open;
      }
    }

    public List<Event> ScheduleDeparture(List<Event> events) {
      events.Add(new Event(DateTime.AddSeconds(1), DepartureEventType, Tram));
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
      // LogTravelTime(travelTime);
      return travelTime;
    }

    public void LogTravelTime(int i) {
      Console.WriteLine("{0} : Travelt tram {2,-2} at {3,-2} : {1} sec",
        DateTime, i, Tram.Number, Station.Number);
    }
  }
}
