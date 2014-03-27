using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Convertable;

namespace SPMReader.Models.Spektrum.DX18
{
  public partial class SpektrumModel : IConvertableModel
  {
    #region IConvertableModel Members

    public string ModelName
    {
      get { return this.Spektrum.Name; }
    }

    public string Version
    {
      get { return this.Spektrum.VCode; }
    }

    #endregion
  }
}
