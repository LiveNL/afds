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

    public int PassengersIn(DateTime dt, int dwellTime) {
      int waiting;

      if (dwellTime == 1) {
        waiting = Station.WaitingPeople();
      } else {
        waiting = Station.WaitingPeople2(dt);
      }

      int space = 420 - Passengers;
      int newPassengers = 0;

      if (space <= 0) {
        newPassengers = 0;
      } else if (space >= waiting) {
        newPassengers = waiting;
      } else if (waiting > space) {
        newPassengers = space;
      }

      Station.Waiting = waiting - newPassengers;
      Passengers = Passengers + newPassengers;
      return newPassengers;
    }

    public int PassengersOut(DateTime dt) {
      string stationName = Station.StationDict()[Station.Number];
      int wantOut = Probabilities.CalcExit(dt, stationName, Direction(), Passengers);
      int p = Passengers;
      Passengers = Passengers - wantOut;
      // LogPassengersOut(dt, stationName, p, wantOut, Passengers);
      return wantOut;
    }

    public char Direction() {
      if (Station.Number < 9) {
        return 'a';
      } else {
        return 'b';
      }
    }

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
