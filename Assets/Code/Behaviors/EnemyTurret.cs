using Actors;
using General;
using UnityEngine;

namespace Behaviors
{
    /// <summary>
    /// Enemy behavior for turrets, points at the player and fires projectiles
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyTurret : MonoBehaviour
    {
        [SerializeField]
        private DynamicActor _enemy;
        [SerializeField]
        private Transform _rotatingGun;

        private Transform _player;

        private void Start ()
        {
            if (!_enemy)
            {
                Debug.LogError("No " + typeof(DynamicActor) + " found. " +
                               "Disabling script...");
                enabled = false;
            }

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (null == player)
            {
                Debug.LogError("No player transform found, use the Player tag");
                enabled = false;
            }
            else
            {
                _player = player.transform;
            }

            InvokeRepeating("CreateSomething", 0, 1);
        }

        private void CreateSomething()
        {
            ObjectPool.Instance(_rotatingGun.gameObject, 10).Spawn();
        }

        private void Update()
        {
            GunMovement();
        }

        /// <summary>
        /// Handles translation and rotation of the <see cref="_rotatingGun"/>
        /// to point towards the player
        /// </summary>
        private void GunMovement()
        {
            Vector3 directionToPlayer = _player.position - transform.position;
            // cancel any verticality, rotate only from x and z
            directionToPlayer.y = 0.0f;
            directionToPlayer = directionToPlayer.normalized;
            // look at player rotation
            float angle = Vector3.Angle(_rotatingGun.forward, directionToPlayer);

            if (Vector3.Cross(_rotatingGun.forward, directionToPlayer).y < 0)
            {
                angle = -angle;
            }

            angle = angle * Time.deltaTime * _enemy.AngularSpeed;
            _rotatingGun.RotateAround(transform.position, transform.up, angle);
        }
    }
}
