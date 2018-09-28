using System;

namespace afds {
  public class Station {
    public int Number { get; set; }
    public int Passengers { get; set; }

    public Station(int i) {
      Number = i;
      Passengers = i;
      Console.WriteLine("New Station #{0} created!", i);
    }
  }
}
