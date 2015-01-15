#region

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace Bricks.WebAPI.Formatters
{
	public sealed class TextPlainFormatter : MediaTypeFormatter
	{
		public TextPlainFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
		}

		#region Overrides of MediaTypeFormatter

		public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
		{
			Encoding encoding = null;
			string charSet = content.Headers.ContentType.CharSet;
			if (!string.IsNullOrEmpty(charSet))
			{
				encoding = Encoding.GetEncoding(charSet);
			}

			using (StreamReader streamReader = encoding != null ? new StreamReader(readStream, encoding) : new StreamReader(readStream))
			{
				return await streamReader.ReadToEndAsync();
			}
		}

		public override async Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
		{
			Encoding encoding = null;
			string charSet = content.Headers.ContentType.CharSet;
			if (!string.IsNullOrEmpty(charSet))
			{
				encoding = Encoding.GetEncoding(charSet);
			}

			var s = (string)value;
			using (StreamWriter streamWriter = encoding != null ? new StreamWriter(writeStream, encoding) : new StreamWriter(writeStream))
			{
				await streamWriter.WriteAsync(s);
			}
		}

		public override bool CanReadType(Type type)
		{
			return type == typeof(string);
		}

		public override bool CanWriteType(Type type)
		{
			return type == typeof(string);
		}

		#endregion
	}
}