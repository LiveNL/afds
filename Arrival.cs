using System;
using System.Collections.Generic;
using System.Linq;

namespace afds {
  public class Arrival {
    public DateTime DateTime { get; set; }
    public Station  Station  { get; set; }
    public Tram     Tram     { get; set; }

    public int DepartureEventType = 0;
    public int MinQ               = 180;
    public int[] QStations        = { 8, 9, 17, 0 };

    public Arrival(Event e, Station station, Tram tram) {
      DateTime = e.DateTime;
      Station = station;
      Tram = tram;
    }

    public List<Event> ScheduleDeparture(List<Event> events, Uithoflijn uithoflijn) {
      Event departureEvent = new Event(TimeAfterDwellTime(uithoflijn), DepartureEventType, Tram, Station);
      events.Add(departureEvent);
      return events;
    }

    public DateTime TimeAfterDwellTime(Uithoflijn uithoflijn) {
      int fstDwellTime             = DwellTime(uithoflijn);
      DateTime dtAfterFstDwellTime = DateTime.AddSeconds(fstDwellTime);

      int sndDwellTime             = SndDwellTime(dtAfterFstDwellTime, uithoflijn);
      DateTime dtAfterSndDwellTime = dtAfterFstDwellTime.AddSeconds(sndDwellTime);

      double extraTime             = ExtraTime(dtAfterSndDwellTime);
      DateTime dtAfterExtraTime    = dtAfterSndDwellTime.AddSeconds(extraTime);

      if (QStations.Contains(Station.Number)) {
        if (dtAfterExtraTime > Tram.Start) {
          Statistics.MaxDelay(extraTime, DateTime);
          Statistics.Delay(extraTime, DateTime);
        }

        UpdateTramSchedule();
        UpdateStatistics();
      }

      return dtAfterExtraTime;
    }

    public int DwellTime(Uithoflijn uithoflijn) {
      int passOut   = Tram.PassengersOut(DateTime);
      int passIn    = Tram.PassengersIn(DateTime, 1, uithoflijn);
      int dwellTime = Probabilities.CalcDwellingTime(passIn, passOut);
      return dwellTime;
    }

    public int SndDwellTime(DateTime dt, Uithoflijn uithoflijn) {
      int passIn    = Tram.PassengersIn(dt, 2, uithoflijn);
      if (passIn == 0) return 0;
      int dwellTime = Probabilities.CalcSecondDwellingTime(passIn);
      return dwellTime;
    }

    public double ExtraTime(DateTime dtAfterSndDwellTime) {
      if (!QStations.Contains(Station.Number)) { return 0; }
      if (Tram.Rounds == 0) { return 0; }

      int dwellTimeSoFar = (int)dtAfterSndDwellTime.Subtract(DateTime).TotalSeconds;

      // Tram is on time and dwellTime is more than minimum q, so just wait till next Scheduled time.
      if (dtAfterSndDwellTime < Tram.Start && dwellTimeSoFar > MinQ) {
        return (Tram.Start - dtAfterSndDwellTime).TotalSeconds;
      }

      // Tram is before schedule but still needs to wait at least a few seconds to have the minimum Q.
      // Then it might still be on time, if the date time after the minimum wait is earlier than the schedule
      if (dtAfterSndDwellTime < Tram.Start && dwellTimeSoFar < MinQ) {
        int restMinQ         = MinQ - dwellTimeSoFar;
        DateTime dtAfterMinQ = dtAfterSndDwellTime.AddSeconds(restMinQ);

        // Still on time
        if (dtAfterMinQ < Tram.Start) {
          return (Tram.Start - dtAfterMinQ).TotalSeconds + restMinQ;
        } else {
          return (dtAfterMinQ - Tram.Start).TotalSeconds + restMinQ;
        }
      }

      // Tram is too late and dwellTime is more than minimum q, so just leave directly.
      if (dtAfterSndDwellTime > Tram.Start && dwellTimeSoFar > MinQ) {
        return 0;
      }

      // Tram is too late and dwellTime is less than minimum q, so wait for the rest of minQ and then leave.
      if (dtAfterSndDwellTime > Tram.Start && dwellTimeSoFar < MinQ) {
        return MinQ - dwellTimeSoFar;
      }

      return 0;
    }

    public void UpdateTramSchedule() {
      DateTime newScheduleStart = Tram.Start.AddMinutes(Tram.Schedule);
      Tram.Start = newScheduleStart;
    }

    public void UpdateStatistics() {
      Statistics.DelayChecks(1, DateTime);
      Tram.Rounds            = Tram.Rounds + 1;
    }

    public void LogDwellTime(int i) {
      Console.WriteLine("{0} : Dwelltm tram {2,-2} at {3,-2} : {1} sec",
        DateTime, i, Tram.Number, Station.Number);
    }
  }
}
