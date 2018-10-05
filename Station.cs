using System;

namespace afds {
  public class Station {
    public int Number { get; set; }
    public int Passengers { get; set; }

    public Station(int i) {
      Number = i;
      Passengers = i;
    }

    public Station NextStation(Station[] stations) {
      int next = this.Number + 1;

      if (next >= stations.Length) { next = 0; };
      return stations[next];
    }
  }
}
