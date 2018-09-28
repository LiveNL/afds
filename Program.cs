using System;
using System.IO;
using System.Collections.Generic;

namespace afds {
  class Program {
    static void Main(string[] args) {
      Uithoflijn uithoflijn = new Uithoflijn();
      State state           = new State();
      List<Event> events    = new List<Event>();

      bool endCondition = false;
      while (endCondition == false) {
        timingRoutine(uithoflijn, state, events);
        eventRoutine(uithoflijn, state, events);

        if (state.SimulationClock > DateTime.Parse("7:00:00 PM")) {
          endCondition = true;
        };
      }

      (new Report()).Print(uithoflijn, state);
    }

    static void timingRoutine(Uithoflijn uithoflijn, State state, List<Event> events) {
      Console.WriteLine(state.SimulationClock);
    }

    static void eventRoutine(Uithoflijn uithoflijn, State state, List<Event> events) {
      uithoflijn.Update(uithoflijn, state);
      Console.WriteLine(state.SimulationClock);
    }
  }
}
