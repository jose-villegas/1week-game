using Actors;
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
        private float _gunHeight;
        private float _gunDistance;

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

            Vector3 position = _rotatingGun.localPosition;
            // height respective to parent
            _gunHeight = position.y;
            // horizontal distance from parent origin
            position.y = 0;
            _gunDistance = position.magnitude;
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
            var rotation = Quaternion.LookRotation(directionToPlayer, Vector3.up);
            var position = transform.position + directionToPlayer * _gunDistance +
                           Vector3.up * _gunHeight;
            // finally do rotation and translation
            float t = Time.deltaTime * _enemy.AngularSpeed;
            _rotatingGun.rotation = Quaternion.Slerp(_rotatingGun.rotation, rotation, t);
            _rotatingGun.position = Vector3.Lerp(_rotatingGun.position, position, t);
        }
    }
}
