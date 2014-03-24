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
          }
          else
          {
            XElement element = new XElement(ns + line.Substring(1, line.Length - 2));
            currentNode.Add(element);
            currentNode = element;
          }
        }
        else
        {
          string theLine = line, attrPrefixValue = null, attrSeparatorStyle = null;
          bool isString = false, valueHasPreceedingSpace = false;

          theLine = theLine.Trim();
          if(theLine.StartsWith("; "))
          {
            attrPrefixValue = "; ";
            theLine = theLine.Substring(2).Trim();
          }
          else if (theLine.StartsWith("*"))
          {
            attrPrefixValue = "*";
            theLine = theLine.Substring(1).Trim();
          }
            

          string[] split = null;

          if (theLine.Contains('='))
          {
            attrSeparatorStyle = "=";
            split = theLine.Split(new char[] { '=' });
          }
          else
          {
            attrSeparatorStyle = ":";
            split = theLine.Split(new char[] { ':' });
          }

          isString = split[1].Contains("\"");
          valueHasPreceedingSpace = split[1].StartsWith(" ");

          string value = split[1].Replace("\"", string.Empty).Trim();

          XAttribute attr = new XAttribute(split[0].Trim(), value);
          currentNode.Add(attr);

          XElement dataDescElem = currentNode.Element(ns + "AttributeDescriptors");
          if(dataDescElem == null)
          {
            dataDescElem = new XElement(ns + "AttributeDescriptors");
            currentNode.Add(dataDescElem);
          }

          XElement descElem = new XElement(ns + "Descriptor");
          descElem.SetAttributeValue("PrefixValue", attrPrefixValue);
          descElem.SetAttributeValue("SeparatorStyle", attrSeparatorStyle);
          descElem.SetAttributeValue("IsString", isString);
          descElem.SetAttributeValue("AttributeName", split[0].Trim());
          descElem.SetAttributeValue("ValueHasPreceedingSpace", valueHasPreceedingSpace);

          dataDescElem.Add(descElem);
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
