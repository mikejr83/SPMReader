using System;
using SPMReader.Convertable;
using SPMReader.Writers;
using SPMReader.Models.Spektrum.DX8;
using SPMReader.Helpers;
using System.Xml.Linq;
using NLog;

namespace SPMReader.Writers.Spektrum
{
  public class DX8 : Writer, IConvertibleWriter, ISPMWriter
  {
    static Logger _Logger = LogManager.GetCurrentClassLogger();

    public DX8 ()
    {
    }

    #region implemented abstract members of Writer

    protected override string CreateModelFile (XDocument serializedModel)
    {
      _Logger.Info ("Creating Model File");

      return null;
    }

    #endregion

    #region IConvertableWriter implementation

    public XDocument Convert (IConvertibleModel model)
    {
      _Logger.Info ("Converting model to a Spektrum DX8 radio type.");
      SpektrumModel newDX8Model = new SpektrumModel ();

      _Logger.Debug ("Model: {0}", model.ModelName);
      newDX8Model.Spektrum.Name = model.ModelName;

      DateTime start = DateTime.Now;
      _Logger.Debug ("Starting serialization...");
      string serializedModelString  = SerializationHelper<SpektrumModel>.Serialize (newDX8Model);
      _Logger.Debug ("Serialization complete. It took: {0}", DateTime.Now - start);

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

