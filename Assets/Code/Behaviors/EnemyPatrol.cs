using Extensions;
using Interfaces;
using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class EnemyPatrol : MonoBehaviour, IHittable
    {
        [SerializeField]
        private Transform[] _points;
        [SerializeField]
        private float _minimumDistance = 0.5f;

        private Rigidbody _rigidbody;
        private NavMeshAgent _agent;
        private int _targetPoint;
        private PlayerHealth _playerHealth = null;

        public EnemyPatrol(Transform[] points)
        {
            _points = points;
        }

        // Use this for initialization
        private void Start()
        {
            // neccesary components for the script to work
            this.GetNeededComponent(ref _rigidbody);
            this.GetNeededComponent(ref _agent);
        }

        private void Update()
        {
            if (!_agent.enabled)
            {
                return;
            }

            // move to next point once the actor is close enough
            if (_agent.remainingDistance < _minimumDistance)
            {
                GoToNextPoint();
            }
        }

        /// <summary>
        /// Action is to disable agent and
        /// enable physics simulation
        /// </summary>
        public void Hit()
        {
            _agent.enabled = false;
            _rigidbody.isKinematic = false;
            enabled = false;
        }

        private void GoToNextPoint()
        {
            if (null == _points || _points.Length == 0)
            {
                return;
            }

            // move to current target
            _agent.SetDestination(_points[_targetPoint].position);
            // increment position to next target
            _targetPoint = (_targetPoint + 1) % _points.Length;
        }

        private void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.tag != "Player") return;

            if (null == _playerHealth)
            {
                _playerHealth = col.transform.GetComponent<PlayerHealth>();
            }

            if (null == _playerHealth) return;

            _playerHealth.Hit();
            _playerHealth.TemporalImmunity();
        }

        public void OnDrawGizmos()
        {
            if (null == _points || _points.Length == 0) return;

            if (null == _points[0]) return;

            Vector3 prevPosition = _points[0].position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(prevPosition, 0.2f);

            for (int i = 1; i < _points.Length; i++)
            {
                if (null == _points[i]) return;

                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_points[i].position, 0.2f);
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(prevPosition, _points[i].position);
                // middle icon
                Gizmos.DrawIcon((prevPosition + _points[i].position) / 2.0f, "footsteps");
                // update previous point
                prevPosition = _points[i].position;
            }
        }
    }
}

