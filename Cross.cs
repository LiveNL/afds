using System;

namespace afds {
  public class Cross {
    public int  Number { get; set; }
    public bool Open { get; set; }

    public Cross(int i) {
      Number = i;
      Open = true;
    }
  }
}
