using System;

namespace afds {
  public class Report {
    public void Print(Uithoflijn uithoflijn, State state) {
      Console.WriteLine("REPORT:");
      foreach (Tram tram in uithoflijn.Trams) {
        Console.WriteLine("Tram NR: {0,-2} | Passengers {1,-4} | Station: {2,-2} | ", tram.Number, tram.Passengers, tram.LastStation.Number);
      }
    }
  }
}
