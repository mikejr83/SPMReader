using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPMReader.Readers
{
  public abstract class Reader
  {
    protected string FileContents { get; private set; }

    public void Read()
    {
      this.ReadContents();
    }

    protected abstract void ReadContents();

    public Reader(string fileContents)
    {
      this.FileContents = fileContents;
    }
  }
}
