using General;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Handles the interface for the player's number of total and collected coins
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class CollectedCoinsUI : MonoBehaviour
    {
        [SerializeField]
        private Text _numberCollected;
        [SerializeField]
        private Text _numberTotal;

        private int _coinCount;
        private int _coinsCollected;

        private void Start()
        {
            EventManager.StartListening("CoinCollected", ScoreCoin);
        }

        /// <summary>
        /// Resets to zero de number of coins collected and sets the
        /// coin count to the number of children of the given transform
        /// </summary>
        /// <param name="numberOfCoins">The number of coins.</param>
        public void Initialize(int numberOfCoins)
        {
            _coinsCollected = 0;
            _numberCollected.text = _coinsCollected.ToString();
            _coinCount = numberOfCoins;
            _numberTotal.text = _coinCount.ToString();
        }

        /// <summary>
        /// Adds to the number of coins collected and updates the interface
        /// respectively
        /// </summary>
        public void ScoreCoin()
        {
            if (_coinCount <= 0) return;

            _coinCount--;
            _coinsCollected++;
            _numberCollected.text = _coinsCollected.ToString();
        }
    }
}
