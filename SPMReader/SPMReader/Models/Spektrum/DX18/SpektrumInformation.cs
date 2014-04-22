using System;
using SPMReader.Convertable;

namespace SPMReader.Models.Spektrum.DX18
{
  public partial class SpektrumInformation : IConvertibleModelInformation
  {
    public string ModelName{ get { return this.Name; } }

    public string Version{ get { return this.VCode; } }
  }
}

