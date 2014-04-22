using System;
using SPMReader.Convertable;
using SPMReader.Writers;
using SPMReader.Models.Spektrum.DX8;
using SPMReader.Helpers;
using System.Xml.Linq;
using NLog;
using System.Xml;
using System.IO;

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

      _Logger.Debug ("Model: {0}", model.StandardizedModelInformation.ModelName);
      _Logger.Debug ("Converting the model information.");
      newDX8Model.Spektrum.Name = model.StandardizedModelInformation.ModelName;
      newDX8Model.Spektrum.VCode = model.StandardizedModelInformation.Version;

      _Logger.Debug ("Converting the configuration information.");
      newDX8Model.Config.FrameRate = model.StandardizedConfiguration.FrameRate;
      newDX8Model.Config.trimMode = model.StandardizedConfiguration.TrimMode;
      newDX8Model.Config.TrimType = model.StandardizedConfiguration.TrimType;

      _Logger.Debug ("Converting the trainer information.");
      newDX8Model.Trainer.Type = model.StandardizedTrainer.Type;

      _Logger.Debug ("Converting FMode.");
      newDX8Model.FMode.switch_a = model.StandardizedFlightMode.SwitchA;
      newDX8Model.FMode.switch_b = model.StandardizedFlightMode.SwitchB;
      newDX8Model.FMode.switch_c = model.StandardizedFlightMode.SwitchC;
      newDX8Model.FMode.size = model.StandardizedFlightMode.Size;

      DateTime start = DateTime.Now;
      _Logger.Debug ("Starting serialization...");
      string serializedModelString  = SerializationHelper<SpektrumModel>.Serialize (newDX8Model);
      _Logger.Debug ("Serialization complete. It took: {0}", DateTime.Now - start);

      StreamReader sReader = 
        new StreamReader (new MemoryStream (System.Text.UTF8Encoding.Default.GetBytes (serializedModelString)));

      XmlReader reader = XmlReader.Create (sReader);

      return XDocument.Load (reader);
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

