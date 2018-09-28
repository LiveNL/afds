using System;

namespace afds {
  public class State {
    public State() {
      SimulationClock = DateTime.Parse("7:00:00 AM");
    }

    public DateTime SimulationClock { get; set; }
  }
}
