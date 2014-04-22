using System;

namespace SPMReader.Models.Spektrum.DX18
{
  public partial class Configuration : IConvertibleConfiguration
  {
    public string TrimMode{ get { return this.trimMode; } }
  }
}

