using System;
using System.IO;

namespace afds {
    class Program {
      static void Main(string[] args) {

        // TODO: init State with 'time' and system clock (seconds)
        // Use this system clock as loop for the whole program

        // Use Event List as input (JSON?)
        readInput();

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

        Uithoflijn uithoflijn = new Uithoflijn();
        Console.WriteLine("New UithofLijn created!");

        uithoflijn.Start(trams, stations);
      }

      static void readInput() {
        using (StreamReader reader = new StreamReader("eventlist")) {
        // using (StreamReader sr = File.OpenText("eventlist")) {
          string line;
          while ((line = reader.ReadLine()) != null) {
            // execute step per event
          }
        }
      }
    }

    public class Tram {
      public int Number { get; set; }

      // Give each Tram a unique schedule that can be checked on runtime
      // (Int type to be changed)
      public int Schedule { get; set; }

      public Tram(int i) {
        Number = i;
        Schedule = i;
        Console.WriteLine("New Tram #{0} created!", i);
      }
    }

    public class Station {
      // Change this to the name of the station
      public int Number { get; set; }

      // Define the amount of passengers waiting on each station
      public int Passengers { get; set; }

      public Station(int i) {
        Number = i;
        Passengers = i;
        Console.WriteLine("New Station #{0} created!", i);
      }
    }

    public class Uithoflijn {
      public void Start(Tram[] trams, Station[] stations) {

      }
    }

    public class State {
      public void ProgramClock() {
      }

      public void Time() {
      }
    }

    // End Condition
}
