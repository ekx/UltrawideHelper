using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace UltrawideHelper
{
	[Serializable]
	public class SerializableException
	{
		public SerializableException()
		{
		}

		public SerializableException(Exception exception)
		{
			if (exception == null)
			{
				throw new ArgumentNullException();
			}

			var oldCulture = Thread.CurrentThread.CurrentCulture;
			var oldUICulture = Thread.CurrentThread.CurrentUICulture;

			try
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

				this.Type = exception.GetType().ToString();

				if (exception.Data.Count != 0)
				{
					foreach (DictionaryEntry entry in exception.Data)
					{
						if (entry.Value != null)
						{
							if (this.Data == null)
							{
								this.Data = new SerializableDictionary<object, object>();
							}

							this.Data.Add(entry.Key, entry.Value);
						}
					}
				}

				if (exception.HelpLink != null)
				{
					this.HelpLink = exception.HelpLink;
				}

				if (exception.InnerException != null)
				{
					this.InnerException = new SerializableException(exception.InnerException);
				}

				if (exception is AggregateException)
				{
					this.InnerExceptions = new List<SerializableException>();

					foreach (var innerException in ((AggregateException)exception).InnerExceptions)
					{
						this.InnerExceptions.Add(new SerializableException(innerException));
					}

					this.InnerExceptions.RemoveAt(0);
				}

				this.Message = exception.Message != string.Empty ? exception.Message : string.Empty;

				if (exception.Source != null)
				{
					this.Source = exception.Source;
				}

				if (exception.StackTrace != null)
				{
					this.StackTrace = exception.StackTrace;
				}

				if (exception.TargetSite != null)
				{
					this.TargetSite = string.Format("{0} @ {1}", exception.TargetSite, exception.TargetSite.DeclaringType);
				}

				this.ExtendedInformation = this.GetExtendedInformation(exception);

			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = oldCulture;
				Thread.CurrentThread.CurrentUICulture = oldUICulture;
			}
		}

		public SerializableDictionary<object, object> Data { get; set; }

		public SerializableDictionary<string, object> ExtendedInformation { get; set; }

		public string HelpLink { get; set; }

		public SerializableException InnerException { get; set; }

		public List<SerializableException> InnerExceptions { get; set; }

		public string Message { get; set; }

		public string Source { get; set; }

		public string StackTrace { get; set; }

		public string TargetSite { get; set; }

		public string Type { get; set; }

		public override string ToString()
		{
			var serializer = new XmlSerializer(typeof(SerializableException));

			using (var stream = new MemoryStream())
			{
				stream.SetLength(0);
				serializer.Serialize(stream, this);
				stream.Position = 0;
				var doc = XDocument.Load(stream);
				return doc.Root.ToString();
			}
		}

		private SerializableDictionary<string, object> GetExtendedInformation(Exception exception)
		{
			var extendedProperties = (from property in exception.GetType().GetProperties()
									  where
										  property.Name != "Data" && property.Name != "InnerExceptions" && property.Name != "InnerException"
										  && property.Name != "Message" && property.Name != "Source" && property.Name != "StackTrace"
										  && property.Name != "TargetSite" && property.Name != "HelpLink" && property.CanRead
									  select property).ToArray();

			if (extendedProperties.Any())
			{
				var extendedInformation = new SerializableDictionary<string, object>();

				foreach (var property in extendedProperties.Where(property => property.GetValue(exception, null) != null))
				{
					extendedInformation.Add(property.Name, property.GetValue(exception, null));
				}

				return extendedInformation;
			}
			else
			{
				return null;
			}
		}
	}

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
				writer.WriteStartElement(key.ToString().Replace(" ", ""));

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
}
