using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SPMReader.Convertable
{
  public interface IConvertibleWriter
  {
    XDocument Convert(IConvertibleModel model);
  }
}
