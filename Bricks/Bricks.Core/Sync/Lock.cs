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

		public Lock(Guid key, DateTimeOffset createdAt, Guid ownerId)
		{
			Key = key;
			CreatedAt = createdAt;
			OwnerId = ownerId;
		}

		public long Id { get; private set; }

		public Guid Key { get; private set; }

		public DateTimeOffset CreatedAt { get; private set; }

		public Guid OwnerId { get; private set; }
	}
}