using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SPMReader.Writers
{
  public class DX18 : Writer
  {
    Readers.DX18 _Reader;

    internal DX18(Readers.DX18 reader)
    {
      this._Reader = reader;
    }

    //protected override string CreateContents()
    //{
    //  XDocument data = this._Reader.ExportXDocument();

    //  StringBuilder contentsBuilder = new StringBuilder();

    //  foreach(XElement element in data.Root.Elements())
    //  {
    //    contentsBuilder.AppendFormat("<{0}>", element.Name.LocalName);
    //    contentsBuilder.AppendLine();

    //    foreach(XAttribute attr in element.Attributes())
    //    {

    //    }

    //    contentsBuilder.AppendFormat("</{0}>", element.Name.LocalName);
    //    contentsBuilder.AppendLine();
    //    contentsBuilder.AppendLine();
    //  }

    //  return contentsBuilder.ToString();
    //}


  }
}
