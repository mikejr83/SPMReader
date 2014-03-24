using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;

namespace SPMReader.Helpers
{
  /// <summary>
  /// Serialization helper methods.
  /// </summary>
  public static class SerializationHelper
  {
    /// <summary>
    /// Creates an object representation of a serializable class from the XML contained within a specific file.
    /// </summary>
    /// <param name="fileName">The filename (name of file and path) which contains the serialized XML.</param>
    /// <param name="extraTypes">Extra types which will be supplied to the XmlSerializer for deserialization.</param>
    /// <returns>An instance of an object containing the data from the XML file.</returns>
    /// <exception cref="InvalidOperationException">An error occurred during deserialization. The original exception is available using the InnerException property.</exception>
    /// <exception cref="ArgumentNullException">The input value is Nothing.</exception>
    /// <exception cref="SecurityException">The XmlReader does not have sufficient permissions to access the location of the XML data.</exception>
    public static object DeserializeFromFileName(string fileName, Type type, params Type[] extraTypes)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName", "A valid file name must be passed for the deserializer.");

      object obj = null;

      if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
      {
        XmlSerializer xs = null;

        if (extraTypes != null && extraTypes.Length > 0)
          xs = new XmlSerializer(type, extraTypes);
        else
          xs = new XmlSerializer(type);

        using (Stream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read))
        {
          try
          {
            object deserializedStream = xs.Deserialize(fileStream);

            obj = deserializedStream;
          }
          catch { }
        }
      }

