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
      Passengers = Passengers + Station.Passengers;
    }

    public Tram NextTram(Tram[] trams) {
      int next = this.Number + 1;

      if (next >= trams.Length) { next = 0; };
      return trams[next];
    }
  }
}
