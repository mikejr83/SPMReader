using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Convertable;

namespace SPMReader.Writers
{
  public class ConversionWriterFactory
  {
    public static IConvertibleWriter LoadConvertableWriter (string radioModelType)
    {
      IConvertibleWriter writer = null;

      switch (radioModelType) {
      case "DX8":
        writer = new Spektrum.DX8 ();
        break;
      default:
        break;
      }

      return writer;
    }
  }
}
