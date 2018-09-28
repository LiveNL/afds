using System;

namespace afds {
  public class Uithoflijn {
    public Tram[] Trams { get; set; }
    public Station[] Stations { get; set; }

    // Initialize Uithoflijn
    public Uithoflijn() {
      // Create 9 stations
      Station[] stations = new Station[9];
      for (int i = 0; i <= 8; i++) {
        stations[i] = new Station(i);
      }

      Stations = stations;

      // Create 13 trams
      Tram[] trams = new Tram[13];
      for (int i = 0; i <= 12; i++) {
        trams[i] = new Tram(i);
      }

      Trams = trams;
      Console.WriteLine("New UithofLijn created!");
    }

    public Uithoflijn (Tram[] trams, Station[] stations) {
      Stations = stations;
      Trams = trams;
    }

    public void Update(int station, DateTime arr, DateTime dep, State state) {
      state.ProgramClock = arr;
    }
  }
}
