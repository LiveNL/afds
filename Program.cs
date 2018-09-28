using System;
using System.IO;

namespace afds {
  class Program {
    static void Main(string[] args) {
      // init uithoflijn
      Uithoflijn uithoflijn = new Uithoflijn();

      // init system state
      State state = new State();

      timingRoutine(uithoflijn, state);
    }

    // TODO: Create dynamic eventlist
    static void timingRoutine(Uithoflijn uithoflijn, State state) {
      using (StreamReader reader = new StreamReader("eventlist")) {
        string line;
        while ((line = reader.ReadLine()) != null) {
          // parse input
          string[] eventt = line.Split(',');
          int stationNumber  = Int32.Parse(eventt[0]);
          DateTime arrival   = DateTime.Parse(eventt[1]);
          DateTime departure = DateTime.Parse(eventt[2]);

          // TODO: maybe just handle arrival/departure as separate events?
          // Create new event for departure after arrival (with RNG/distribution)
          uithoflijn.Update(stationNumber, arrival, departure, state);

          Console.WriteLine(state.ProgramClock);

          // TODO: Check for EndCondition (end of day, 19:00)

          // TODO: Check new added events before using the next line as next event
        }

        // TODO: Invoke creation of report anyway if EOF is reached
        (new Report()).Print(uithoflijn, state);
      }
    }
  }
}
