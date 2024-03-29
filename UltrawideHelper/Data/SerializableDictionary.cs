using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UltrawideHelper.Data;

[Serializable]
[XmlRoot("dictionary")]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
    public SerializableDictionary()
    {
    }

    public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
    {
        if (dictionary == null)
        {
            throw new ArgumentNullException();
        }

        foreach (var pair in dictionary)
        {
            this.Add(pair.Key, pair.Value);
        }
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        var inner = reader.ReadSubtree();
        var xElement = XElement.Load(inner);

        if (xElement.HasElements)
        {
            foreach (var element in xElement.Elements())
            {
                this.Add((TKey)Convert.ChangeType(element.Name.ToString(), typeof(TKey)), (TValue)Convert.ChangeType(element.Value, typeof(TValue)));
            }
        }

        inner.Close();
        reader.ReadEndElement();
    }

    public void WriteXml(XmlWriter writer)
    {
        foreach (var key in this.Keys)
        {
            writer.WriteStartElement(key.ToString()?.Replace(" ", "") ?? string.Empty);

            // Check to see if we can actually serialize element
            if (this[key].GetType().IsSerializable)
            {
                // if it's Serializable doesn't mean serialization will succeed (IE. GUID and SQLError types)
                try
                {
                    writer.WriteValue(this[key]);
                }
                catch (Exception)
                {
                    // we're not Throwing anything here, otherwise evil thing will happen
                    writer.WriteValue(this[key].ToString());
                }
            }
            else
            {
                // If Type has custom implementation of ToString() we'll get something useful here
                // Otherwise we'll get Type string. (Still better than crashing).
                writer.WriteValue(this[key].ToString());
            }

            writer.WriteEndElement();
        }
    }
}