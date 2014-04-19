using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Convertable;

namespace SPMReader.Writers
{
  public class ConversionWriter
  {
    public IConvertableReader Reader { get; private set; }

    public ConversionWriter(IConvertableReader reader)
    {
      this.Reader = reader;
    }

    public void Write(IConvertableWriter writer)
    {
 
    }
  }
}
