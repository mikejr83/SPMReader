using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SPMReader.Readers.Spektrum
{
  public abstract class Spektrum : Reader
  {
    XNamespace _Namespace = "urn:SPMReader/Spektrum/Spektrum.xsd";
    XNamespace _DescNamespace = "urn:SPMReader/ModelDescription.xsd";

    protected string Generator { get; private set; }

    protected XNamespace Namespace
    {
      get
      {
        return this._Namespace;
      }
    }

    protected XNamespace ModelDescNamespace
    {
      get
      {
        return this._DescNamespace;
      }
    }

    protected virtual Regex XMLTagTypeFinder
    {
      get
      {
        return new Regex("<.*>");
      }
    }

    protected virtual Regex BracketXmlTagTypeFinder
    {
      get
      {
        return new Regex(@"\[.*]");
      }
    }

    public Spektrum(string generator, string fileContents)
      : base(fileContents)
    {
      this.Generator = generator;
    }

    protected virtual XDocument CreateXDocument(out XElement modelDescription)
    {
      XDocument doc = new XDocument();
      doc.Add(new XElement(this.Namespace + "SPM",
        new XAttribute(XNamespace.Xmlns + "modelDesc", this.ModelDescNamespace)));

      modelDescription = new XElement(this.ModelDescNamespace + "ModelDescription");
      modelDescription.SetAttributeValue("EOFMarkerText", "*EOF*");
      modelDescription.SetAttributeValue("NewLineMarker", "\r");
      doc.Root.Add(modelDescription);

      return doc;
    }

    protected bool DefaultHandleLine(string line, ref XElement currentNode)
    {
      if (line.Equals("*EOF*", StringComparison.OrdinalIgnoreCase))
      {
        return true;
      }

      if (this.XMLTagTypeFinder.IsMatch(line) || this.BracketXmlTagTypeFinder.IsMatch(line))
      {
        if (line[1] == '/')
        {
          currentNode = currentNode.Parent;
        }
        else
        {
          XElement element = new XElement(this.Namespace + line.Substring(1, line.Length - 2));
          currentNode.Add(element);
          currentNode = element;
        }
      }
      else
      {
        string theLine = line, attrPrefixValue = null, attrSeparatorStyle = null;
        bool isString = false, valueHasPreceedingSpace = false;

        theLine = theLine.Trim();
        if (theLine.StartsWith("; "))
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

        string value = null;

        if (isString)
          value = split[1].Replace("\"", string.Empty);
        else
          value = split[1].Replace("\"", string.Empty).Trim();

        XAttribute attr = new XAttribute(split[0].Trim(), value);
        currentNode.Add(attr);

        XElement dataDescElem = currentNode.Element(this.ModelDescNamespace + "AttributeDescriptors");
        if (dataDescElem == null)
        {
          dataDescElem = new XElement(this.ModelDescNamespace + "AttributeDescriptors");
          currentNode.Add(dataDescElem);
        }

        XElement descElem = new XElement(this.ModelDescNamespace + "Descriptor");
        descElem.SetAttributeValue("PrefixValue", attrPrefixValue);
        descElem.SetAttributeValue("SeparatorStyle", attrSeparatorStyle);
        descElem.SetAttributeValue("IsString", isString);
        descElem.SetAttributeValue("AttributeName", split[0].Trim());
        descElem.SetAttributeValue("ValueHasPreceedingSpace", valueHasPreceedingSpace);

        dataDescElem.Add(descElem);
      }

      return false;
    }

    public abstract XDocument ExportXDocument();
  }
}
