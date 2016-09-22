using Behaviors.UI;
using UnityEngine;

namespace Behaviors
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private PlayerHeartsUI _heartsInterface;
        [SerializeField]
        private int _health = 3;

        public int Health
        {
            get { return _health;}

            private set { _health = value; }
        }

        public void ReduceHealth()
        {
            if (Health <= 0) return;

            Health--;

            if (null != _heartsInterface)
            {
                _heartsInterface.DissapearHeart(Health);
            }
        }
    }
}
