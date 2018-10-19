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
      int space = 420 - Passengers;
      int newPassengers = 0;

      if (space <= 0) {
        newPassengers = 0;
      } else if (space >= waiting) {
        newPassengers = waiting;
      } else if (waiting > space) {
        newPassengers = space;
      }

      Station.Passengers = waiting - newPassengers;
      Passengers = Passengers + newPassengers;
      return newPassengers;
    }

    public int PassengersOut(DateTime dt) {
      char dir;
      string stationName = Station.StationDict()[Station.Number];

      if (Station.Number < 9) {
        dir = 'a';
      } else {
        dir = 'b';
      }

      int wantOut = Probabilities.CalcExit(dt, stationName, dir, Passengers);
      int p = Passengers;
      Passengers = Passengers - wantOut;
      // LogPassengersOut(dt, stationName, p, wantOut, Passengers);
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

    public void LogPassengersOut(DateTime dt, string stationName, int p, int wantOut, int Passengers) {
      Console.WriteLine("{0} : tram {1,-2} at {2,-2} : {3, -20} - {4,-3} passengers : {5,-3} go out, {6,-3} left",
          dt, Number, Station.Number, stationName, p, wantOut, Passengers);
    }
  }
}
