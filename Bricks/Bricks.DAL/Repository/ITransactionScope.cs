#region

using System;

#endregion

namespace Bricks.DAL.Repository
{
	/// <summary>
	/// Определяет область транзакции.
	/// </summary>
	public interface ITransactionScope : IDisposable
	{
		/// <summary>
		/// Фиксирует транзакцию.
		/// </summary>
		void Commit();

		/// <summary>
		/// Откатывает транзакцию.
		/// </summary>
		void Rollback();
	}
}