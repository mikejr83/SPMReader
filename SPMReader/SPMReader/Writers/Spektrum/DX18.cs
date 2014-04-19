using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using SPMReader.Convertable;
using SPMReader.Models.Spektrum.DX18;
using SPMReader.Helpers;

namespace SPMReader.Writers.Spektrum
{
  public class DX18 : ISPMWriter, IConvertibleWriter
  {
    Readers.Spektrum.DX18 _Reader;

    internal DX18 (Readers.Spektrum.DX18 reader)
    {
      this._Reader = reader;
    }

    public string OutputToSPMFormat ()
    {
      StringBuilder spmBuilder = new StringBuilder ();

      XDocument data = this._Reader.ExportXDocument ();
      XNamespace ns = "urn:Spektrum/DX18.xsd";
      XElement modelDescription = data.Root.Element (ns + "ModelDescription");
      string newLineMarker = modelDescription.Attribute ("NewLineMarker").Value;

      foreach (XElement element in data.Root.Elements().Where(e => e.Name.LocalName != "ModelDescription")) {

        spmBuilder.AppendFormat ("<{0}>", element.Name.LocalName);
        spmBuilder.Append (newLineMarker);

        this.DoAttributes (spmBuilder, element, newLineMarker);

        spmBuilder.Append (newLineMarker);
        foreach (XElement subElement in element.Elements().Where(e => e.Name.LocalName != "AttributeDescriptors")) {
          spmBuilder.AppendFormat ("[{0}]", subElement.Name.LocalName);
          spmBuilder.Append (newLineMarker);

          this.DoAttributes (spmBuilder, subElement, newLineMarker);

          spmBuilder.AppendFormat ("[/{0}]", subElement.Name.LocalName);
          spmBuilder.Append (newLineMarker);
          spmBuilder.Append (newLineMarker);
        }

        spmBuilder.Remove (spmBuilder.Length - 1, 1);

        spmBuilder.AppendFormat ("</{0}>", element.Name.LocalName);
        spmBuilder.Append (newLineMarker);
        spmBuilder.Append (newLineMarker);
      }

      spmBuilder.AppendFormat ("{0}{1}{1}", modelDescription.Attribute ("EOFMarkerText").Value, newLineMarker);

      if (modelDescription.Element (ns + "PostfixModelText") != null) {
        byte[] strBytes = modelDescription.Element (ns + "PostfixModelText").Value.Split (new char[] { ' ' }).Select (strByte => byte.Parse (strByte)).ToArray ();

        spmBuilder.Append (UTF8Encoding.UTF8.GetString (strBytes));
      }

      return spmBuilder.ToString ();
    }

    void DoAttributes (StringBuilder spmBuilder, XElement element, string newLineMarker)
    {
      XNamespace ns = "urn:Spektrum/DX18.xsd";

      if (element.Element (ns + "AttributeDescriptors") == null)
        return;

      var query = from descriptor in element.Element (ns + "AttributeDescriptors").Elements ()
                  join attribute in element.Attributes ()
                  on descriptor.Attribute ("AttributeName").Value equals attribute.Name.LocalName
                  select new { Descriptor = descriptor, Attribute = attribute };

      foreach (var kvp in query.ToList()) {
        bool isString = bool.Parse (kvp.Descriptor.Attribute ("IsString").Value);
        bool valueHasPreceedingSpace = bool.Parse (kvp.Descriptor.Attribute ("ValueHasPreceedingSpace").Value);
        string prefixValue = string.Empty;

        if (kvp.Descriptor.Attribute ("PrefixValue") != null)
          prefixValue = kvp.Descriptor.Attribute ("PrefixValue").Value;

        string value = isString ? string.Format ("\"{0}\"", kvp.Attribute.Value) : kvp.Attribute.Value;

        value = valueHasPreceedingSpace ? " " + value : value;

        spmBuilder.AppendFormat ("{0}{1}{2}{3}{4}", prefixValue,
                                kvp.Descriptor.Attribute ("AttributeName").Value,
                                kvp.Descriptor.Attribute ("SeparatorStyle").Value,
                                value,
                                newLineMarker);
      }
    }
    #region IConvertableWriter Members
    public XDocument Convert (IConvertibleModel model)
    {
      SpektrumModel newModel = new SpektrumModel ();

      newModel.Spektrum.Name = model.ModelName;
      newModel.Spektrum.VCode = " 1.05";

      string serializedModelString = SerializationHelper<SpektrumModel>.Serialize (newModel);

      return XDocument.Parse (serializedModelString);//
    }
    #endregion
  }
}
