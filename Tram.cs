using System;

namespace afds {
  public class Tram {
    public int Number { get; set; }
    public int Schedule { get; set; }
    public int Passengers { get; set; }
    public Station Station { get; set; }

    public Tram(int i) {
      Number = i;
      Schedule = i;
      Passengers = i;
      Station = null;
      // Console.WriteLine("New Tram #{0} created!", i);
    }
  }
}
