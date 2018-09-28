using System;

namespace afds {
  // Station Class, one object for every station available
  public class Station {
    // TODO: Change this to the name of the station
    public int Number { get; set; }

    // Define the amount of passengers waiting on each station
    public int Passengers { get; set; }

    public Station(int i) {
      Number = i;

      // TODO: update Passengers according to input file/distribution
      Passengers = i;
      Console.WriteLine("New Station #{0} created!", i);
    }
  }
}
