using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventSystemAJP 
{
	//Making Game events. Handlers are delegates that hold Game Events.
	public abstract class GameEvent 
	{
		public delegate void Handler (GameEvent e);
	}

	public class EventManager 
	{
		//Make it a singleton
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

		//A dictionairy that holds our handlers.  It takes in a type and returns the handler that handles that type
		private Dictionary<System.Type, GameEvent.Handler> eventMap = new Dictionary<System.Type, GameEvent.Handler> ();

		//Registering for an event.  Adds a specific instance of a method to the appropriate handler.
		public void Register<EventType> (GameEvent.Handler handler) where EventType: GameEvent // Gets passed a GameEvent (what is the event) and a method (what to do when it fires).
		{
			System.Type t = typeof(EventType); //The particular type of of the passed GameEvent.
			if (eventMap.ContainsKey (t)) 
			{
				eventMap [t] += handler; // If the dictionairy already has a log for that type of event, add the method to that handler.
			} else {
				eventMap [t] = handler; // If the dictionary does not have a reference to the passed event, make an entry for it.
			}
		}

		public void Unregister<EventType> (GameEvent.Handler handler) where EventType : GameEvent
		{
			System.Type t = typeof(EventType);
			GameEvent.Handler handlers;  //A temporary reference to the handler we're unregistering.
			if (eventMap.TryGetValue (t, out handlers)) //if the handler exists...
			{
				handlers -= handler; //...remove the passed method.
				if (handlers == null) // If there are no more methods left in that handler
				{
					eventMap.Remove (t); // Remove the reference to the handler.
				}
				else 
				{
					eventMap [t] = handlers; // Else, the handler is updated to reflect the fewer handlers.
				}
			}
		}

		public void Fire(GameEvent e)
		{
			//Look up the handler and call the delegate.
			System.Type t = e.GetType();
			GameEvent.Handler handlers;
			if (eventMap.TryGetValue (t, out handlers)) 
			{
				handlers (e);
			}
		}
	}
}