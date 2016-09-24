using System;
using Entities;
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

        private CollectedCoinsUI _collectedCoins;
        private int _cointCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelController"/> class.
        /// </summary>
        /// <param name="initialSpawn">The initial spawn zone.</param>
        public LevelController(SpawnZone initialSpawn)
        {
            _initialSpawn = initialSpawn;
        }

        private void Start ()
        {
            _collectedCoins = FindObjectOfType<CollectedCoinsUI>();

            if (!_collectedCoins)
            {
                Debug.LogError("No " + typeof(CollectedCoinsUI) + " found. " +
                               "Disabling script...");
                enabled = false;
            }
            else
            {
                var transforms = transform.GetComponentsInChildren<Transform>();
                _cointCount = Array.FindAll(transforms, x => x.tag == "Coin").Length;
                _collectedCoins.Initialize(_cointCount);
            }
        }

        /// <summary>
        /// Restarts this level.
        /// </summary>
        private void RestartLevel()
        {
        }
    }
}
