using System;

namespace afds {
  public class Tram {
    public int Number { get; set; }
    public int Schedule { get; set; }
    public int Passengers { get; set; }
    public Station Station { get; set; }

    public Tram(int i, Station station) {
      Number = i;
      Schedule = i;
      Passengers = 0;
      Station = station;
    }

    public void InAndOut() {
      int current = Passengers; // Needs to change to some that get out
      int extra   = Station.Passengers;
      Passengers = current + extra; // Needs to be capped at 420
    }

    public Tram NextTram(Tram[] trams) {
      int next = this.Number + 1;

      if (next >= trams.Length) { next = 0; };
      return trams[next];
    }
  }
}
