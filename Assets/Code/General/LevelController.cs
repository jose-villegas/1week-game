using System;
using Entities;
using Extensions;
using Interfaces;
using UnityEngine;

namespace General
{
    /// <summary>
    /// Handles a level, its components and active status
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class LevelController : MonoBehaviour, IRestartable
    {
        [SerializeField]
        private SpawnZone _initialSpawn;

        private IRestartable[] _restartables;

        private static Transform _player;

        public int CointCount { get; private set; }

        public static LevelController ActiveLevel { get; private set; }

        /// <summary>
        /// Initializes a <see cref="LevelController"/> class.
        /// </summary>
        /// <param name="initialSpawn">The initial spawn zone.</param>
        public void Initialize(SpawnZone initialSpawn)
        {
            _initialSpawn = initialSpawn;
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
            // find restartables for level reset
            _restartables = GetComponentsInChildren<IRestartable>();
            // trigger level begin event for other scripts to handle
            EventManager.TriggerEvent("LevelBegin");
            // on level begin set the player to starting spawn point
            _player.position = _initialSpawn.SpawnPoint.position;
        }

        private void OnEnable()
        {
            ActiveLevel = this;
        }

        /// <summary>
        /// Restarts this level.
        /// </summary>
        public void Restart()
        {
            if (!enabled) return;

            // call all level restart actions
            for (int i = 0; i < _restartables.Length; i++)
            {
                // avoid calling restart on self, infinite recursion
                if (ReferenceEquals(_restartables[i], this)) continue;

                _restartables[i].Restart();
            }

            // restore position from initial spawn
            _player.position = _initialSpawn.SpawnPoint.position;
            // call non-level related restart level actions
            EventManager.TriggerEvent("LevelReset");
        }
    }
}
