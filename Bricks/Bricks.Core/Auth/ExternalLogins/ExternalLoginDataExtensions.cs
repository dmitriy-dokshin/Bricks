#region

using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Web;

#endregion

namespace Bricks.Core.Auth.ExternalLogins
{
	public static class ExternalLoginDataExtensions
	{
		public static string GetFullName(this IExternalLoginData externalLoginData)
		{
			var nameBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(externalLoginData.FirstName))
			{
				nameBuilder.Append(externalLoginData.FirstName);
			}

			if (!string.IsNullOrEmpty(externalLoginData.LastName))
			{
				if (nameBuilder.Length > 0)
				{
					nameBuilder.Append(' ');
				}

				nameBuilder.Append(externalLoginData.LastName);
			}

			string name = nameBuilder.Length > 0 ? nameBuilder.ToString() : null;
			return name;
		}

		public static async Task<Image> GetImage(this IExternalLoginData externalLoginData, IWebClient webClient, CancellationToken cancellationToken)
		{
			Image image = null;
			if (!string.IsNullOrEmpty(externalLoginData.ImageUrl))
			{
				IWebResponse webResponse = await webClient.ExecuteRequestAsync(new Uri(externalLoginData.ImageUrl), cancellationToken);
				if (webResponse.Success)
				{
					image = Image.FromStream(webResponse.Stream);
				}
			}

			return image;
		}
	}
}