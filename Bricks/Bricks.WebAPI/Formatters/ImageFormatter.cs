#region

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Bricks.Core.Images;

#endregion

namespace Bricks.WebAPI.Formatters
{
	public sealed class ImageFormatter : MediaTypeFormatter
	{
		public ImageFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/gif"));
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpeg"));
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/pjpeg"));
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/tiff"));
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/vnd.microsoft.icon"));
		}

		#region Overrides of MediaTypeFormatter

		public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
		{
			Image image;
			try
			{
				image = Image.FromStream(readStream);
			}
			catch (ArgumentException)
			{
				image = null;
			}

			return Task.FromResult((object)image);
		}

		public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content, TransportContext transportContext)
		{
			var image = (Image)value;
			byte[] imageData = image.GetBytes();
			var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(imageData) };
			HttpContentHeaders httpContentHeaders = httpResponseMessage.Content.Headers;
			string contentType = string.Format("image/{0}", image.RawFormat.ToStringFixed());
			httpContentHeaders.ContentType = new MediaTypeHeaderValue(contentType);
			return Task.FromResult(httpResponseMessage);
		}

		public override bool CanReadType(Type type)
		{
			return type == typeof(Image);
		}

		public override bool CanWriteType(Type type)
		{
			return type == typeof(Image);
		}

		#endregion
	}
}