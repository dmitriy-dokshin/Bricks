#region

using System;

#endregion

namespace Bricks.Core.Sync
{
	public sealed class Lock
	{
		public Lock()
		{
		}

		public Lock(string key, string key1, DateTimeOffset createdAt)
		{
			Key = key;
			Key1 = key1;
			CreatedAt = createdAt;
		}

		public string Key { get; private set; }

		public string Key1 { get; private set; }

		public DateTimeOffset CreatedAt { get; private set; }
	}
}