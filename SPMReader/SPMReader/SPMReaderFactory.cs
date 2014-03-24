using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Readers;

namespace SPMReader
{
  public class SPMReaderFactory
  {
    public static Reader CreateReader(string filename)
    {
      if (!File.Exists(filename))
        throw new FileNotFoundException("Cannot find SPM file!", filename);

      IEnumerable<string> lines = File.ReadLines(filename);

      string generatorLine = null;
      foreach (string line in lines)
      {
        if (line.StartsWith("Generator", StringComparison.OrdinalIgnoreCase))
        {
          generatorLine = line;
          break;
        }
      }

      Reader reader = null;
      switch (generatorLine.Substring(11, generatorLine.Length - 12))
      {
        case "DX18":
          reader = new DX18(string.Join(Environment.NewLine, lines.ToArray()));
          break;
      }

      return reader;
    }
  }
}
