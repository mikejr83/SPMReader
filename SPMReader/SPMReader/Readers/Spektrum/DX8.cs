using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPMReader.Convertable;

namespace SPMReader.Readers.Spektrum
{
  public class DX8 : Reader, IConvertableReader
  {
    public override string ModelName
    {
      get { return "Spektrum DX8"; }
    }

    public DX8(string fileContents)
      : base(fileContents)
    {

    }

    #region IConvertableReader Members

    public IConvertableModel LoadConvertableModel()
    {
      throw new NotImplementedException();
    }

    #endregion

    protected override void ReadContents()
    {
      throw new NotImplementedException();
    }
  }
}
