using System;

namespace SPMReader.Models.Spektrum.DX8
{
  public partial class SpektrumModel
  {
    public SpektrumModel ()
    {
      this.ModelDescription = new ModelDescription ();
      this.Spektrum = new SpektrumInformation ();
      this.Trainer = new Trainer ();
      this.Config = new Configuration ();
      this.FMode = new FlightMode ();
    }
  }
}

