using System;
using Entities;
using Interfaces;
using UI;
using UnityEngine;

namespace General
{
    /// <summary>
    /// Handles a level
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class LevelController : MonoBehaviour
    {
        [SerializeField]
        private SpawnZone _initialSpawn;

        private IRestartable[] _restartables;

        private static Transform _player;
        private static CollectedCoinsUI _collectedCoins;

        public int CointCount { get; private set; }

        public static LevelController ActiveLevel { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelController"/> class.
        /// </summary>
        /// <param name="initialSpawn">The initial spawn zone.</param>
        public LevelController(SpawnZone initialSpawn)
        {
            _initialSpawn = initialSpawn;
        }

        private void OnEnable()
        {
            ActiveLevel = this;
        }

        private void Start ()
        {
            if (null == _initialSpawn)
            {
                Debug.LogError("No initial " + typeof(SpawnZone) + " given. " +
                               "Disabling script...");
                enabled = false;
                return;
            }

            // find coins interface
            if (null == _collectedCoins)
            {
                _collectedCoins = FindObjectOfType<CollectedCoinsUI>();

                if (null == _collectedCoins)
                {
                    Debug.LogError("No " + typeof(CollectedCoinsUI) + " found. " +
                                   "Disabling script...");
                    enabled = false;
                    return;
                }
            }

            // find player transform
            if (null == _player)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                if (null == player)
                {
                    Debug.LogError("No player transform found, use the Player tag");
                    enabled = false;
                    return;
                }

                _player = player.transform;
            }

            // get coin count and initialize coin interface
            var transforms = GetComponentsInChildren<Transform>();
            CointCount = Array.FindAll(transforms, x => x.tag == "Coin").Length;
            _collectedCoins.Restart();
            // find restartables for level reset
            _restartables = GetComponentsInChildren<IRestartable>();
        }

        /// <summary>
        /// Restarts this level.
        /// </summary>
        private void RestartLevel()
        {
            if (!enabled) return;

            // call all level restart actions
            for (int i = 0; i < _restartables.Length; i++)
            {
                _restartables[i].Restart();
            }

            // restore position from initial spawn
            _player.position = _initialSpawn.SpawnPoint.position;
            // call non-level related restart level actions
            EventManager.TriggerEvent("LevelReset");
        }
    }
}
