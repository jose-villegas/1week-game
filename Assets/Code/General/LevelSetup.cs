using System;
using UI;
using UnityEngine;

namespace General
{
    /// <summary>
    /// Handles the configuration and startup of a level
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class LevelSetup : MonoBehaviour
    {
        private CollectedCoinsUI _collectedCoins;

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
                int coinCount = Array.FindAll(transforms, x => x.tag == "Coin").Length;
                _collectedCoins.Initialize(coinCount);
            }
        }
    }
}
