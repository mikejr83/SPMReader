using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMReader.Convertable
{
  public interface IConvertibleModelInformation
  {
    string ModelName { get; }
    string Version { get; }
  }
}
