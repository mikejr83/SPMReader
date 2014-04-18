using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Readers;
using SPMReader.Readers.Spektrum;

namespace SPMReader
{
  public class SPMReaderFactory
  {
    public static Reader CreateReader(string filename)
    {
      if (!File.Exists(filename))
        throw new FileNotFoundException("Cannot find SPM file!", filename);

      List<string> lines = File.ReadLines(filename).ToList ();

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
      string generator = generatorLine.Substring(11, generatorLine.Length - 12);
      switch (generator)
      {
        case "DX18":
          reader = new DX18(generator, string.Join(Environment.NewLine, lines.ToArray()));
          break;

        case "DX8":
          reader = new DX8(generator, string.Join(Environment.NewLine, lines.ToArray()));
          break;
      }

      return reader;
    }
  }
}
