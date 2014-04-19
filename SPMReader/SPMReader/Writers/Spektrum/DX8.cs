using System;
using SPMReader.Convertable;
using SPMReader.Writers;
using SPMReader.Models.Spektrum.DX8;
using SPMReader.Helpers;
using System.Xml.Linq;

namespace SPMReader.Writers.Spektrum
{
  public class DX8 : Writer, IConvertibleWriter, ISPMWriter
  {

    public DX8 ()
    {
    }

    #region implemented abstract members of Writer

    protected override string CreateModelFile (XDocument serializedModel)
    {
      return null;
    }

    #endregion

    #region IConvertableWriter implementation

    public XDocument Convert (IConvertibleModel model)
    {
      SpektrumModel newDX8Model = new SpektrumModel ();
      newDX8Model.Spektrum.Name = model.ModelName;

      string serializedModelString  = SerializationHelper<SpektrumModel>.Serialize (newDX8Model);

      return XDocument.Parse (serializedModelString);
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

