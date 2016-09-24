using Actors;
using UnityEngine;

namespace General
{
    /// <summary>
    /// Handles the gameplay parameters
    /// </summary>
    /// <seealso cref="GameSettings" />
    public class GameSettings : Singleton<GameSettings>
    {
        public PlayerActor PlayerSettings { get; private set; }

        public void Awake()
        {
            PlayerSettings = Resources.Load("Player") as PlayerActor;
        }
    }
}
