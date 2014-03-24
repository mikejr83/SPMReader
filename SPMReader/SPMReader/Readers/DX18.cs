using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using SPMReader.Helpers;

namespace SPMReader.Readers
{
  public class DX18 : Reader
  {
    XDocument _ReadFile = null;
    XSD.DX18.SpektrumModel _Model = null;

    public DX18(string fileContents)
      : base(fileContents)
    {

    }

    protected override void ReadContents()
    {
      string[] lines = this.FileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

      Stack<string> xmlTypeSections = new Stack<string>();
      XElement currentNode = null;
      XDocument doc = new XDocument();
      XNamespace ns = "urn:Spektrum/DX18.xsd";
      doc.Add(new XElement(ns + "SPM"));
      currentNode = doc.Root;

      Regex xmlTagTypeFinder = new Regex("<.*>");
      Regex bracketXmlTagTypeFinder = new Regex(@"\[.*]");

      foreach (string line in lines)
      {
        if (line.Equals("*EOF*", StringComparison.OrdinalIgnoreCase))
          break;

        if (xmlTagTypeFinder.IsMatch(line) || bracketXmlTagTypeFinder.IsMatch(line))
        {
          if (line[1] == '/')
          {
            currentNode = currentNode.Parent;
            xmlTypeSections.Pop();
          }
          else
          {
            XElement element = new XElement(ns + line.Substring(1, line.Length - 2));
            currentNode.Add(element);
            currentNode = element;
            xmlTypeSections.Push(line);
          }
        }
        else
        {
          string theLine = line;

          theLine = theLine.Trim();
          if (theLine.StartsWith(";") || theLine.StartsWith("*"))
            theLine = theLine.Substring(1).Trim();

          string[] split = null;

          if (theLine.Contains('='))
            split = theLine.Split(new char[] { '=' });
          else
            split = theLine.Split(new char[] { ':' });

          string value = split[1].Replace("\"", string.Empty).Trim();

          XAttribute attr = new XAttribute(split[0].Trim(), value);
          currentNode.Add(attr);
        }
      }

      try
      {
        this._Model = SerializationHelper<XSD.DX18.SpektrumModel>.DeserializeFromParse(doc.ToString());
      }
      catch (Exception e)
      {
        Console.WriteLine("Failed to deserialize the DX18 XML to an object structure. " + e.ToString());
      }

      this._ReadFile = doc;
    }

    public XDocument ExportXDocument()
    {
      return new XDocument(this._ReadFile);
    }
  }
}
