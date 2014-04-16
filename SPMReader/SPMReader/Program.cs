using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Convertable;
using SPMReader.Readers;
using SPMReader.Writers;

namespace SPMReader
{
  class Program
  {
    const string USAGE = @"SPEKTRUM SPM File Reader
------------------------
Usage: SPMReader.exe <filename>
";

    static void Main(string[] args)
    {
      if (args == null || args.Length < 1)
        Console.WriteLine(USAGE);

      Reader reader = SPMReaderFactory.CreateReader(args[0]);

      reader.Read();

      /*
       * TODO: Need to not use the DX18 here.
       */

      object doc = ((SPMReader.Readers.Spektrum.DX18)(reader)).ExportXDocument();

      string output = null;
      if (doc != null)
        output = doc.ToString();

      FileInfo input = new FileInfo(args[0]);

      string outputFilename = args[0].ToLowerInvariant().Replace(".spm", ".xml");
      
      string temp = Path.GetTempFileName();

      File.WriteAllText(temp, output);
      File.Copy(temp, outputFilename, true);

      Writers.Spektrum.DX18 writer = new Writers.Spektrum.DX18(reader as Readers.Spektrum.DX18);
      string spm = writer.OutputToSPMFormat();

      ConversionWriter convertsion = new ConversionWriter(reader as IConvertableReader);
      convertsion.Write(writer as IConvertableWriter);
    }
  }
}
