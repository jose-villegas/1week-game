using System;
using UI;
using UnityEngine;

namespace General
{
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
