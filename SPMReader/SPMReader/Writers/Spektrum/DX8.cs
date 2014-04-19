using System;
using SPMReader.Convertable;
using SPMReader.Writers;

namespace SPMReader.Writers.Spektrum
{
  public class DX8 : IConvertableWriter, ISPMWriter
  {
    public DX8 ()
    {
    }

    #region IConvertableWriter implementation

    public string Convert (IConvertableModel model)
    {
      throw new NotImplementedException ();
    }

    #endregion

    #region ISPMWriter implementation

    public string OutputToSPMFormat ()
    {
      throw new NotImplementedException ();
    }

    #endregion
  }
}

