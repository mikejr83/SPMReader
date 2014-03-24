using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMReader.Writers
{
  public abstract class Writer
  {
    public static Writer CreateWriter(SPMReader.Readers.Reader reader)
    {
      return null;
    }
  }
}
