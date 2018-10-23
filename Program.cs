﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace afds {
  class Program {
    static void Main(string[] args) {
      Probabilities.InitProbabilities();
      
      int n = 100;
      for (int i = 0; i < n; i++) { 
        Statistics.InitStatistics();

        Uithoflijn uithoflijn = new Uithoflijn();
        State state           = new State();
        List<Event> events    = new List<Event>();

        Tram    firstTram  = uithoflijn.Trams[0];
        Station depot      = firstTram.Station;
        DateTime start     = DateTime.Parse("6:00:00 AM");
        firstTram.Start    = start;
        firstTram.Rounds   = 0;
        events.Add(new Event(start, 3, firstTram, depot));

        bool endCondition = false;
        while (endCondition == false) {
          events = events.OrderBy(e => e.DateTime).ToList();
          Event nextEvent = timingRoutine(uithoflijn, state, events);

          events = eventRoutine(uithoflijn, state, nextEvent, events);

          // Check if loop/simulation should be ended
          if (!events.Any() || state.SimulationClock > DateTime.Parse("9:30:00 PM")) {
          // if (!events.Any() || state.SimulationClock > DateTime.Parse("7:45:00 AM")) {
            endCondition = true;
          };
        }

        Statistics.UpdateAll(i);
      }

      Console.Write(Statistics.ResultsAll());
    }

    static Event timingRoutine(Uithoflijn uithoflijn, State state, List<Event> events) {
      Event nextEvent = events[0];
      state.SimulationClock = nextEvent.DateTime;
      events.RemoveAt(0);
      return nextEvent;
    }

    static List<Event> eventRoutine(Uithoflijn uithoflijn, State state, Event next, List<Event> events) {
      // TODO: state.Update(uithoflijn);

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
