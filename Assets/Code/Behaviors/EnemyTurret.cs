﻿using Actors;
using Extensions;
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
        [SerializeField]
        private GameObject _bulletObject;
        [SerializeField]
        private float _shootForce = 10.0f;
        [SerializeField]
        private float _gunFieldOfView = 60.0f;
        [SerializeField]
        private float _gunFarPlane = 10.0f;

        private Transform _player;
        private ObjectPool _bulletPool;

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

            // create bullet pool
            _bulletPool = ObjectPool.Instance(_bulletObject, 20);
            _bulletPool.Preload();

            // fire rate
            if (_enemy.Speed > 0.0)
            {
                InvokeRepeating("FireBullet", 0.0f, 1.0f / _enemy.Speed);
            }
            else
            {
                Debug.LogError("Enemy speed controls the firing rate, use a value" +
                               "greater than zero.");
            }
        }

        private void Update()
        {
            GunMovement();
        }

        /// <summary>
        /// Fires a bullet towards the player if this is inside the gun view cone
        /// and it's actually visible from the gun point of view
        /// </summary>
        private void FireBullet()
        {
            // check if the player is inside the view cone
            Vector3 src = _rotatingGun.transform.position;
            Vector3 playerDir = (_player.position - src).normalized;

            if (Vector3.Angle(_rotatingGun.forward, playerDir) > _gunFieldOfView)
            {
                return;
            }

            // no verticallity
            playerDir = Vector3.ProjectOnPlane(playerDir, transform.up);
            Ray ray = new Ray(src, playerDir);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, _gunFarPlane)) return;

            if (hit.transform != _player) return;

            // player inside the view cone and visible, proceed to fire bullets
            var go = _bulletPool.Spawn(_rotatingGun.position, _rotatingGun.rotation);
            var rb = go.GetComponent<Rigidbody>();

            // push bullet forward with shoot force
            if (null != rb) { rb.AddForce(_rotatingGun.forward * _shootForce); }
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

        /// <summary>
        /// Draws the gun view cone area
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.matrix = Matrix4x4.TRS(_rotatingGun.position, _rotatingGun.rotation,
                                          Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, _gunFieldOfView, 0.1f, _gunFarPlane, 1);
        }

        private void OnCollisionEnter(Collision col)
        {
            if (!col.gameObject.CompareTag("Player")) return;

            EventManager.TriggerEvent("HitPlayer");
        }
    }
}
