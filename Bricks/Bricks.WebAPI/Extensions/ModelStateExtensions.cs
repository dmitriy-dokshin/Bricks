#region

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http.ModelBinding;

#endregion

namespace Bricks.WebAPI.Extensions
{
	internal static class ModelStateExtensions
	{
		public static string GetMessage(this ModelStateDictionary modelStateDictionary)
		{
			var messageBuilder = new StringBuilder();
			IEnumerable<ModelError> modelErrors = modelStateDictionary.Values.SelectMany(x => x.Errors);
			foreach (ModelError modelError in modelErrors)
			{
				if (!string.IsNullOrEmpty(modelError.ErrorMessage))
				{
					messageBuilder.AppendLine(modelError.ErrorMessage);
				}
				else if (modelError.Exception != null)
				{
					messageBuilder.AppendLine(modelError.Exception.Message);
				}
			}

			return messageBuilder.ToString();
		}
	}
}