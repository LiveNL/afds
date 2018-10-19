using System;
using System.Collections.Generic;

namespace afds {
  public class Station {
    public int   Number             { get; set; }
    public int   Waiting            { get; set; }
    public Event LastDepartureEvent { get; set; }
    public Event LastArrivalEvent   { get; set; }
    public Tram  Tram               { get; set; }

    public Station(int i) {
      Number             = i;
      Waiting            = 0;
      LastDepartureEvent = null;
      LastArrivalEvent   = null;
      Tram               = null;
    }

    public Station NextStation(Station[] stations) {
      int next = Number + 1;
      if (next == stations.Length) {
        next = 0;
      } else if (Number == 666) {
        next = 0;
      }
      return stations[next];
    }

    public int WaitingPeople() {
      DateTime then;
      DateTime now = LastArrivalEvent.DateTime;
      DateTime timeIs715 = new DateTime(1, 1, 1, 7, 15, 0);
      bool laterThan715 = now.AddMinutes(-15).TimeOfDay > timeIs715.TimeOfDay;

      if (LastDepartureEvent != null)  {
        then = LastDepartureEvent.DateTime;
      } else if (laterThan715){
        then = now.AddMinutes(-15);
      } else { return 0; }

      int p = Probabilities.GeneratePassengerArrivals(then, now, Rates(Number));
      Waiting = Waiting + p;
      return Waiting;
    }

    public int WaitingPeople2(DateTime now) {
      DateTime then = LastArrivalEvent.DateTime;
      int p = Probabilities.GeneratePassengerArrivals(then, now, Rates(Number));
      Waiting = Waiting + p;
      return Waiting;
    }

    public double[] Rates(int nr) {
      string stationName = StationDict()[nr];
      if (nr < 9) {
        return Probabilities.Rates_a[stationName];
      } else {
        return Probabilities.Rates_b[stationName];
      }
    }

    public void LogWaitingPeople(int p, DateTime then) {
      Console.WriteLine(
        "Then: {0} | Now: {1} | passengers: {2,-4} | Station: {3,-20} | Tram: {4}",
        then.TimeOfDay, LastArrivalEvent.DateTime.TimeOfDay, p,
        StationDict()[Number], LastArrivalEvent.Tram.Number
      );
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
  }
}
