using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Convertable;

namespace SPMReader.Models.Spektrum.DX18
{
  public partial class SpektrumModel : IConvertibleModel
  {
    #region IConvertibleModel implementation
    public IConvertibleModelInformation StandardizedModelInformation { get { return this.Spektrum; } }

    public IConvertibleTrainer StandardizedTrainer{ get { return this.Trainer; } }

    public IConvertibleConfiguration StandardizedConfiguration{ get { return this.Config; } }

    public IConvertibleFlightMode StandardizedFlightMode{ get { return this.FMode; } }
    #endregion
    public SpektrumModel ()
    {
      this.Spektrum = new SpektrumInformation ();
      this.Config = new Configuration ();
      this.FMode = new FlightMode ();
      this.Trainer = new Trainer ();
    }
  }
}
