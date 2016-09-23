using Extensions;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Handles a level's spawn zone and its spawn point
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Collider))]
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField]
        private Transform _spawnPoint;

        public static Transform CurrentSpawnPoint { get; private set; }

        private void Start ()
        {
            if (!_spawnPoint)
            {
                _spawnPoint = transform;
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag != "Player") return;

            CurrentSpawnPoint = _spawnPoint;
        }
    }
}
