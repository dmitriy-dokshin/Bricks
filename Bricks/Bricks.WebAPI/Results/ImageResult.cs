#region

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

using Bricks.Core.Images;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.WebAPI.Results
{
	public sealed class ImageResult : IHttpActionResult
	{
		private readonly int? _height;
		private readonly byte[] _imageData;
		private readonly string _imageFormat;
		private readonly IImageProcessor _imageProcessor;
		private readonly bool _preserveAspectRatio;
		private readonly int? _width;

		public ImageResult(byte[] imageData, string imageFormat, int? width = null, int? height = null, bool preserveAspectRatio = true)
		{
			_imageData = imageData;
			_imageFormat = imageFormat;
			_width = width;
			_height = height;
			_preserveAspectRatio = preserveAspectRatio;
			_imageProcessor = ServiceLocator.Current.GetInstance<IImageProcessor>();
		}

		#region Implementation of IHttpActionResult

		/// <summary>
		/// Creates an <see cref="T:System.Net.Http.HttpResponseMessage" /> asynchronously.
		/// </summary>
		/// <returns>
		/// A task that, when completed, contains the <see cref="T:System.Net.Http.HttpResponseMessage" />.
		/// </returns>
		/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
		{
			byte[] imageData = _imageProcessor.Resize(_imageData, _width, _height, _preserveAspectRatio, ImageHelper.ParseFormat(_imageFormat));
			var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(imageData) };
			HttpContentHeaders httpContentHeaders = httpResponseMessage.Content.Headers;
			string contentType = string.Format("image/{0}", _imageFormat);
			httpContentHeaders.ContentType = new MediaTypeHeaderValue(contentType);
			return Task.FromResult(httpResponseMessage);
		}

		#endregion
	}
}