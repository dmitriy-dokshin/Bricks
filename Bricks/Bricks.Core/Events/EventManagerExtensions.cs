#region

using System;

#endregion

namespace Bricks.Core.Events
{
	public static class EventManagerExtensions
	{
		public static IDisposable Subscribe<TEventArgs>(this IEventManager eventManager, EventHandler<TEventArgs> eventHandler, object sender = null)
			where TEventArgs : EventArgs
		{
			return eventManager.Subscribe(new EventHandlerAdapter<TEventArgs>(eventHandler), sender);
		}
	}
}