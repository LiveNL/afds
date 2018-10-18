using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Departure {
    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int ArrivalEventType = 1;
    public int Q                = 180; // TODO: vary this Q
    public int Interval         = 180; // TODO: vary this Interval

    public Departure(DateTime dt, Station station, Tram tram) {
      DateTime = dt;
      Station = station;
      Tram = tram;
    }

    // TODO: verify if this is the way we want to add new trams to the track
    public List<Event> ScheduleNewTram(List<Event> events, Uithoflijn uithoflijn) {
      Tram nextTram = Tram.NextTram(uithoflijn.Trams);

      if (Tram.Station.Number == 0 && nextTram.Station == null) {
        nextTram.Station = uithoflijn.Stations[0];
        events.Add(new Event(DateTime.AddSeconds(Interval), 1, nextTram));
      }
      return events;
    }

    public List<Event> ScheduleArrival(List<Event> events, Uithoflijn uithoflijn) {
      Tram   prevTram   = Tram.PrevTram(uithoflijn.Trams);
      Event  newEvent   = NewArrivalEvent(uithoflijn);
      double travelTime = (newEvent.DateTime - DateTime).TotalSeconds;

      DateTime expectedArrival = DateTime.AddSeconds(travelTime);
      DateTime expectedArrivalplus40sec = expectedArrival.AddSeconds(40);
      int nextStation = Station.NextStation(uithoflijn.Stations).Number;

      if (Station.NextStation(uithoflijn.Stations).Tram == null) {
        events.Add(newEvent);
      } else if (prevTram.ExpectedDeparture < expectedArrivalplus40sec) {
        // TODO: FIX THIS CONDITIONAL
        events.Add(newEvent);
      }

      // else {
      //  double diff = (prevTram.ExpectedDeparture - expectedArrival).TotalSeconds;
      //  LogToCloseTram(travelTime, prevTram, expectedArrival, nextStation, diff);
      //  newEvent.DateTime = newEvent.DateTime.AddSeconds(diff + 1);
      //  events.Add(newEvent);
      // }

      // if (prevTram.Station?.Number == nextStation) {
      //   if (prevTram.ExpectedDeparture < expectedArrivalplus40sec) {
      //     events.Add(newEvent);
      //   } else {
      //     double diff = (prevTram.ExpectedDeparture - expectedArrival).TotalSeconds;
      //     LogToCloseTram(travelTime, prevTram, expectedArrival, nextStation, diff);
      //     newEvent.DateTime = newEvent.DateTime.AddSeconds(diff + 1);
      //     events.Add(newEvent);
      //   }
      // } else {
      //   // TODO: prevTram is past nextStation, but could still be to close
      //   events.Add(newEvent);
      // }
      return events;
    }

    public Event NewArrivalEvent(Uithoflijn uithoflijn) {
      Tram.Station = Tram.Station.NextStation(uithoflijn.Stations);
      DateTime timeAfterTravelTime = DateTime.AddSeconds(TravelTime());
      return new Event(timeAfterTravelTime, ArrivalEventType, Tram);
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
      return travelTime; // LogTravelTime(travelTime);
    }

    public void LogToCloseTram(double tt, Tram prevTram, DateTime expArr, int ns, double diff) {
      Console.WriteLine("TT:     {0}", tt);
      Console.WriteLine("Now:    {0}", DateTime);
      Console.WriteLine("ExpDep: {0} tram {1} at {2}",
        prevTram.ExpectedDeparture, prevTram.Number, prevTram.Station?.Number);
      Console.WriteLine("ExpArr: {0} tram {1} at {2}",
        expArr, Tram.Number, ns);
      Console.WriteLine("Diff:   {0}", diff);
    }

    public void LogTravelTime(int i) {
      Console.WriteLine("{0} : Travel {1} sec tram {2} at {3}",
        DateTime, i, Tram.Number, Station.Number);
    }
  }
}
