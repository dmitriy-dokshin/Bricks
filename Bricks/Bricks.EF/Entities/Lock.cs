#region

using System;

#endregion

namespace Bricks.EF.Entities
{
	public sealed class Lock
	{
		public Lock()
		{
		}

		public Lock(string key, string key1, DateTime createdAt)
		{
			Key = key;
			Key1 = key1;
			CreatedAt = createdAt;
		}

		public string Key { get; private set; }
		public string Key1 { get; private set; }
		public DateTime CreatedAt { get; private set; }
	}
}