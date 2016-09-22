using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CollectedCoinsUI : MonoBehaviour
    {
        [SerializeField]
        private Text _numberCollected;
        [SerializeField]
        private Text _numberTotal;
        [SerializeField]
        private Transform _levelCoins;

        private int _coinCount;
        private int _coinsCollected;

        // Use this for initialization
        private void Start ()
        {
            _numberCollected.text = 0.ToString();
            _coinCount = _levelCoins.childCount;
            _numberTotal.text = _coinCount.ToString();
        }

        public void ScoreCoin()
        {
            if (_coinCount <= 0) return;

            _coinCount--;
            _coinsCollected++;
            _numberCollected.text = _coinsCollected.ToString();
        }
    }
}
