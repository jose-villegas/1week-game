using UnityEngine;
using UnityEngine.Events;

namespace Bases
{
    public class EnemyBase : MonoBehaviour, IEnemy
    {
        public UnityEvent onMovementEvent;
        public UnityEvent onKillEvent;

        /// <summary>
        /// Execute event on movement call
        /// </summary>
        public virtual void Movement()
        {
            if (null != onMovementEvent)
            {
                onMovementEvent.Invoke();
            }
        }
        
        /// <summary>
        /// Execute event on dying call
        /// </summary>
        public virtual void Kill()
        {
            if (null != onKillEvent)
            {
                onKillEvent.Invoke();
            } 
        }
    }
}

