using System;
using System.IO;

namespace afds {
  class Program {
    static void Main(string[] args) {
      Uithoflijn uithoflijn = new Uithoflijn();
      State state           = new State();

      bool endCondition = false;
      while (endCondition == false) {
        timingRoutine(uithoflijn, state);
        eventRoutine(uithoflijn, state);

        if (state.SimulationClock > DateTime.Parse("7:00:00 PM")) {
          endCondition = true;
        };
      }

      (new Report()).Print(uithoflijn, state);
    }

    // TODO: Create dynamic eventlist
    static void timingRoutine(Uithoflijn uithoflijn, State state) {
      Console.WriteLine(state.SimulationClock);
    }

    static void eventRoutine(Uithoflijn uithoflijn, State state) {
      uithoflijn.Update(uithoflijn, state);
      Console.WriteLine(state.SimulationClock);
    }
  }
}
