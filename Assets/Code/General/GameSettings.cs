using Actors;
using UnityEngine;

namespace General
{
    public class GameSettings : MonoBehaviour
    {
        private static GameSettings _instance;
        private static object _lock = new object();
        private static bool _applicationQuitting;

        public static GameSettings Instance
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

                    _instance = FindObjectOfType<GameSettings>();

                    if (FindObjectsOfType<GameSettings>().Length > 1)
                    {
                        Debug.LogError("There shouldn't be more than one " +
                                       typeof(GameSettings) + " component in scene");
                        return _instance;
                    }

                    if (_instance != null) return _instance;

                    GameObject gameSettings = new GameObject();
                    _instance = gameSettings.AddComponent<GameSettings>();
                    gameSettings.name = typeof(GameSettings).ToString();
                    return _instance;
                }
            }
        }

        public PlayerActor PlayerSettings { get; private set; }

        public void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            // query player scriptableObject asset
            PlayerSettings = Resources.Load("Player") as PlayerActor;
        }

        public void OnDestroy()
        {
            _applicationQuitting = true;
        }
    }
}
