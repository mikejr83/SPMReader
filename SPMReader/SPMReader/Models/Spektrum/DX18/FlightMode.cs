using System;
using SPMReader.Convertable;

namespace SPMReader.Models.Spektrum.DX18
{
  public partial class FlightMode : IConvertibleFlightMode
  {
    public string SwitchA{ get { return this.switch_a; } }

    public string SwitchB{ get { return this.switch_b; } }

    public string SwitchC{ get { return this.switch_c; } }

    public string Size{ get { return this.size; } }
  }
}

