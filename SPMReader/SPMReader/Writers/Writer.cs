using System;
using System.Xml.Linq;
using NLog;

namespace SPMReader.Writers
{
  public abstract class Writer
  {
    static Logger _Logger = LogManager.GetCurrentClassLogger();

    public void WriteToModelFile (string filename, XDocument serializedModel)
    {
      _Logger.Info ("Writing model file to {0}.", filename);
      _Logger.Debug ("Serialized model for writing to output:{0}{1}", Environment.NewLine, serializedModel.ToString ());

      string modelContents = this.CreateModelFile (serializedModel);

      _Logger.Debug ("Model file contents to be written to disk:{0}{1}", Environment.NewLine, modelContents);
    }

    protected abstract string CreateModelFile (XDocument serializedModel);
  }
}