      return obj;
    }

    /// <summary>
    /// Creates an object representation of a serializable class from a XML string.
    /// </summary>
    /// <param name="xml">The XML representing a serialized class</param>
    /// <param name="extraTypes">Extra types which will be supplied to the XmlSerializer for deserialization.</param>
    /// <returns>An instance of an object containing the data from the XML string.</returns>
    /// <exception cref="InvalidOperationException">An error occurred during deserialization. The original exception is available using the InnerException property.</exception>
    /// <exception cref="ArgumentNullException">The input value is Nothing.</exception>
    /// <exception cref="SecurityException">The XmlReader does not have sufficient permissions to access the location of the XML data.</exception>
    public static object DeserializeFromParse(string xml, Type type, params Type[] extraTypes)
    {
      if (string.IsNullOrEmpty(xml))
        throw new ArgumentNullException("xml", "A valid string of XML must be passed to the deserializer.");

      object obj = null;

      XmlSerializer xs = null;

      if (extraTypes != null && extraTypes.Length > 0)
        xs = new XmlSerializer(type, extraTypes);
      else
        xs = new XmlSerializer(type);

      // Encode the XML string in a UTF-8 byte array
      byte[] encodedString = Encoding.UTF8.GetBytes(xml);

      // Put the byte array into a stream and rewind it to the beginning
      MemoryStream ms = new MemoryStream(encodedString);
      ms.Flush();
      ms.Position = 0;

      // Build the XmlDocument from the MemorySteam of UTF-8 encoded bytes
      //XmlDocument xmlDoc = new XmlDocument();
      //xmlDoc.Load(ms);

      using (XmlReader reader = XmlReader.Create(ms))
      {
        object deserializedObject = xs.Deserialize(reader);

        obj = deserializedObject;
      }

      //XDocument doc = XDocument.Parse(xml);

      //object deserializedObject = s.Deserialize(doc.CreateReader());

      //obj = (T)deserializedObject;


      return obj;
    }

    /// <summary>
    /// Serializes an object of type T to XML by writing the XML to the supplied Stream.
    /// </summary>
    /// <param name="stream">The Stream where the XML will be written</param>
    /// <param name="obj">The serializable object to be serialized into XML.</param>
    /// <param name="extraTypes">Extra types which will be supplied to the XmlSerializer for serialization.</param>
    public static void Serialize(Stream stream, object obj, Type type, params Type[] extraTypes)
    {
      XmlSerializer xs = null;

      if (extraTypes != null && extraTypes.Length > 0)
        xs = new XmlSerializer(type, extraTypes);
      else
        xs = new XmlSerializer(type);

      xs.Serialize(stream, obj);
    }

    /// <summary>
    /// Serializes an object to a XML string.
    /// </summary>
    /// <param name="obj">The serializable object to serialize into XML.</param>
    /// <param name="extraTypes">Extra types which will be supplied to the XmlSerializer for serialization.</param>
    /// <returns>An XML string representing the object.</returns>
    public static string Serialize(object obj, Type type, params Type[] extraTypes)
    {
      string result = null;

      XmlSerializer xs = null;

      if (extraTypes != null && extraTypes.Length > 0)
        xs = new XmlSerializer(type, extraTypes);
      else
        xs = new XmlSerializer(type);

      using (MemoryStream memoryStream = new MemoryStream())
      {
        XmlWriterSettings settings = new XmlWriterSettings()
        {
          Encoding = Encoding.UTF8
        };
        using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
        {
          xs.Serialize(writer, obj);
        }

        result = Encoding.UTF8.GetString(memoryStream.ToArray());
      }

      return result;
    }
  }

  /// <summary>
  /// Serialization helper methods specific to a generic serializable class.
  /// </summary>
  /// <typeparam name="T">Type of class which will either be serialized to a XML string or created through deserialization of a XML string.</typeparam>
  public static class SerializationHelper<T>
  {
    /// <summary>
    /// Creates an object representation of a serializable class from the XML contained within a specific file.
    /// </summary>
    /// <param name="fileName">The filename (name of file and path) which contains the serialized XML.</param>
    /// <param name="extraTypes">Extra types which will be supplied to the XmlSerializer for deserialization.</param>
    /// <returns>An instance of an object of type T containing the data from the XML file.</returns>
    /// <exception cref="InvalidOperationException">An error occurred during deserialization. The original exception is available using the InnerException property.</exception>
    /// <exception cref="ArgumentNullException">The input value is Nothing.</exception>
    /// <exception cref="SecurityException">The XmlReader does not have sufficient permissions to access the location of the XML data.</exception>
    public static T DeserializeFromFileName(string fileName, params Type[] extraTypes)
    {
      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName", "A valid file name must be passed for the deserializer.");

      T obj = default(T);

      if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
      {
        XmlSerializer xs = null;

        if (extraTypes != null && extraTypes.Length > 0)
          xs = new XmlSerializer(typeof(T), extraTypes);
        else
          xs = new XmlSerializer(typeof(T));

        using (Stream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read))
        {
          try
          {
            object deserializedStream = xs.Deserialize(fileStream);

            obj = (T)deserializedStream;
          }
          catch { }
        }
      }

      return obj;
    }

    /// <summary>
    /// Creates an object representation of a serializable class from a XML string.
    /// </summary>
    /// <param name="xml">The XML representing a serialized class</param>
    /// <param name="extraTypes">Extra types which will be supplied to the XmlSerializer for deserialization.</param>
    /// <returns>An instance of an object of type T containing the data from the XML string.</returns>
    /// <exception cref="InvalidOperationException">An error occurred during deserialization. The original exception is available using the InnerException property.</exception>
    /// <exception cref="ArgumentNullException">The input value is Nothing.</exception>
    /// <exception cref="SecurityException">The XmlReader does not have sufficient permissions to access the location of the XML data.</exception>
    public static T DeserializeFromParse(string xml, params Type[] extraTypes)
    {
      if (string.IsNullOrEmpty(xml))
        throw new ArgumentNullException("xml", "A valid string of XML must be passed to the deserializer.");

      T obj = default(T);

      XmlSerializer xs = null;

      if (extraTypes != null && extraTypes.Length > 0)
        xs = new XmlSerializer(typeof(T), extraTypes);
      else
        xs = new XmlSerializer(typeof(T));

      // Encode the XML string in a UTF-8 byte array
      byte[] encodedString = Encoding.UTF8.GetBytes(xml);

      // Put the byte array into a stream and rewind it to the beginning
      MemoryStream ms = new MemoryStream(encodedString);
      ms.Flush();
      ms.Position = 0;

      // Build the XmlDocument from the MemorySteam of UTF-8 encoded bytes
      //XmlDocument xmlDoc = new XmlDocument();
      //xmlDoc.Load(ms);

      using (XmlReader reader = XmlReader.Create(ms))
      {
        object deserializedObject = xs.Deserialize(reader);

        obj = (T)deserializedObject;
      }

      //XDocument doc = XDocument.Parse(xml);

      //object deserializedObject = s.Deserialize(doc.CreateReader());

      //obj = (T)deserializedObject;


      return obj;
    }

    /// <summary>
    /// Serializes an object of type T to XML by writing the XML to the supplied Stream.
    /// </summary>
    /// <param name="stream">The Stream where the XML will be written</param>
    /// <param name="obj">The serializable object to be serialized into XML.</param>
    /// <param name="extraTypes">Extra types which will be supplied to the XmlSerializer for serialization.</param>
    public static void Serialize(Stream stream, T obj, params Type[] extraTypes)
    {
      XmlSerializer xs = null;

      if (extraTypes != null && extraTypes.Length > 0)
        xs = new XmlSerializer(typeof(T), extraTypes);
      else
        xs = new XmlSerializer(typeof(T));

      xs.Serialize(stream, obj);
    }

    /// <summary>
    /// Serializes an object of type T to a XML string.
    /// </summary>
    /// <param name="obj">The serializable object to serialize into XML.</param>
    /// <param name="extraTypes">Extra types which will be supplied to the XmlSerializer for serialization.</param>
    /// <returns>An XML string representing the object.</returns>
    public static string Serialize(T obj, params Type[] extraTypes)
    {
      string result = null;

      XmlSerializer xs = null;

      if (extraTypes != null && extraTypes.Length > 0)
        xs = new XmlSerializer(typeof(T), extraTypes);
      else
        xs = new XmlSerializer(typeof(T));

      using (MemoryStream memoryStream = new MemoryStream())
      {
        XmlWriterSettings settings = new XmlWriterSettings()
        {
          Encoding = Encoding.UTF8
        };
        using (XmlWriter writer = XmlWriter.Create(memoryStream, settings))
        {
          xs.Serialize(writer, obj);
        }

        result = Encoding.UTF8.GetString(memoryStream.ToArray());
      }

      return result;
    }
  }
}
