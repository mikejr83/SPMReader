using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SPMReaderTester
{
  class MainClass
  {
    public static void Main(string[] args)
    {
      FileInfo assemblyFI = new FileInfo(typeof(MainClass).Assembly.Location);

      string spmFilesDirectoryName = Path.Combine(assemblyFI.DirectoryName, "SPMFiles");

      var spmFileslist = Directory.EnumerateFiles(spmFilesDirectoryName, "*", SearchOption.AllDirectories).ToList();

      foreach(string spmFilename in spmFileslist)
      {
        Console.WriteLine(string.Format("********** {0} **********", spmFilename));
        //ProcessStartInfo psi = new ProcessStartInfo("SPMReader.exe", string.Format("-Debug \"{0}\"", spmFilename));
        ProcessStartInfo psi = new ProcessStartInfo("SPMReader.exe", string.Format("\"{0}\"", spmFilename));
        psi.UseShellExecute = false;
        psi.RedirectStandardError = true;
        psi.RedirectStandardOutput = true;
        psi.WindowStyle = ProcessWindowStyle.Minimized;

        Process process = new Process();
        process.StartInfo = psi;
        process.ErrorDataReceived += process_ErrorDataReceived;
        process.OutputDataReceived += process_OutputDataReceived;

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();

        Console.WriteLine();
      }
    }

    static void process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
      Console.Error.WriteLine(e.Data);
    }

    static void process_OutputDataReceived(object sender, DataReceivedEventArgs e)
    {
      Console.WriteLine(e.Data);
    }
  }
}
