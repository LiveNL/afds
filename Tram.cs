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

    public int PassengersIn() {
      // Passengers needs to be capped at 420
      int waiting = Station.WaitingPeople();
      Passengers = Passengers + waiting;
      return waiting;
    }

    public int PassengersOut() {
      // Passengers needs to be capped at 420
      int wantOut = 0;
      Passengers = Passengers - wantOut;
      return wantOut; // TODO: No clue yet
    }

    public Tram NextTram(Tram[] trams) {
      int next = this.Number + 1;

      if (next >= trams.Length) { next = 0; };
      return trams[next];
    }
  }
}
