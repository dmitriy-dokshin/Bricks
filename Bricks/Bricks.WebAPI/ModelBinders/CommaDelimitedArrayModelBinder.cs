#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;

#endregion

namespace Bricks.WebAPI.ModelBinders
{
	public class CommaDelimitedArrayModelBinder : IModelBinder
	{
		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			string modelName = bindingContext.ModelName;
			ValueProviderResult value = bindingContext.ValueProvider.GetValue(modelName);
			if (value != null)
			{
				string attemptedValue = value.AttemptedValue;
				Type modelType = bindingContext.ModelType;
				Type elementType = null;
				if (modelType.IsArray)
				{
					elementType = modelType.GetElementType();
				}
				else if (modelType.IsGenericType)
				{
					Type genericType = modelType.GetGenericTypeDefinition();
					if (genericType == typeof(IEnumerable<>) || genericType == typeof(IReadOnlyCollection<>))
					{
						elementType = modelType.GetGenericArguments()[0];
					}
				}

				if (elementType == null)
				{
					throw new InvalidOperationException(Resources.CommaDelimitedArrayModelBinder_BindModel_ArrayOrEnumerableOrReadOnlyCollectionRequired);
				}

				if (attemptedValue != null)
				{
					var converter = TypeDescriptor.GetConverter(elementType);
					var values = Array.ConvertAll(attemptedValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries), x => converter.ConvertFromString(x != null ? x.Trim() : null));
					var typedValues = Array.CreateInstance(elementType, values.Length);
					values.CopyTo(typedValues, 0);
					bindingContext.Model = typedValues;
				}
				else
				{
					bindingContext.Model = null;
				}

				return true;
			}

			return false;
		}
	}
}