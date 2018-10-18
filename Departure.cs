using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Departure {
    public DateTime DateTime { get; set; }
    public Station Station { get; set; }
    public Tram Tram { get; set; }
    public int ArrivalEventType = 1;

    public Departure(DateTime dt, Station station, Tram tram) {
      DateTime = dt;
      Station = station;
      Tram = tram;
    }

    public List<Event> ScheduleNewTram(List<Event> events, Uithoflijn uithoflijn) {
      Tram nextTram = Tram.NextTram(uithoflijn.Trams);

      if (Tram.Station.Number == 0 && nextTram.Station == null) {
        nextTram.Station = uithoflijn.Stations[0];
        // TODO: maybe set this on 0, such that it is an departure instead of arrival,
        // in this way it just makes it possible to gain passengers at the beginning
        events.Add(new Event(this.DateTime.AddSeconds(900), 1, nextTram));
      }

      return events;
    }

    public List<Event> ScheduleArrival(List<Event> events, Uithoflijn uithoflijn) {
      Tram prevTram = Tram.PrevTram(uithoflijn.Trams);

      if (prevTram.Station?.Number == Station.NextStation(uithoflijn.Stations).Number) {
        if (prevTram.ExpectedDeparture.AddSeconds(-TravelTime()) > DateTime.AddSeconds(40)) {
          events.Add(NewArrivalEvent(uithoflijn));
        } else {
          LogToCloseTram(uithoflijn);
        }
      } else {
        // NOTE: might be the case that tram is still to close,
        // even when the tram is past the next station, need to check this
        events.Add(NewArrivalEvent(uithoflijn));
      }
      return events;
    }

    public Event NewArrivalEvent(Uithoflijn uithoflijn) {
      Tram.Station = Tram.Station.NextStation(uithoflijn.Stations);
      return new Event(TimeAfterTravelTime(), ArrivalEventType, Tram);
    }

    public DateTime TimeAfterTravelTime() {
      return this.DateTime.AddSeconds(TravelTime());
    }

    public int TravelTime() {
      // TODO: check if this is always the same for each call within this class
      int travelTime;
      if (Station.Number < 9) {
        travelTime = Probabilities.CalcRunTime(Probabilities.Runtimes_b[Station.Number]);
      } else {
        travelTime = Probabilities.CalcRunTime(Probabilities.Runtimes_a[Station.Number - 9]);
      }

      // LogTravelTime(travelTime);
      return travelTime;
    }

    public void LogToCloseTram(Uithoflijn uithoflijn) {
      Tram prevTram = Tram.PrevTram(uithoflijn.Trams);
      Console.WriteLine("Tram: {0} is to close to prevTram {1}" +
                        "at prevTram.Station.Number {2} and Station.Number {3}",
        Tram.Number, prevTram.Number, prevTram.Station.Number, Station.Number);
    }

    public void LogTravelTime(int i) {
      Console.WriteLine("{0} : Travel {1} sec tram {2} at {3}",
        DateTime, i, Tram.Number, Station.Number);
    }
  }
}
