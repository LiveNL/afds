using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  class Program {
    static void Main(string[] args) {
      Uithoflijn uithoflijn = new Uithoflijn();
      State state           = new State();
      List<Event> events    = new List<Event>();

      events.Add(new Event(DateTime.Parse("7:00:00 AM"), 0, 0));

      bool endCondition = false;
      while (endCondition == false) {
        Event nextEvent = timingRoutine(uithoflijn, state, events);
        eventRoutine(uithoflijn, state, nextEvent, events);

        if (!events.Any() || state.SimulationClock > DateTime.Parse("7:00:00 PM")) {
          endCondition = true;
        };
      }

      (new Report()).Print(uithoflijn, state);
    }

    static Event timingRoutine(Uithoflijn uithoflijn, State state, List<Event> events) {
      Event nextEvent = events[0];
      state.SimulationClock = nextEvent.DateTime;
      events.RemoveAt(0);
      return nextEvent;
    }

    static void eventRoutine(Uithoflijn uithoflijn, State state, Event next, List<Event> events) {
      // state.Update(uithoflijn);
      uithoflijn.Update(uithoflijn, next, events);
    }
  }
}
