using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace General
{
    /// <summary>
    /// Messaging system that enables other objects to subscribe to events
    /// or trigger them
    /// </summary>
    /// <seealso cref="EventManager" />
    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<string, UnityEvent> _eventDictionary;

        public void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            // initialize dictionary
            _eventDictionary = new Dictionary<string, UnityEvent>();
        }

        /// <summary>
        /// Subscribes to the given event, if it doesn't exist it's created
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="listener">The listener function.</param>
        public static void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent;

            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance._eventDictionary.Add(eventName, thisEvent);
            }
        }

        /// <summary>
        /// Unsubscribes from the given event.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="listener">The listener function.</param>
        public static void StopListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent;

            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        /// <summary>
        /// Triggers the given event.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        public static void TriggerEvent(string eventName)
        {
            UnityEvent thisEvent;

            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }
    }
}
