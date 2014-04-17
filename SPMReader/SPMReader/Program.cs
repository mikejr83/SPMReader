using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Convertable;
using SPMReader.Helpers;
using SPMReader.Readers;
using SPMReader.Writers;

namespace SPMReader
{
  class Program
  {
    const string DEBUG_FLAG = "-Debug";

    const string USAGE = @"SPEKTRUM SPM File Reader
------------------------
Usage: SPMReader.exe <filename>
";

    static void Main(string[] args)
    {
      if (args == null || args.Length < 1)
      {
        Console.WriteLine(USAGE);
        return;
      }

      string filename = args.LastOrDefault();

      if (args.Contains(DEBUG_FLAG))
      {
        System.Diagnostics.Debugger.Launch();
      }


      Reader reader = SPMReaderFactory.CreateReader(filename);

      try
      {
        Console.WriteLine("Reading file for {0} model of radio.", reader.ModelName);
        reader.Read();
      }
      catch(NotImplementedException e)
      {
        Console.WriteLine("The radio model, {0}, is not supported at this time.", reader.ModelName);
        return;
      }

      if(reader == null)
      {
        Console.WriteLine("Unable to match the radio model to this file. Please check that you're submitting a valid file type.");
        return;
      }

      /*
       * TODO: Need to not use the DX18 here.
       */



      object doc = ((SPMReader.Readers.Spektrum.Spektrum)(reader)).ExportXDocument();

      string output = null;
      if (doc != null)
        output = doc.ToString();

      FileInfo input = new FileInfo(filename);

      string outputFilename = filename.ToLowerInvariant().Replace(".spm", ".xml");
      
      string temp = Path.GetTempFileName();

      File.WriteAllText(temp, output);
      File.Copy(temp, outputFilename, true);
    }
  }
}
