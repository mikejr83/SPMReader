using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace SPMReader.Readers
{
  public abstract class Reader
  {
    static Logger _Logger = LogManager.GetCurrentClassLogger ();

    public abstract string ModelName { get; }

    public bool IsRead { get; private set; }

    protected string FileContents { get; private set; }

    public void Read ()
    {
      try {
        this.ReadContents ();
      } catch (Exception e) {
        _Logger.FatalException ("Failure during read.", e);
        throw;
      }
      this.IsRead = true;
    }

    protected abstract void ReadContents ();

    public Reader (string fileContents)
    {
      this.FileContents = fileContents;
    }
  }
}
