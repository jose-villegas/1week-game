using UnityEngine;

namespace General
{
    public class PoolMember : MonoBehaviour
    {
        public ObjectPool Pool { get; set; }

        public void Despawn()
        {
            if(null != Pool) Pool.Despawn(gameObject);
        }

        public void Spawn()
        {
            if (null != Pool) Pool.Spawn(transform.position, transform.rotation,
                                             transform.localScale);
        }

        /// <summary>
        /// Called by <see cref="ObjectPool"/> on respawn
        /// </summary>
        public virtual void OnSpawn() {}

        /// <summary>
        /// Called by <see cref="ObjectPool"/> on despawn
        /// </summary>
        public virtual void OnDespawn() {}
    }
}
