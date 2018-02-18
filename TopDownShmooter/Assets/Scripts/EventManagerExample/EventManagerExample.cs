using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCenter
{
	public abstract class GameEvent 
	{
		public delegate void Handler (GameEvent e);
	}

	public class EventManager
	{
		private static EventManager _instance;
		public static EventManager instance
		{
			get 
			{
				if (_instance == null) 
				{
					_instance = new EventManager ();
				}

				return instance;
			}

		}

		private Dictionary<System.Type, GameEvent.Handler> _eventTypeToHandlersMap = new Dictionary<System.Type, GameEvent.Handler> ();

		public void Register<EventType> (GameEvent.Handler handler) where EventType: GameEvent
		{
			System.Type t = typeof(EventType);
			if (_eventTypeToHandlersMap.ContainsKey (t)) {
				_eventTypeToHandlersMap [t] += handler;
			} else {
				_eventTypeToHandlersMap [t] = handler;
			}
		}

		public void Unregister<EventType> (GameEvent.Handler handler) where EventType : GameEvent
		{
			System.Type type = typeof(EventType);
			GameEvent.Handler handlers;
			if (_eventTypeToHandlersMap.TryGetValue (type, out handlers)) 
			{
				handlers -= handler;
				if (handlers == null) 
				{
					_eventTypeToHandlersMap.Remove (type);
				}
				else 
				{
					_eventTypeToHandlersMap [type] = handlers;
				}
			}
		}

		public void Fire(GameEvent e)
		{
			//Get the list of handlers for this event type and call each one with the event.
			System.Type type = e.GetType();
			GameEvent.Handler handlers;
			if (_eventTypeToHandlersMap.TryGetValue (type, out handlers)) 
			{
				handlers (e);
			}
		}
	}
}