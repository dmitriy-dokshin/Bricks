#region

using System;
using System.Threading;

#endregion

namespace Bricks.Sync.Implementation
{
	/// <summary>
	/// ���������� �� ��������� <see cref="IInterlockedHelper" />.
	/// </summary>
	internal sealed class InterlockedHelper : IInterlockedHelper
	{
		#region Implementation of IInterlockedHelper

		/// <summary>
		/// ��������� ������������� ������ �������� <paramref name="target" />, ��������� ��� ��������� �������
		/// <paramref name="change" />.
		/// </summary>
		/// <typeparam name="T">��� �������� ��������.</typeparam>
		/// <param name="target">��������, ������� ���������� ��������.</param>
		/// <param name="change">������� ��������� ��������.</param>
		public void CompareExchange<T>(ref T target, Func<T, T> change) where T : class
		{
			T currentValue = Volatile.Read(ref target);
			T oldValue;
			do
			{
				oldValue = currentValue;
				T newValue = change(currentValue);
				currentValue = Interlocked.CompareExchange(ref target, newValue, currentValue);
			} while (!Equals(oldValue, currentValue));
		}

		/// <summary>
		/// ��������� ������������� ������ �������� <paramref name="target" />, ��������� ��� ��������� �������
		/// <paramref name="change" />. ����� ���������� ��������������� �������� <paramref name="change" /> ���������.
		/// </summary>
		/// <typeparam name="T">��� �������� ��������.</typeparam>
		/// <typeparam name="TResult">��� ����������.</typeparam>
		/// <param name="target">��������, ������� ���������� ��������.</param>
		/// <param name="change">������� ��������� ��������</param>
		/// <returns>��������� ������ ��������.</returns>
		public TResult CompareExchange<T, TResult>(ref T target, Func<T, IChangeResult<T, TResult>> change) where T : class
		{
			T currentValue = Volatile.Read(ref target);
			T oldValue;
			TResult result;
			do
			{
				oldValue = currentValue;
				IChangeResult<T, TResult> changeResult = change(currentValue);
				currentValue = Interlocked.CompareExchange(ref target, changeResult.NewValue, currentValue);
				result = changeResult.Result;
			} while (!Equals(oldValue, currentValue));

			return result;
		}

		/// <summary>
		/// ������ ��������� ��������� ��������.
		/// </summary>
		/// <typeparam name="T">��� �������� ��������.</typeparam>
		/// <typeparam name="TResult">��� ����������.</typeparam>
		/// <param name="newValue">����� ������� ��������.</param>
		/// <param name="result">��������� ������� ���������.</param>
		/// <returns></returns>
		public IChangeResult<T, TResult> CreateChangeResult<T, TResult>(T newValue, TResult result)
		{
			return new ChangeResult<T, TResult>(newValue, result);
		}

		#endregion

		private sealed class ChangeResult<T, TResult> : IChangeResult<T, TResult>
		{
			public ChangeResult(T newValue, TResult result)
			{
				NewValue = newValue;
				Result = result;
			}

			#region Implementation of IChangeResult<out T,out TResult>

			/// <summary>
			/// ����� ������� ��������.
			/// </summary>
			public T NewValue { get; private set; }

			/// <summary>
			/// ��������� ������� ���������.
			/// </summary>
			public TResult Result { get; private set; }

			#endregion
		}
	}
}