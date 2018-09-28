using System;

namespace afds {
  public class State {
    DateTime time;
    public DateTime ProgramClock {
      get { return this.time; }
      set { this.time = value; }
    }

    public DateTime Time() {
      return DateTime.Now;
    }
  }
}
