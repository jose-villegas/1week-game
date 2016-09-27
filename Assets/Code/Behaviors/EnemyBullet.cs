using Extensions;
using General;
using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(Collider))]
    public class EnemyBullet : PoolMember
    {
        private Rigidbody _rigidbody;
        private Collider _collider;
        private TrailRenderer _trailRenderer;
        private Vector3 _savedAngularVelocity;
        private Vector3 _savedVelocity;

        private void Awake()
        {
            this.GetNeededComponent(ref _rigidbody);
            this.GetNeededComponent(ref _collider);
            this.GetNeededComponent(ref _trailRenderer);
            // subscribe to game pause events
            EventManager.StartListening("GamePaused", OnGamePaused);
            EventManager.StartListening("GameResumed", OnGameResumed);
        }

        private void OnGameResumed()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddTorque(_savedAngularVelocity, ForceMode.VelocityChange);
            _rigidbody.AddForce(_savedVelocity, ForceMode.VelocityChange);
        }

        private void OnGamePaused()
        {
            _savedAngularVelocity = _rigidbody.angularVelocity;
            _savedVelocity = _rigidbody.velocity;
            _rigidbody.isKinematic = true;
        }

        public override void OnDespawn()
        {
            _collider.enabled = false;
        }

        public override void OnSpawn()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _trailRenderer.Clear();
            _collider.enabled = true;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag("Player")) return;

            EventManager.TriggerEvent("HitPlayer");
            // despawn from pool
            Despawn();
        }
    }
}
