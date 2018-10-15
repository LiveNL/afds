using System;

namespace afds {
  public class Station {
    public int Number { get; set; }
    public int Passengers { get; set; }
    public Event LastDepartureEvent { get; set; }
    public Event LastArrivalEvent { get; set; }

    public Station(int i) {
      Number = i;
      Passengers = 10;
      LastDepartureEvent = null; // Initialize this at -15 min ? (to get people in at first)
      LastArrivalEvent = null;
    }

    public Station NextStation(Station[] stations) {
      int next = this.Number + 1;

      if (next >= stations.Length) { next = 0; };
      return stations[next];
    }

    public int WaitingPeople() {
      if (LastDepartureEvent != null)  {
        DateTime now = LastArrivalEvent.DateTime;
        int secs = (int)(now - LastDepartureEvent.DateTime).TotalSeconds;

        LogWaitingTime(now, secs);
      }
      return 1;
    }

    public void LogWaitingTime(DateTime now, int secs) {
      Console.WriteLine(
      "LastDepartureEvent.DT: {0}, Now: {1}, diff: {2}, Station: {3}, PreTram: {4}, CurrTram: {5}",
      LastDepartureEvent.DateTime.TimeOfDay, now.TimeOfDay, secs, Number,
      LastDepartureEvent.Tram.Number, LastArrivalEvent.Tram.Number);
    }
  }
}
