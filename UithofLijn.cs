using System;

namespace afds {
  public class Uithoflijn {
    public Tram[] Trams { get; set; }
    public Station[] Stations { get; set; }

    public Uithoflijn() {
      Station[] stations = new Station[9];
      for (int i = 0; i <= 8; i++) { stations[i] = new Station(i); }
      Stations = stations;

      Tram[] trams = new Tram[13];
      for (int i = 0; i <= 12; i++) { trams[i] = new Tram(i); }
      Trams = trams;

      Console.WriteLine("New UithofLijn created!");
    }

    public Uithoflijn (Tram[] trams, Station[] stations) {
      Stations = stations;
      Trams = trams;
    }

    public void Update(Uithoflijn uithoflijn, State state) {
      state.SimulationClock = DateTime.Parse("7:00:01 PM");
    }
  }
}
