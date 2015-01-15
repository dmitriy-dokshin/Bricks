#region

using System;
using System.Runtime.Serialization;

using Bricks.Core.Reflection;

#endregion

namespace Bricks.Core.Conversion
{
	/// <summary>
	/// Ошибка преобразования.
	/// </summary>
	[Serializable]
	public class ConversionException : Exception
	{
		public ConversionException(Type sourceType, Type destinationType)
		{
			SourceType = sourceType;
			DestinationType = destinationType;
		}

		public ConversionException(string message, Type sourceType, Type destinationType)
			: base(message)
		{
			SourceType = sourceType;
			DestinationType = destinationType;
		}

		public ConversionException(string message, Exception inner, Type sourceType, Type destinationType)
			: base(message, inner)
		{
			SourceType = sourceType;
			DestinationType = destinationType;
		}

		protected ConversionException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			var sourceTypeName = info.GetString(Refl.GetMemberName(() => SourceType));
			SourceType = !string.IsNullOrEmpty(sourceTypeName) ? Type.GetType(sourceTypeName) : null;
			var destinationTypeName = info.GetString(Refl.GetMemberName(() => DestinationType));
			DestinationType = !string.IsNullOrEmpty(destinationTypeName) ? Type.GetType(destinationTypeName) : null;
		}

		public Type SourceType { get; private set; }

		public Type DestinationType { get; private set; }

		#region Overrides of Exception

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue(info.GetString(Refl.GetMemberName(() => SourceType)), SourceType != null ? SourceType.AssemblyQualifiedName : null);
			info.AddValue(info.GetString(Refl.GetMemberName(() => Data)), DestinationType != null ? DestinationType.AssemblyQualifiedName : null);
		}

		#endregion
	}
}