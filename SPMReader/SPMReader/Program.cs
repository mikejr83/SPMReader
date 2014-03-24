using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Readers;

namespace SPMReader
{
  class Program
  {
    static void Main(string[] args)
    {
      Reader reader = SPMReaderFactory.CreateReader(args[0]);

      reader.Read();

      object doc = ((DX18)(reader)).ExportXDocument();

      string output = null;
      if (doc != null)
        output = doc.ToString();
    }
  }
}
