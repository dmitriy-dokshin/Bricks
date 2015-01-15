#region

using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;

using Bricks.Core.Configuration;
using Bricks.WebAPI.Extensions;

using Microsoft.Practices.ServiceLocation;

using Newtonsoft.Json;

#endregion

namespace Bricks.WebAPI.Controllers
{
	public abstract class ApiControllerBase : ApiController
	{
		private readonly JsonSerializerSettings _jsonSerializerSettings;

		protected ApiControllerBase()
		{
			_jsonSerializerSettings = ServiceLocator.Current.GetInstance<IConfigurationManager>().GetSettings<JsonSerializerSettings>();
		}

		protected new JsonResult<T> Json<T>(T content)
		{
			return Json(content, _jsonSerializerSettings);
		}

		protected new BadRequestErrorMessageResult BadRequest(ModelStateDictionary modelState)
		{
			return BadRequest(modelState.GetMessage());
		}
	}
}