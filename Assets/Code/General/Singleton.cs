using UnityEngine;

namespace General
{
    /// <summary>
    /// Singleton pattern with Unity, object persists between scenes
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static object _lock = new object();

        public static T Instance
        {
            get
            {
                if (_applicationIsQuitting)
                {
                    Debug.LogWarning("Instance of " + typeof(T) + " already " +
                                     "destroyed on application quit.");
                    return null;
                }

                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = (T)FindObjectOfType(typeof(T));

                        if (FindObjectsOfType(typeof(T)).Length > 1)
                        {
                            Debug.LogError("There shouldn't be more than one " +
                                           typeof(T) + " component in scene.");
                            return _instance;
                        }

                        if (_instance == null)
                        {
                            GameObject singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                            singleton.name = typeof(T).ToString();
                            DontDestroyOnLoad(singleton);
                        }
                    }

                    return _instance;
                }
            }
        }

        private static bool _applicationIsQuitting;

        /// <summary>
        /// This prevents the component from leaking if <see cref="Instance"/>
        /// is called after the application has ended.
        /// </summary>
        public void OnDestroy()
        {
            _applicationIsQuitting = true;
        }
    }
}
