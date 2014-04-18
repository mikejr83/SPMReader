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
    const string WRITE_XML_FLAG = "-WriteXML";

    static readonly string[] CHECKS = new string[]{DEBUG_FLAG, WRITE_XML_FLAG};

    const string USAGE = @"SPEKTRUM SPM File Reader
------------------------
Usage: SPMReader.exe <filename>
";

    static void Main(string[] args)
    {
      if (args == null || args.Length < 1) {
        Console.WriteLine (USAGE);
        return;
      } else if (args.Length == 1) {
        foreach (string argTest in CHECKS) {
          if(args[0].Contains(argTest)){
            string newArgPos0 = args [0].Replace (argTest, string.Empty);
            args [0] = newArgPos0.Trim();
            List<string> newArgs = new List<string> (args);
            if (argTest.StartsWith ("-"))
              newArgs.Insert (0, argTest);
            else
              newArgs.Add (argTest);
            args = newArgs.ToArray ();
          }
        }
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

      if (args.Contains(WRITE_XML_FLAG))
      {
        object doc = ((SPMReader.Readers.Spektrum.Spektrum)(reader)).ExportXDocument();

        string output = null;
        if (doc != null)
          output = doc.ToString();

        FileInfo input = new FileInfo(filename);

        string outputFilename = filename.Substring(0, filename.Length - 4) + ".xml";

        string temp = Path.GetTempFileName();

        File.WriteAllText(temp, output);
        File.Copy(temp, outputFilename, true); 
      }
    }
  }
}
