using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace General
{
    /// <summary>
    /// Messaging system that enables other objects to subscribe to events
    /// or trigger them
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class EventManager : MonoBehaviour
    {
        private Dictionary<string, UnityEvent> _eventDictionary;

        private static EventManager _instance;
        private static object _lock = new object();
        private static bool _applicationQuitting;

        public static EventManager Instance
        {
            get
            {
                // instance has been already destroyed, don't create more
                if (_applicationQuitting)
                {
                    return null;
                }

                lock (_lock)
                {
                    if (null != _instance) return _instance;

                    _instance = FindObjectOfType<EventManager>();

                    if (FindObjectsOfType<EventManager>().Length > 1)
                    {
                        Debug.LogError("There shouldn't be more than one " +
                                       typeof(EventManager) + " component in scene");
                        return _instance;
                    }

                    if (_instance != null) return _instance;

                    GameObject eventManager = new GameObject();
                    _instance = eventManager.AddComponent<EventManager>();
                    eventManager.name = typeof(EventManager).ToString();
                    return _instance;
                }
            }
        }

        public void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            // initialize dictionary
            _eventDictionary = new Dictionary<string, UnityEvent>();
        }

        public void OnDestroy()
        {
            _applicationQuitting = true;
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
            if (_instance == null) return;

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
