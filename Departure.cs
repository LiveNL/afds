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

    public Departure(DateTime dt, Station station, Tram tram) {
      DateTime = dt;
      Station  = station;
      Tram     = tram;
    }

    public List<Event> ScheduleStationCheck(List<Event> events, Uithoflijn uithoflijn) {
      DateTime dtAfterTt         = DateTime.AddSeconds(TravelTime());
      Station station            = StationToCheck(uithoflijn);
      Event newCheckStationEvent = new Event(dtAfterTt, StationCheckEventType, Tram, station);
      events.Add(newCheckStationEvent);
      return events;
    }

    public bool CrossIsOpen(Uithoflijn uithoflijn) {
      if (Station.Number == 8) {
        return uithoflijn.Crosses[0].Open;
      } else {
        return uithoflijn.Crosses[1].Open;
      }
    }

    public Station StationToCheck(Uithoflijn uithoflijn) {
      if (Station.Number == 8) {
        return uithoflijn.Stations[10];
      } else if (Station.Number == 17) {
        return uithoflijn.Stations[1];
      } else {
        return Tram.LastStation.NextStation(uithoflijn.Stations);
      }
    }

    public int TravelTime() {
      int travelTime = 0;

      if (Station.Number == 9) {
        travelTime = Probabilities.CalcRunTime(Probabilities.Runtimes_a[7]);
      } else if (Station.Number == 0) {
        travelTime = Probabilities.CalcRunTime(Probabilities.Runtimes_b[7]);
      } else if (Station.Number < 8) {
        travelTime = Probabilities.CalcRunTime(Probabilities.Runtimes_a[Station.Number]);
      } else if (Station.Number > 8) {
        travelTime = Probabilities.CalcRunTime(Probabilities.Runtimes_b[Station.Number - 10]);
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
