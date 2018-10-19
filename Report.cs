using System;

namespace afds {
  public class Report {
    public void Print(Uithoflijn uithoflijn, State state) {
      Console.WriteLine("REPORT:");
      foreach (Tram tram in uithoflijn.Trams) {
        Console.WriteLine("Tram NR: {0,-2} | Passengers {1,-4} | Station: {2,-2} | {3}",
          tram.Number, tram.Passengers, tram.LastStation.Number, tram.GetHashCode());
      }

      foreach (Station station in uithoflijn.Stations) {
        Console.WriteLine("Station NR: {0,-2} | Passengers {1,-4} | {2}",
          station.Number, station.Passengers, station.GetHashCode());
      }
    }
  }
}
