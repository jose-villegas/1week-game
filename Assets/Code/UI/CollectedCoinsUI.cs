using General;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Handles the interface for the player's number of total and collected coins
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class CollectedCoinsUI : MonoBehaviour, IRestartable
    {
        [SerializeField]
        private Text _numberCollected;
        [SerializeField]
        private Text _numberTotal;

        private int _coinCount;
        private int _coinsCollected;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectedCoinsUI"/> class.
        /// </summary>
        /// <param name="numberCollected">The text for number collected.</param>
        /// <param name="numberTotal">The text for number total.</param>
        public CollectedCoinsUI(Text numberCollected, Text numberTotal)
        {
            _numberCollected = numberCollected;
            _numberTotal = numberTotal;
        }

        private void Start()
        {
            // updates interface when a coin is collected
            EventManager.StartListening("CoinCollected", ScoreCoin);
            // restarts ui on level reset
            EventManager.StartListening("LevelReset", Restart);
            // restarts on level begin
            EventManager.StartListening("LevelBegin", Restart);
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

        /// <summary>
        /// Resets to zero de number of coins collected and
        /// sets the coin count to the number of coins in
        /// <see cref="LevelController.ActiveLevel"/>
        /// </summary>
        public void Restart()
        {
            _coinsCollected = 0;
            _numberCollected.text = _coinsCollected.ToString();
            _coinCount = LevelController.ActiveLevel.CointCount;
            _numberTotal.text = _coinCount.ToString();
        }
    }
}
