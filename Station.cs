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
      Passengers = 0;
      LastDepartureEvent = null;
      LastArrivalEvent = null;
    }

    public Station NextStation(Station[] stations) {
      int next = this.Number + 1;

      if (next >= stations.Length) { next = 0; };
      return stations[next];
    }

    public int WaitingPeople() {
      DateTime then;
      DateTime now = LastArrivalEvent.DateTime;
      bool laterThan715 = now.AddMinutes(-15).TimeOfDay > new DateTime(1, 1, 1, 7, 15, 0).TimeOfDay;

      if (LastDepartureEvent != null)  {
        then = LastDepartureEvent.DateTime;

      } else if (laterThan715){
        // TODO: check if this is what we want,
        // provides passengers for the first time on a day, with a 15 min window.
        then = now.AddMinutes(-15);
      } else { return 0; }

      int p = Probabilities.GeneratePassengerArrivals(then, now, Rates(Number));
      LogWaitingPeople(p, then);
      return p;
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
        "Then: {0} | Now: {1} | passengers: {2,-3} | Station: {3,-20} | Tram: {4}",
        then.TimeOfDay, LastArrivalEvent.DateTime.TimeOfDay, p,
        StationDict()[Number], LastArrivalEvent.Tram.Number
      );
    }

    public Dictionary<int, string> StationDict() {
      // TODO: Just add this to the place where csvs are read
      // TODO: Correct order of stations (csv is different from logic order)
      var map = new Dictionary<int, string>();
      map.Add(0, "CS Centrumzijde");     map.Add(17, "CS Centrumzijde");
      map.Add(1, "Bleekstraat");         map.Add(16, "Bleekstraat");
      map.Add(2, "Sterrenwijk");         map.Add(15, "Sterrenwijk");
      map.Add(3, "Rubenslaan");          map.Add(14, "Rubenslaan");
      map.Add(4, "Stadion Galgenwaard"); map.Add(13, "Stadion Galgenwaard");
      map.Add(5, "De Kromme Rijn");      map.Add(12, "De Kromme Rijn");
      map.Add(6, "Padualaan");           map.Add(11, "Padualaan");
      map.Add(7, "Heidelberglaan");      map.Add(10, "Heidelberglaan");
      map.Add(8, "AZU");                 map.Add(9,  "AZU");
      return map;
    }
  }
}
