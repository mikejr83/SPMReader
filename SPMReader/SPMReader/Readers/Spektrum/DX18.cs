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
using SPMReader.Models.Spektrum.DX18;
using NLog;

namespace SPMReader.Readers.Spektrum
{
  public class DX18 : Spektrum, IConvertableReader
  {
    public static SpektrumModel CreateTestModel()
    {
      SpektrumModel model = new SpektrumModel()
      {
        ModelDescription = new ModelDescription()
        {
          EOFMarkerText = "*EOF*",
          NewLineMarker = "&#xD;"
        },
        Spektrum = new SpektrumInformation()
        {
          Generator = "DX18",
          VCode = " 1.00",
          PosIndex = "5",
          Type = "Acro",
          curveIndex = "7",
          enabXPLUS = "Disabled",
          Name = "DHC-6 Twin Otter",
          AttributeDescriptors = new Descriptor[] 
          { 
            new Descriptor()
            {
              AttributeName = "Generator",
              IsString = true,
              SeparatorStyle="=",
              ValueHasPreceedingSpace=false
            },
            new Descriptor()
            {
              AttributeName = "VCode",
              IsString = true,
              SeparatorStyle="=",
              ValueHasPreceedingSpace=false
            },
            new Descriptor()
            {
              AttributeName = "PosIndex",
              IsString = false,
              SeparatorStyle="=",
              ValueHasPreceedingSpace=true
            },
            new Descriptor()
            {
              AttributeName = "Type",
              IsString = false,
              SeparatorStyle="=",
              ValueHasPreceedingSpace=false
            }
          }
        },
        Trainer = new Trainer()
        {
          Type = "Disabled",
          conditionID = "92",
          MOverride = "Disabled",
          activePositions = "0",
          AttributeDescriptors = new Descriptor[] 
          {

          }
        }
      };

      return model;
    }

    static Logger _Logger = LogManager.GetCurrentClassLogger ();

    XDocument _ReadFile = null;
    Models.Spektrum.DX18.SpektrumModel _Model = null;

    public override string ModelName
    {
      get { return "Spektrum DX18"; }
    }

    public DX18(string generator, string fileContents)
      : base(generator, fileContents)
    {
      _Logger.Debug ("DX18 Reader is created.");
    }

    protected override void ReadContents()
    {
      _Logger.Debug ("Reading the contents of the file as individual lines.");
      string[] lines = this.FileContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
      _Logger.Debug ("Finished reading. Found {0} lines.", lines.Length);

      XElement currentNode = null;
      XElement modelDescription = null;
      _Logger.Debug ("Using the base Spektrum XML model creation...");
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

      _Logger.Debug ("Finished processing all lines.");

      if (!string.IsNullOrEmpty(postfixText))
      {
        _Logger.Debug ("Postfix text was found. Storing this meta information.");
        XElement postfixModelText = new XElement(this.ModelDescNamespace + "PostfixModelText");
        postfixModelText.Value = String.Join(" ", UTF8Encoding.UTF8.GetBytes(postfixText));
        //postfixModelText.SetValue(postfixText);
        modelDescription.Add(postfixModelText);
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
      if (!this.IsRead)
        throw new InvalidOperationException ("The SPM file has not been read");

      _Logger.Info ("Loading a convertable model for a DX18 read SPM file.");
      _Logger.Debug ("Deserializing the XML to a Models.Spektrum.DX18.SpektrumModel type.");

      try
      {
        this._Model = SerializationHelper<Models.Spektrum.DX18.SpektrumModel>.DeserializeFromParse(this._ReadFile.ToString());
      }
      catch (Exception e)
      {
        _Logger.ErrorException ("Failed to deserialize the DX18 XML to an object structure.", e);
      }

      return this._Model as IConvertableModel;
    }

    #endregion
  }
}
