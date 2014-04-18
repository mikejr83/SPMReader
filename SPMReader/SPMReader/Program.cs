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
using NLog;

namespace SPMReader
{
  class Program
  {
    const string DEBUG_FLAG = "-Debug";
    const string WRITE_XML_FLAG = "-WriteXML";
    const string USAGE = @"SPEKTRUM SPM File Reader
------------------------
Usage: SPMReader.exe <filename>
";
    static readonly string[] CHECKS = new string[] { DEBUG_FLAG, WRITE_XML_FLAG };
    static Logger _Logger = LogManager.GetCurrentClassLogger ();

    static void Main (string[] args)
    {
      LogManager.EnableLogging ();

      if (args == null || args.Length < 1) {
        _Logger.Info (USAGE);
        return;
      } else if (args.Length == 1) {
        foreach (string argTest in CHECKS) {
          if (args [0].Contains (argTest)) {
            string newArgPos0 = args [0].Replace (argTest, string.Empty);
            args [0] = newArgPos0.Trim ();
            List<string> newArgs = new List<string> (args);
            if (argTest.StartsWith ("-"))
              newArgs.Insert (0, argTest);
            else
              newArgs.Add (argTest);
            args = newArgs.ToArray ();
          }
        }
      }

      string filename = args.LastOrDefault ();

      if (args.Contains (DEBUG_FLAG)) {
        System.Diagnostics.Debugger.Launch ();
      }

      Reader reader = SPMReaderFactory.CreateReader (filename);

      try {
        _Logger.Info("Reading file for {0} model of radio.", reader.ModelName);
        reader.Read ();
      } catch (NotImplementedException) {
        string errorMsg = string.Format ("The radio model, {0}, is not supported at this time.", reader.ModelName);

        _Logger.Error (errorMsg);
        Console.Error.WriteLine (errorMsg);
        return;
      }

      if (reader == null) {
        string errorMsg = "Unable to match the radio model to this file. Please check that you're submitting a valid file type.";
        _Logger.Error (errorMsg);
        Console.Error.WriteLine (errorMsg);
        return;
      }

      if (args.Contains (WRITE_XML_FLAG)) {
        _Logger.Info ("Preparing XML document.");
        object doc = ((SPMReader.Readers.Spektrum.Spektrum)(reader)).ExportXDocument ();

        string output = null;
        if (doc != null)
          output = doc.ToString ();

        string outputFilename = filename.Substring (0, filename.Length - 4) + ".xml";

        string temp = Path.GetTempFileName ();

        File.WriteAllText (temp, output);
        _Logger.Info ("Ouptting XML document to: {0}", outputFilename);
        File.Copy (temp, outputFilename, true); 
      }

      _Logger.Info ("Done!");
      //LogManager.Shutdown ();
    }
  }
}
