﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace UltrawideHelper.Data;

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
			throw new ArgumentNullException(nameof(exception));
		}

		var oldCulture = Thread.CurrentThread.CurrentCulture;
		var oldUiCulture = Thread.CurrentThread.CurrentUICulture;

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
						this.Data ??= new SerializableDictionary<object, object>();
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

			if (exception is AggregateException aggregateException)
			{
				this.InnerExceptions = new List<SerializableException>();

				foreach (var innerException in aggregateException.InnerExceptions)
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
				this.TargetSite = $"{exception.TargetSite} @ {exception.TargetSite.DeclaringType}";
			}

			this.ExtendedInformation = this.GetExtendedInformation(exception);

		}
		finally
		{
			Thread.CurrentThread.CurrentCulture = oldCulture;
			Thread.CurrentThread.CurrentUICulture = oldUiCulture;
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

		using var stream = new MemoryStream();

		stream.SetLength(0);
		serializer.Serialize(stream, this);
		stream.Position = 0;
		var doc = XDocument.Load(stream);
		return doc.Root?.ToString();
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