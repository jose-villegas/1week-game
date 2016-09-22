using UI;
using UnityEngine;

namespace Behaviors
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private PlayerHeartsUI _heartsInterface;
        [SerializeField]
        private int _health = 3;
        [SerializeField]
        private float _immunityTime = 3.0f;

        private bool _immunityActive;

        public int Health
        {
            get { return _health;}

            private set { _health = value; }
        }

        public void ReduceHealth()
        {
            if (Health <= 0 || _immunityActive) return;

            Health--;

            if (null != _heartsInterface)
            {
                _heartsInterface.DissapearHeart(Health);
            }
        }

        public void TemporalImmunity()
        {
            _immunityActive = true;
            CancelInvoke("DisableImmunity");
            Invoke("DisableImmunity", _immunityTime);
        }

        private void DisableImmunity()
        {
            _immunityActive = false;
        }
    }
}
