using System;

namespace afds {
  public class Tram {
    public int      Number            { get; set; }
    public int      Passengers        { get; set; }
    public Station  Station           { get; set; }
    public Station  LastStation       { get; set; }

    public Tram(int i, Station station) {
      Number = i;
      Passengers = 0;
      Station = station;
      LastStation = station;
    }

    // TODO: Passengers needs to be capped at 420
    public int PassengersIn() {
      int waiting = Station.WaitingPeople();
      Passengers = Passengers + waiting;
      return waiting;
    }

    public int PassengersOut() {
      // TODO: No clue yet
      int wantOut = 0;
      Passengers = Passengers - wantOut;
      return wantOut;
    }

    // TODO: make this prev/next based on location instead of nr
    public Tram PrevTram(Tram[] trams) {
      int prev = this.Number + 1;
      if (prev >= trams.Length) { prev = 0; };
      return trams[prev];
    }

    public Tram NextTram(Tram[] trams) {
      int next = this.Number - 1;
      if (next < 0) { next = trams.Length - 1; };
      return trams[next];
    }
  }
}
