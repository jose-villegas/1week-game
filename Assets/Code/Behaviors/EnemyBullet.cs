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
        private GameplaySettings _gameplay;

        private void Awake()
        {
            _gameplay = GameSettings.Instance.GameplaySettings;
            // get neccesary components for this script to work
            this.GetNeededComponent(ref _rigidbody);
            this.GetNeededComponent(ref _collider);
            this.GetNeededComponent(ref _trailRenderer);
            // subscribe to game pause events
            EventManager.StartListening("GamePaused", OnGamePaused);
            EventManager.StartListening("GameResumed", OnGameResumed);
        }

        private void OnGameResumed()
        {
            if (null == _rigidbody) return;

            _rigidbody.isKinematic = false;
            _rigidbody.AddTorque(_savedAngularVelocity, ForceMode.VelocityChange);
            _rigidbody.AddForce(_savedVelocity, ForceMode.VelocityChange);
        }

        private void OnGamePaused()
        {
            if (null == _rigidbody) return;

            _savedAngularVelocity = _rigidbody.angularVelocity;
            _savedVelocity = _rigidbody.velocity;
            _rigidbody.isKinematic = true;
        }

        public override void OnDespawn()
        {
            if (_collider == null) return;

            _collider.enabled = false;
        }

        public override void OnSpawn()
        {
            if (null == _rigidbody || null == _trailRenderer || null == _collider) return;

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _trailRenderer.Clear();
            _collider.enabled = true;
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag("Player")) return;

            for (int i = 0; i < _gameplay.ProjectileDamage; i++)
            {
                EventManager.TriggerEvent("HitPlayer");
            }

            // despawn from pool
            Despawn();
        }
    }
}
