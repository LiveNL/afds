using System;
using System.IO;

namespace afds {
    class Program {
      static void Main(string[] args) {

        // initialize uithoflijn
        Uithoflijn uithoflijn = initialize();

        // init system state
        State state = new State();

        // progress on input
        // TODO: do we want to have some class that creates an eventlist based on tram schedules?
        readInput(uithoflijn, state);
      }

      static void readInput(Uithoflijn uithoflijn, State state) {
        using (StreamReader reader = new StreamReader("eventlist")) {
          string line;
          while ((line = reader.ReadLine()) != null) {
            // execute step per event
            // use uithoflijn and state
            string[] eventt = line.Split(',');
            int stationNumber  = Int32.Parse(eventt[0]);
            DateTime arrival   = DateTime.Parse(eventt[1]);
            DateTime departure = DateTime.Parse(eventt[2]);

            // Check for EndCondition
            // TODO: maybe just handle arrival/departure as separate events?
            uithoflijn.Update(stationNumber, arrival, departure);
          }
        }
      }

      static Uithoflijn initialize() {
        // Create 13 trams
        Tram[] trams = new Tram[13];
        for (int i = 0; i <= 12; i++) {
          trams[i] = new Tram(i);
        }

        // Create 9 stations
        Station[] stations = new Station[9];
        for (int i = 0; i <= 8; i++) {
          stations[i] = new Station(i);
        }

        Uithoflijn uithoflijn = new Uithoflijn(trams, stations);
        Console.WriteLine("New UithofLijn created!");
        return uithoflijn;
      }
    }

    // Tram Class, one object for every tram available
    public class Tram {
      public int Number { get; set; }

      // Give each Tram a unique schedule that can be checked on runtime
      // (Int type to be changed)
      // Maybe leave schedule out but just act on events?
      public int Schedule { get; set; }

      public int Passengers { get; set; }

      public Tram(int i) {
        Number = i;
        Schedule = i;
        Passengers = i;
        Console.WriteLine("New Tram #{0} created!", i);
      }
    }

    // Station Class, one object for every station available
    public class Station {
      // Change this to the name of the station
      public int Number { get; set; }

      // Define the amount of passengers waiting on each station
      public int Passengers { get; set; }

      public Station(int i) {
        Number = i;

        // update Passengers according to input file/distribution
        Passengers = i;
        Console.WriteLine("New Station #{0} created!", i);
      }
    }

    public class Uithoflijn {
      public Tram[] Trams { get; set; }
      public Station[] Stations { get; set; }

      public Uithoflijn (Tram[] trams, Station[] stations) {
        Stations = stations;
        Trams = trams;
      }

      public void Update(int station, DateTime arr, DateTime dep) {

      }
    }

    public class State {
      public void ProgramClock() {
        // update "Time" based on events
      }

      public DateTime Time() {
        return DateTime.Now;
      }
    }
}
