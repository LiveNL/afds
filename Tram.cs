using System;

namespace afds {
// Tram Class, one object for every tram available
  public class Tram {
    public int Number { get; set; }

    // TODO: Give each Tram a unique schedule that can be checked on runtime
    // NOTE: Maybe leave schedule out but just act on events?
    public int Schedule { get; set; }

    public int Passengers { get; set; }

    public Tram(int i) {
      Number = i;
      Schedule = i;
      Passengers = i;
      Console.WriteLine("New Tram #{0} created!", i);
    }
  }
}
