using UnityEngine;

namespace Behaviors
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private int _health = 3;

        public int Health
        {
            get { return _health;}

            private set { _health = value; }
        }

        public void ReduceHealth()
        {
            if (Health > 0)
            {
                Health--;
            }
        }
    }
}
