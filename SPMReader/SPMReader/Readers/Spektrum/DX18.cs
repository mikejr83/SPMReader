using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SPMReader.Helpers;
using SPMReader.Readers;
using SPMReader.Convertable;

namespace SPMReader.Readers.Spektrum
{
  public class DX18 : Spektrum, IConvertableReader
  {
    XDocument _ReadFile = null;
    Models.Spektrum.DX18.SpektrumModel _Model = null;

    public override string ModelName
    {
      get { return "Spektrum DX18"; }
    }

    public DX18(string generator, string fileContents)
      : base(generator, fileContents)
    {

    }

    protected override void ReadContents()
    {
      string[] lines = this.FileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

      XElement currentNode = null;
      XElement modelDescription = null;
      XDocument doc = this.CreateXDocument(out modelDescription);
      currentNode = doc.Root;

      bool foundEOF = false;
      string postfixText = null;

      foreach (string line in lines)
      {
        if (!foundEOF)
          foundEOF = this.DefaultHandleLine(line, ref currentNode);

        if (foundEOF)
        {
          postfixText += line;
          continue;
        }
      }

      if (!string.IsNullOrEmpty(postfixText))
      {
        XElement postfixModelText = new XElement(this.ModelDescNamespace + "PostfixModelText");
        postfixModelText.Value = String.Join(" ", UTF8Encoding.UTF8.GetBytes(postfixText));
        //postfixModelText.SetValue(postfixText);
        modelDescription.Add(postfixModelText);
      }

      try
      {
        this._Model = SerializationHelper<Models.Spektrum.DX18.SpektrumModel>.DeserializeFromParse(doc.ToString());
      }
      catch (Exception e)
      {
        Console.WriteLine("Failed to deserialize the DX18 XML to an object structure. " + e.ToString());
      }

      this._ReadFile = doc;
    }

    public override XDocument ExportXDocument()
    {
      return new XDocument(this._ReadFile);
    }

    #region IConvertableReader Members

    public IConvertableModel LoadConvertableModel()
    {
      return this._Model as IConvertableModel;
    }

    #endregion
  }
}
