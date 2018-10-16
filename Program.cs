using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace afds {
  class Program {
    static void Main(string[] args) {
      Probabilities.InitProbabilities();
      Uithoflijn uithoflijn = new Uithoflijn();
      State state           = new State();
      List<Event> events    = new List<Event>();

      Tram firstTram = uithoflijn.Trams[0];
      firstTram.Station = uithoflijn.Stations[0];
      // This maybe needs to be an arrival event? How many people are then at the station?
      events.Add(new Event(DateTime.Parse("7:00:00 AM"), 1, firstTram));

      bool endCondition = false;
      while (endCondition == false) {
        events = events.OrderBy(e => e.DateTime).ToList();
        Event nextEvent = timingRoutine(uithoflijn, state, events);

        events = eventRoutine(uithoflijn, state, nextEvent, events);

        // Check if loop/simulation should be ended
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

    static List<Event> eventRoutine(Uithoflijn uithoflijn, State state, Event next, List<Event> events) {
      // state.Update(uithoflijn);
      // update statistical counters?

      // generates future events
      return uithoflijn.Update(uithoflijn, next, events);
    }

    static void LogEvents(List<Event> events) {
      foreach (Event eventt in events) {
        Console.WriteLine("{2} - {0}: {1}",
            eventt.Tram.Number, eventt.Tram.Station.Number, eventt.GetHashCode());
      }
    }
  }
}
