using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Readers;

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

      object doc = ((DX18)(reader)).ExportXDocument();

      string output = null;
      if (doc != null)
        output = doc.ToString();

      FileInfo input = new FileInfo(args[0]);

      string outputFilename = args[0].ToLowerInvariant().Replace(".spm", ".xml");
      
      string temp = Path.GetTempFileName();

      File.WriteAllText(temp, output);
      File.Copy(temp, outputFilename, true);

      Writers.DX18 writer = new Writers.DX18(reader as DX18);
      string spm = writer.OutputToSPMFormat();
    }
  }
}
