﻿#region

using System;

using Bricks.Helpers.Collections;
using Bricks.Sync;

#endregion

namespace Bricks.Helpers.Sync.Implementation
{
	internal sealed class LockContainer : ILockContainer
	{
		private readonly IContainer<object, ILockAsync> _container;
		private readonly ISyncFactory _syncFactory;

		public LockContainer(IContainerFactory containerFactory, ISyncFactory syncFactory)
		{
			_container = containerFactory.Create<object, ILockAsync>();
			_syncFactory = syncFactory;
		}

		#region Implementation of ILockContainer

		public IDisposable GetLock(object key, out ILockAsync @lock)
		{
			return _container.GetOrAdd(key, () => _syncFactory.CreateAsyncLock(), out @lock);
		}

		#endregion
	}
}