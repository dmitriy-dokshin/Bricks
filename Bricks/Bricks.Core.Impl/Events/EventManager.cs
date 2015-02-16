#region

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Bricks.Core.Disposing;
using Bricks.Core.Events;
using Bricks.Core.Sync;

using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;

#endregion

namespace Bricks.Core.Impl.Events
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IEventManager" />.
	/// </summary>
	public sealed class EventManager : IEventManager
	{
		private IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<IEventHandler>>> _eventHandlersByEventArgsType;
		private IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<Type>>> _eventHandlerTypesByEventArgsType;
		private readonly IDisposableHelper _disposableHelper;
		private readonly IInterlockedHelper _interlockedHelper;
		private readonly object _nullSender = new object();
		private readonly IServiceLocator _serviceLocator;

		public EventManager(IServiceLocator serviceLocator, IInterlockedHelper interlockedHelper, IDisposableHelper disposableHelper)
		{
			_serviceLocator = serviceLocator;
			_interlockedHelper = interlockedHelper;
			_disposableHelper = disposableHelper;
			_eventHandlerTypesByEventArgsType = ImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<Type>>>.Empty;
			_eventHandlersByEventArgsType = ImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<IEventHandler>>>.Empty;
		}

		private void Unsubscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler, object sender = null)
			where TEventArgs : EventArgs
		{
			if (sender == null)
			{
				sender = _nullSender;
			}

			_interlockedHelper.CompareExchange(ref _eventHandlersByEventArgsType, x => Unsubscribe(eventHandler, sender, x));
		}

		private void Unsubscribe<TEventArgs, TEventHandler>(object sender = null)
			where TEventArgs : EventArgs
			where TEventHandler : IEventHandler<TEventArgs>
		{
			if (sender == null)
			{
				sender = _nullSender;
			}

			_interlockedHelper.CompareExchange(ref _eventHandlerTypesByEventArgsType, x => Unsubscribe<TEventArgs, TEventHandler>(sender, x));
		}

		private static IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<IEventHandler>>> Subscribe<TEventArgs>(
			IEventHandler<TEventArgs> eventHandler, object sender, IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<IEventHandler>>> eventHandlersByEventArgsType)
			where TEventArgs : EventArgs
		{
			var type = typeof(TEventArgs);
			IImmutableDictionary<object, IImmutableSet<IEventHandler>> eventHandlersBySender;
			if (!eventHandlersByEventArgsType.TryGetValue(type, out eventHandlersBySender))
			{
				eventHandlersBySender = ImmutableDictionary<object, IImmutableSet<IEventHandler>>.Empty;
			}

			IImmutableSet<IEventHandler> eventHandlers;
			if (!eventHandlersBySender.TryGetValue(sender, out eventHandlers))
			{
				eventHandlers = ImmutableHashSet<IEventHandler>.Empty;
			}

			eventHandlers = eventHandlers.Add(eventHandler);
			eventHandlersBySender = eventHandlersBySender.SetItem(sender, eventHandlers);
			return eventHandlersByEventArgsType.SetItem(type, eventHandlersBySender);
		}

		private static IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<IEventHandler>>> Unsubscribe<TEventArgs>(
			IEventHandler<TEventArgs> eventHandler, object sender, IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<IEventHandler>>> eventHandlersByEventArgsType)
			where TEventArgs : EventArgs
		{
			var type = typeof(TEventArgs);
			IImmutableDictionary<object, IImmutableSet<IEventHandler>> eventHandlersBySender;
			if (eventHandlersByEventArgsType.TryGetValue(type, out eventHandlersBySender))
			{
				IImmutableSet<IEventHandler> eventHandlers;
				if (eventHandlersBySender.TryGetValue(sender, out eventHandlers))
				{
					eventHandlers = eventHandlers.Remove(eventHandler);
					eventHandlersBySender =
						eventHandlers.Count > 0
							? eventHandlersBySender.SetItem(sender, eventHandlers)
							: eventHandlersBySender.Remove(sender);

					eventHandlersByEventArgsType =
						eventHandlersBySender.Count > 0
							? eventHandlersByEventArgsType.SetItem(type, eventHandlersBySender)
							: eventHandlersByEventArgsType.Remove(type);
				}
			}

			return eventHandlersByEventArgsType;
		}

		private static IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<Type>>> Subscribe<TEventArgs, TEventHandler>(
			object sender, IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<Type>>> eventHandlerTypesByEventArgsType)
			where TEventArgs : EventArgs
			where TEventHandler : IEventHandler<TEventArgs>
		{
			var type = typeof(TEventArgs);
			IImmutableDictionary<object, IImmutableSet<Type>> eventHandlerTypesBySender;
			if (!eventHandlerTypesByEventArgsType.TryGetValue(type, out eventHandlerTypesBySender))
			{
				eventHandlerTypesBySender = ImmutableDictionary<object, IImmutableSet<Type>>.Empty;
			}

			IImmutableSet<Type> eventHandlerTypes;
			if (!eventHandlerTypesBySender.TryGetValue(sender, out eventHandlerTypes))
			{
				eventHandlerTypes = ImmutableHashSet<Type>.Empty;
			}

			eventHandlerTypes = eventHandlerTypes.Add(typeof(TEventHandler));
			eventHandlerTypesBySender = eventHandlerTypesBySender.SetItem(sender, eventHandlerTypes);
			return eventHandlerTypesByEventArgsType.SetItem(type, eventHandlerTypesBySender);
		}

		private static IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<Type>>> Unsubscribe<TEventArgs, TEventHandler>(
			object sender, IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<Type>>> eventHandlerTypesByEventArgsType)
			where TEventArgs : EventArgs
			where TEventHandler : IEventHandler<TEventArgs>
		{
			var type = typeof(TEventArgs);
			IImmutableDictionary<object, IImmutableSet<Type>> eventHandlerTypesBySender;
			if (eventHandlerTypesByEventArgsType.TryGetValue(type, out eventHandlerTypesBySender))
			{
				eventHandlerTypesBySender = ImmutableDictionary<object, IImmutableSet<Type>>.Empty;
				IImmutableSet<Type> eventHandlerTypes;
				if (eventHandlerTypesBySender.TryGetValue(sender, out eventHandlerTypes))
				{
					eventHandlerTypes = eventHandlerTypes.Remove(typeof(TEventHandler));
					eventHandlerTypesBySender =
						eventHandlerTypes.Count > 0
							? eventHandlerTypesBySender.SetItem(sender, eventHandlerTypes)
							: eventHandlerTypesBySender.Remove(sender);

					eventHandlerTypesByEventArgsType
						= eventHandlerTypesBySender.Count > 0
							  ? eventHandlerTypesByEventArgsType.SetItem(type, eventHandlerTypesBySender)
							  : eventHandlerTypesByEventArgsType.Remove(type);
				}
			}

			return eventHandlerTypesByEventArgsType;
		}

		#region Implementation of IEventManager

		public IDisposable Subscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler, object sender = null)
			where TEventArgs : EventArgs
		{
			if (sender == null)
			{
				sender = _nullSender;
			}

			_interlockedHelper.CompareExchange(ref _eventHandlersByEventArgsType, x => Subscribe(eventHandler, sender, x));
			return _disposableHelper.Action(() => Unsubscribe(eventHandler, sender));
		}

		public IDisposable Subscribe<TEventArgs, TEventHandler>(object sender = null)
			where TEventArgs : EventArgs
			where TEventHandler : IEventHandler<TEventArgs>
		{
			if (sender == null)
			{
				sender = _nullSender;
			}

			_interlockedHelper.CompareExchange(ref _eventHandlerTypesByEventArgsType, x => Subscribe<TEventArgs, TEventHandler>(sender, x));
			return _disposableHelper.Action(() => Unsubscribe<TEventArgs, TEventHandler>(sender));
		}

		public IDisposable Subscribe<TEventArgs>(out IObservable<TEventArgs> observable, EventHandlerObservableInvokeMode invokeMode = EventHandlerObservableInvokeMode.Series, object sender = null) where TEventArgs : EventArgs
		{
			var unityContainer = _serviceLocator.GetInstance<IUnityContainer>();
			var eventHandlerObservable = unityContainer.Resolve<EventHandlerObservable<TEventArgs>>(new ParameterOverride("invokeMode", invokeMode));
			observable = eventHandlerObservable;
			return Subscribe(eventHandlerObservable, sender);
		}

		public async Task Raise<TEventArgs>(object sender, TEventArgs args, CancellationToken cancellationToken, RaiseMode mode = RaiseMode.Series) where TEventArgs : EventArgs
		{
			IImmutableDictionary<object, IImmutableSet<IEventHandler>> eventHandlersBySender;
			List<IEventHandler<TEventArgs>> eventHandlers = new List<IEventHandler<TEventArgs>>();
			IImmutableSet<IEventHandler> eventHandlerSet;
			if (_eventHandlersByEventArgsType.TryGetValue(typeof(TEventArgs), out eventHandlersBySender)
				&& ((eventHandlersBySender.TryGetValue(sender, out eventHandlerSet)
					 || eventHandlersBySender.TryGetValue(_nullSender, out eventHandlerSet))))
			{
				eventHandlers.AddRange(eventHandlerSet.Cast<IEventHandler<TEventArgs>>());
			}

			IImmutableDictionary<object, IImmutableSet<Type>> eventHandlerTypesBySender;
			IImmutableSet<Type> eventHandlerTypes;
			if (_eventHandlerTypesByEventArgsType.TryGetValue(typeof(TEventArgs), out eventHandlerTypesBySender)
				&& (eventHandlerTypesBySender.TryGetValue(sender, out eventHandlerTypes)
					|| eventHandlerTypesBySender.TryGetValue(_nullSender, out eventHandlerTypes)))
			{
				eventHandlers.AddRange(eventHandlerTypes.Select(x => (IEventHandler<TEventArgs>)_serviceLocator.GetInstance(x)));
			}

			switch (mode)
			{
				case RaiseMode.Series:
					foreach (var eventHandler in eventHandlers)
					{
						cancellationToken.ThrowIfCancellationRequested();
						await eventHandler.InvokeAsync(sender, args, cancellationToken);
					}
					break;
				case RaiseMode.Parallel:
					await Task.WhenAll(eventHandlers.Select(x => x.InvokeAsync(sender, args, cancellationToken)));
					break;
				default:
					throw new ArgumentOutOfRangeException("mode");
			}
		}

		#endregion
	}
}