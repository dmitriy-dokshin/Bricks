#region

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Practices.ServiceLocation;

#endregion

namespace Bricks.Core.Events.Implementation
{
	/// <summary>
	/// Реализация по умолчанию <see cref="IEventManager" />.
	/// </summary>
	public sealed class EventManager : IEventManager
	{
		private readonly object _nullSender = new object();
		private readonly IServiceLocator _serviceLocator;
		private IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<Type>>> _eventHandlerTypesByEventArgsType;
		private IImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<IEventHandler>>> _eventHandlersByEventArgsType;

		public EventManager(IServiceLocator serviceLocator)
		{
			_serviceLocator = serviceLocator;
			_eventHandlerTypesByEventArgsType = ImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<Type>>>.Empty;
			_eventHandlersByEventArgsType = ImmutableDictionary<Type, IImmutableDictionary<object, IImmutableSet<IEventHandler>>>.Empty;
		}

		#region Implementation of IEventManager

		/// <summary>
		/// Подписывается на события с аргументами типа <typeparamref name="TEventArgs" />.
		/// </summary>
		/// <typeparam name="TEventArgs">Тип аргументов события.</typeparam>
		/// <param name="eventHandler">Обработчик события.</param>
		/// <param name="sender">
		/// Отправитель события. Если не указан, обрабатываются все события с аргументами типа
		/// <typeparamref name="TEventArgs" />.
		/// </param>
		public void Subscribe<TEventArgs>(IEventHandler<TEventArgs> eventHandler, object sender = null) where TEventArgs : EventArgs
		{
			if (sender == null)
			{
				sender = _nullSender;
			}

			Type type = typeof(TEventArgs);
			IImmutableDictionary<object, IImmutableSet<IEventHandler>> eventHandlersBySender;
			if (!_eventHandlersByEventArgsType.TryGetValue(type, out eventHandlersBySender))
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
			_eventHandlersByEventArgsType = _eventHandlersByEventArgsType.SetItem(type, eventHandlersBySender);
		}

		/// <summary>
		/// Подписывается на события с аргументами типа <typeparamref name="TEventArgs" />.
		/// </summary>
		/// <typeparam name="TEventArgs">Тип аргументов события.</typeparam>
		/// <typeparam name="TEventHandler">Тип обработчика события.</typeparam>
		/// <param name="sender">
		/// Отправитель события. Если не указан, обрабатываются все события с аргументами типа
		/// <typeparamref name="TEventArgs" />.
		/// </param>
		public void Subscribe<TEventArgs, TEventHandler>(object sender = null) where TEventArgs : EventArgs where TEventHandler : IEventHandler<TEventArgs>
		{
			if (sender == null)
			{
				sender = _nullSender;
			}

			Type type = typeof(TEventArgs);
			IImmutableDictionary<object, IImmutableSet<Type>> eventHandlerTypesBySender;
			if (!_eventHandlerTypesByEventArgsType.TryGetValue(type, out eventHandlerTypesBySender))
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
			_eventHandlerTypesByEventArgsType = _eventHandlerTypesByEventArgsType.SetItem(type, eventHandlerTypesBySender);
		}

		/// <summary>
		/// Создаёт событие с аргументами <paramref name="args" /> от отправителя <paramref name="sender" />.
		/// </summary>
		/// <typeparam name="TEventArgs">Тип аргументов события.</typeparam>
		/// <param name="sender">Отправитель события.</param>
		/// <param name="args">Аргументы события.</param>
		/// <param name="cancellationToken">Токен отмены.</param>
		/// <returns>Задача обработки события.</returns>
		public async Task Raise<TEventArgs>(object sender, TEventArgs args, CancellationToken cancellationToken) where TEventArgs : EventArgs
		{
			IImmutableDictionary<object, IImmutableSet<IEventHandler>> eventHandlersBySender;
			IImmutableSet<IEventHandler> eventHandlers;
			if (_eventHandlersByEventArgsType.TryGetValue(typeof(TEventArgs), out eventHandlersBySender)
				&& ((eventHandlersBySender.TryGetValue(sender, out eventHandlers)
					 || eventHandlersBySender.TryGetValue(_nullSender, out eventHandlers))))
			{
				foreach (IEventHandler<TEventArgs> eventHandler in eventHandlers.Cast<IEventHandler<TEventArgs>>())
				{
					await eventHandler.InvokeAsync(sender, args, cancellationToken);
				}
			}

			IImmutableDictionary<object, IImmutableSet<Type>> eventHandlerTypesBySender;
			IImmutableSet<Type> eventHandlerTypes;
			if (_eventHandlerTypesByEventArgsType.TryGetValue(typeof(TEventArgs), out eventHandlerTypesBySender)
				&& (eventHandlerTypesBySender.TryGetValue(sender, out eventHandlerTypes)
					|| eventHandlerTypesBySender.TryGetValue(_nullSender, out eventHandlerTypes)))
			{
				foreach (Type eventHandlerType in eventHandlerTypes)
				{
					var eventHandler = (IEventHandler<TEventArgs>)_serviceLocator.GetInstance(eventHandlerType);
					await eventHandler.InvokeAsync(sender, args, cancellationToken);
				}
			}
		}

		#endregion
	}
}