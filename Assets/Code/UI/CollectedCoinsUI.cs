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
        [SerializeField]
        private Animator _scoreAnimator;
        [SerializeField]
        private Text _score;

        private int _coinCount;
        private int _coinsCollected;
        private int _appearAnimation;
        private int _totalCollected;

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

            // prefetch animation
            if (null == _scoreAnimator || null == _score) return;

            _appearAnimation = Animator.StringToHash("Appear");
            // on start over show score and reset total collected
            EventManager.StartListening("StartOver", () =>
            {
                _score.text = _totalCollected.ToString();
                _totalCollected = 0;
                _scoreAnimator.SetBool(_appearAnimation, true);
            });
            // add to the total count the number of coins from the previous level
            EventManager.StartListening("GoToNextLevel", () =>
            {
                _totalCollected += _coinsCollected;
            });
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
            // level reset will also happen after StartOver, hide score
            _scoreAnimator.SetBool(_appearAnimation, false);
        }
    }
}
