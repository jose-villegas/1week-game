using Extensions;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(Collider))]
    public class SpawnZone : MonoBehaviour
    {
        [SerializeField]
        private Transform _spawnPoint;

        private Collider _collider;

        public static Transform CurrentSpawnPoint { get; private set; }

        public SpawnZone(Collider collider, Transform spawnPoint)
        {
            _collider = collider;
            _spawnPoint = spawnPoint;
        }

        private void Start ()
        {
            if (!_spawnPoint)
            {
                _spawnPoint = transform;
            }

            this.GetNeededComponent(ref _collider);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag != "Player") return;

            CurrentSpawnPoint = _spawnPoint;
        }
    }
}
