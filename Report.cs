using System;

namespace afds {
  public class Report {
    public void Print(Uithoflijn uithoflijn, State state) {
      Console.WriteLine("REPORT:");
      foreach (Tram tram in uithoflijn.Trams) {
        // Console.WriteLine("{0}: {1}", tram.Number, tram.Passengers);
      }
    }
  }
}
