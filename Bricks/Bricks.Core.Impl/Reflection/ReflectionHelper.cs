#region

using System.Collections.Immutable;
using System.Reflection;
using System.Text;

using Bricks.Core.Reflection;
using Bricks.Core.Sync;

#endregion

namespace Bricks.Core.Impl.Reflection
{
	internal sealed class ReflectionHelper : IReflectionHelper
	{
		private IImmutableDictionary<MethodBase, string> _methodFullNames;
		private readonly IInterlockedHelper _interlockedHelper;

		public ReflectionHelper(IInterlockedHelper interlockedHelper)
		{
			_interlockedHelper = interlockedHelper;
			_methodFullNames = ImmutableDictionary.Create<MethodBase, string>();
		}

		#region Implementation of IReflectionHelper

		public string GetFullName(MethodBase method)
		{
			return _interlockedHelper.CompareExchange(ref _methodFullNames, x =>
				{
					string fullName;
					IImmutableDictionary<MethodBase, string> newValue;
					if (x.TryGetValue(method, out fullName))
					{
						newValue = x;
					}
					else
					{
						fullName = GetFullNameCore(method);
						newValue = x.Add(method, fullName);
					}

					return _interlockedHelper.CreateChangeResult(newValue, fullName);
				});
		}

		#endregion

		private static string GetFullNameCore(MethodBase method)
		{
			var methodFullNameBuilder = new StringBuilder();
			var reflectedType = method.ReflectedType;
			if (reflectedType != null)
			{
				methodFullNameBuilder.Append(reflectedType.Name);
				methodFullNameBuilder.Append('.');
			}

			methodFullNameBuilder.Append(method.Name);
			var methodFullName = methodFullNameBuilder.ToString();
			return string.Intern(methodFullName);
		}
	}
}