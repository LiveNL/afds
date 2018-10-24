using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace afds {
  class Program {

    // Config
    public static DateTime StartDateTime = DateTime.Parse("6:00:00 AM");
    public static DateTime EndDateTime   = DateTime.Parse("9:30:00 PM");
    public static int NumberOfRuns       = 100;

    static void Main(string[] args) {
      Probabilities.InitProbabilities();

      for (int i = 0; i < NumberOfRuns; i++) {
        Statistics.InitStatistics();

        Uithoflijn uithoflijn = new Uithoflijn();
        List<Event> events    = new List<Event>();

        Tram    firstTram  = uithoflijn.Trams[0];
        Station depot      = firstTram.Station;
        DateTime start     = StartDateTime;
        firstTram.Start    = start;
        firstTram.Rounds   = 0;
        events.Add(new Event(start, 3, firstTram, depot));

        bool endCondition = false;
        while (endCondition == false) {
          events          = events.OrderBy(e => e.DateTime).ToList();
          Event nextEvent = timingRoutine(uithoflijn, events);
          events          = eventRoutine(uithoflijn, nextEvent, events);

          // Check if End Condition is reached.
          if (!events.Any() || uithoflijn.SimulationClock > EndDateTime) {
            endCondition = true;
          };
        }

        Statistics.UpdateAll(i);
      }

      Console.Write(Statistics.ResultsAll());
    }

    static Event timingRoutine(Uithoflijn uithoflijn, List<Event> events) {
      Event nextEvent = events[0];
      uithoflijn.SimulationClock = nextEvent.DateTime;
      events.RemoveAt(0);
      return nextEvent;
    }

    static List<Event> eventRoutine(Uithoflijn uithoflijn, Event next, List<Event> events) {
      return uithoflijn.Update(uithoflijn, next, events);
    }
  }
}
