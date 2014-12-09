#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

using Bricks.Core.IoC;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.WebAPI.Filters
{
	/// <summary>
	/// Фильтр проверки моделей.
	/// </summary>
	public sealed class ValidationFilter : IActionFilter
	{
		private readonly IServiceLocator _serviceLocator;

		public ValidationFilter(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
		}

		#region Implementation of IFilter

		/// <summary>
		/// Gets or sets a value indicating whether more than one instance of the indicated attribute can be specified for a single
		/// program element.
		/// </summary>
		/// <returns>
		/// true if more than one instance is allowed to be specified; otherwise, false. The default is false.
		/// </returns>
		public bool AllowMultiple
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Executes the filter action asynchronously.
		/// </summary>
		/// <returns>
		/// The newly created task for this operation.
		/// </returns>
		/// <param name="actionContext">The action context.</param>
		/// <param name="cancellationToken">The cancellation token assigned for this task.</param>
		/// <param name="continuation">The delegate function to continue after the action method is invoked.</param>
		public Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
		{
			CheckRequired(actionContext);
			if (actionContext.ModelState.IsValid)
			{
				CheckDefaultIfNull(actionContext);
				BuildUpParameters(actionContext);
			}

			return continuation();
		}

		#endregion

		private static void CheckRequired(HttpActionContext actionContext)
		{
			var parameterDescriptorRequiredAttributes =
				actionContext.ActionDescriptor.GetParameters()
					.Select(x => new { Parameter = x, Attribute = x.GetCustomAttributes<RequiredAttribute>().FirstOrDefault() })
					.Where(x => x.Attribute != null);
			foreach (var parameterDescriptorRequiredAttribute in parameterDescriptorRequiredAttributes)
			{
				HttpParameterDescriptor parameterDescriptor = parameterDescriptorRequiredAttribute.Parameter;
				RequiredAttribute requiredAttribute = parameterDescriptorRequiredAttribute.Attribute;
				string parameterName = parameterDescriptor.ParameterName;
				object value;
				if (!actionContext.ActionArguments.TryGetValue(parameterName, out value) || value == null || (!requiredAttribute.AllowEmptyStrings && (value as string) == string.Empty))
				{
					actionContext.ModelState.AddModelError(parameterName, Resources.NotNullErrorMessage);
				}
			}
		}

		private static void CheckDefaultIfNull(HttpActionContext actionContext)
		{
			IEnumerable<HttpParameterDescriptor> parameterDescriptors =
				actionContext.ActionDescriptor.GetParameters().Where(x => x.GetCustomAttributes<DefaultIfNullAttribute>().Any());
			foreach (HttpParameterDescriptor parameterDescriptor in parameterDescriptors)
			{
				string parameterName = parameterDescriptor.ParameterName;
				object value;
				if (!actionContext.ActionArguments.TryGetValue(parameterName, out value) || value == null)
				{
					value = Activator.CreateInstance(parameterDescriptor.ParameterType);
					actionContext.ActionArguments[parameterName] = value;
				}
			}
		}

		private void BuildUpParameters(HttpActionContext actionContext)
		{
			foreach (HttpParameterDescriptor parameterDescriptor in actionContext.ActionDescriptor.GetParameters())
			{
				string parameterName = parameterDescriptor.ParameterName;
				object value;
				if (actionContext.ActionArguments.TryGetValue(parameterName, out value) && value is IInitializable)
				{
					_serviceLocator.BuildUp(value);
				}
			}
		}
	}
}