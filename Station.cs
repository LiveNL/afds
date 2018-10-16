using System;
using System.Collections.Generic;

namespace afds {
  public class Station {
    public int Number { get; set; }
    public int Passengers { get; set; }
    public Event LastDepartureEvent { get; set; }
    public Event LastArrivalEvent { get; set; }

    public Station(int i) {
      Number = i;
      Passengers = 10;
      LastDepartureEvent = null; // Initialize this at -15 min ? (to get people in at first)
      LastArrivalEvent = null;
    }

    public Station NextStation(Station[] stations) {
      int next = this.Number + 1;

      if (next >= stations.Length) { next = 0; };
      return stations[next];
    }

    public int WaitingPeople() {
      if (LastDepartureEvent != null)  {
        DateTime now = LastArrivalEvent.DateTime;
        DateTime then = LastDepartureEvent.DateTime;

        int p = Probabilities.GeneratePassengerArrivals(then, now, Rates(Number));
        LogWaitingPeople(p);
      }
      return 1;
    }

    public double[] Rates(int nr) {
      string stationName = StationDict()[nr];
      if (nr < 9) {
        return Probabilities.Rates_a[stationName];
      } else {
        return Probabilities.Rates_b[stationName];
      }
    }

    public void LogWaitingPeople(int p) {
      Console.WriteLine(
      "Then: {0} | Now: {1} | passengers: {2,-3} | Station: {3,-20} | PreTram: {4} - CurrTram: {5}",
      LastDepartureEvent.DateTime.TimeOfDay, LastArrivalEvent.DateTime.TimeOfDay, p, StationDict()[Number],
      LastDepartureEvent.Tram.Number, LastArrivalEvent.Tram.Number);
    }

    public Dictionary<int, string> StationDict() {
      // TODO: Just add this to the place where csvs are read
      var map = new Dictionary<int, string>();
      map.Add(0, "CS Centrumzijde");        map.Add(17, "CS Centrumzijde");
      map.Add(1, "Bleekstraat");            map.Add(16, "Bleekstraat");
      map.Add(2, "Sterrenwijk");            map.Add(15, "Sterrenwijk");
      map.Add(3, "Rubenslaan");             map.Add(14, "Rubenslaan");
      map.Add(4, "Stadion Galgenwaard");    map.Add(13, "Stadion Galgenwaard");
      map.Add(5, "De Kromme Rijn");         map.Add(12, "De Kromme Rijn");
      map.Add(6, "Padualaan");              map.Add(11, "Padualaan");
      map.Add(7, "Heidelberglaan");         map.Add(10, "Heidelberglaan");
      map.Add(8, "AZU");                    map.Add(9,  "AZU");
      return map;
    }
  }
}
