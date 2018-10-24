using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Station {
    public int   Number             { get; set; }
    public int   Waiting            { get; set; }
    public List<DateTime> WaitingList { get; set; }
    public Event LastDepartureEvent { get; set; }
    public Event LastArrivalEvent   { get; set; }
    public Tram  Tram               { get; set; }

    public Station(int i) {
      Number             = i;
      Waiting            = 0;
      LastDepartureEvent = null;
      LastArrivalEvent   = null;
      Tram               = null;
      WaitingList        = new List<DateTime>();
    }

    public Station NextStation(Station[] stations) {
      int next = Number + 1;
      if (next == 18 || Number == 666) { next = 0; }
      return stations[next];
    }

    public int WaitingPeople(Uithoflijn uithoflijn) {
      DateTime now              = LastArrivalEvent.DateTime;
      DateTime then             = LatestDeparture(uithoflijn);
      List<DateTime> passengers = Passengers(then, now);

      WaitingList.AddRange(passengers);
      Waiting = Waiting + passengers.Count;
      return Waiting;
    }

    DateTime LatestDeparture(Uithoflijn uithoflijn) {
      if (!( new int[] {8, 9, 17, 0}.Contains(Number))) {
        if (LastDepartureEvent != null)
          return LastDepartureEvent.DateTime;
        else return DateTime.Parse("6:00:00 AM");
      }
      else {
        if (Number == 8 || Number == 9) {
          if (uithoflijn.Stations[8].LastDepartureEvent != null && uithoflijn.Stations[9].LastDepartureEvent != null) {
            if (uithoflijn.Stations[8].LastDepartureEvent.DateTime < uithoflijn.Stations[9].LastDepartureEvent.DateTime)
              return uithoflijn.Stations[9].LastDepartureEvent.DateTime;
            else return uithoflijn.Stations[8].LastDepartureEvent.DateTime;
          }
          else if (uithoflijn.Stations[8].LastDepartureEvent != null) return uithoflijn.Stations[8].LastDepartureEvent.DateTime;
          else if (uithoflijn.Stations[9].LastDepartureEvent != null) return uithoflijn.Stations[9].LastDepartureEvent.DateTime;
          else return DateTime.Parse("6:00:00 AM");
        }
        else {
          if (uithoflijn.Stations[0].LastDepartureEvent != null && uithoflijn.Stations[17].LastDepartureEvent != null) {
            if (uithoflijn.Stations[0].LastDepartureEvent.DateTime < uithoflijn.Stations[17].LastDepartureEvent.DateTime)
              return uithoflijn.Stations[17].LastDepartureEvent.DateTime;
            else return uithoflijn.Stations[0].LastDepartureEvent.DateTime;
          }
          else if (uithoflijn.Stations[0].LastDepartureEvent != null) return uithoflijn.Stations[0].LastDepartureEvent.DateTime;
          else if (uithoflijn.Stations[17].LastDepartureEvent != null) return uithoflijn.Stations[17].LastDepartureEvent.DateTime;
          else return DateTime.Parse("6:00:00 AM");
        }
      }
    }

    public int WaitingPeople2(DateTime now) {
      DateTime then             = LastArrivalEvent.DateTime;
      List<DateTime> passengers = Passengers(then, now);

      WaitingList.AddRange(passengers);
      Waiting = Waiting + passengers.Count;
      return Waiting;
    }

    public List<DateTime> Passengers(DateTime then, DateTime now) {
      if (Number == 8) {
        return Probabilities.GeneratePassengerArrivals(then, now, Rates(9));
      } else if (Number == 17) {
        return Probabilities.GeneratePassengerArrivals(then, now, Rates(0));
      } else {
        return Probabilities.GeneratePassengerArrivals(then, now, Rates(Number));
      }
    }

    public double[] Rates(int nr) {
      string stationName = StationDict()[nr];
      if (nr < 9) {
        return Probabilities.Rates_a[stationName];
      } else {
        return Probabilities.Rates_b[stationName];
      }
    }

    public Dictionary<int, string> StationDict() {
      var map = new Dictionary<int, string>();
      map.Add(0, "P+R De Uithof");    map.Add(17, "P+R De Uithof");
      map.Add(1, "WKZ");              map.Add(16, "WKZ");
      map.Add(2, "UMC");              map.Add(15, "UMC");
      map.Add(3, "Heidelberglaan");   map.Add(14, "Heidelberglaan");
      map.Add(4, "Padualaan");        map.Add(13, "Padualaan");
      map.Add(5, "Kromme Rijn");      map.Add(12, "Kromme Rijn");
      map.Add(6, "Galgenwaard");      map.Add(11, "Galgenwaard");
      map.Add(7, "Vaartsche Rijn");   map.Add(10, "Vaartsche Rijn");
      map.Add(8, "Centraal Station"); map.Add(9,  "Centraal Station");

      map.Add(666, "Depot");
      return map;
    }

    public void LogWaitingPeople(int p, DateTime then) {
      Console.WriteLine(
        "Then: {0} | Now: {1} | passengers: {2,-4} | Station: {3,-20} | Tram: {4}",
        then.TimeOfDay, LastArrivalEvent.DateTime.TimeOfDay, p,
        StationDict()[Number], LastArrivalEvent.Tram.Number
      );
    }
  }
}
