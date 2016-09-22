using Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Bases
{
    public abstract class EnemyBase : MonoBehaviour, IEnemy
    {
        public UnityEvent OnMovementEvent;
        public UnityEvent OnKillEvent;

        /// <summary>
        /// Execute event on movement call
        /// </summary>
        public virtual void Movement()
        {
            if (null != OnMovementEvent)
            {
                OnMovementEvent.Invoke();
            }
        }

        /// <summary>
        /// Execute event on dying call
        /// </summary>
        public virtual void Kill()
        {
            if (null != OnKillEvent)
            {
                OnKillEvent.Invoke();
            }
        }
    }
}

