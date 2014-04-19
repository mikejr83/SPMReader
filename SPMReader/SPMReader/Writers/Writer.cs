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

      string modelContents = this.CreateModelFile (serializedModel);

    }

    protected abstract string CreateModelFile (XDocument serializedModel);
  }
}

