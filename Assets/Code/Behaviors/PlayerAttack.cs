using Actors;
using Entities;
using Extensions;
using General;
using Interfaces;
using Movement;
using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(Rigidbody), typeof(MovementState))]
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        private PlayerActor _player;
        private Rigidbody _rigidbody;
        private MovementState _state;
        private float _verticalInput;
        private bool _isAttacking;
        private int _animFlattenSpeed;
        private int _enemiesMask;

        public PlayerAttack(PlayerActor player)
        {
            _player = player;
        }

        private void Start()
        {
            _player = GameSettings.Instance.PlayerSettings;

            // without the PlayerActor scriptableobject there
            // is no defined parameters for the player movement
            if (!_player)
            {
                Debug.LogError("No " + typeof(PlayerActor) + " found. " +
                               "Disabling script...");
                enabled = false;
            }

            // neccesary components for the script to work
            this.GetNeededComponent(ref _rigidbody);
            this.GetNeededComponent(ref _state);
            this.GetNeededComponent(ref _animator);
            // get animator parameters ids
            _animFlattenSpeed = Animator.StringToHash("FlattenSpeed");
            // prefetch enemies mask
            _enemiesMask = LayerMask.GetMask("Enemies");
        }

        private void RecoverFlattenSpeed()
        {
            _animator.SetFloat(_animFlattenSpeed, 1.0f);
        }

        private void Update()
        {
            _verticalInput = Input.GetAxis("Vertical");
        }

        private void FixedUpdate()
        {
            // to enter attack mode downward force is added on the already falling player
            if (_state.IsCurrent(MovementState.States.Falling) && _verticalInput < 0.0f)
            {
                _rigidbody.AddForce(-Vector3.up * _player.AttackVerticalForce);
                _isAttacking = true;
            }
        }

        private void OnCollisionEnter(Collision col)
        {
            // hit the ground on falling mode
            if (!_state.IsCurrent(MovementState.States.Falling) && _isAttacking)
            {
                _isAttacking = false;
                // animate flatten faster to give a sense of force push
                _animator.SetFloat(_animFlattenSpeed, col.impulse.y * 0.75f);
                // radius scan pushback area
                HitPushback(col.contacts[0].point, col.impulse.y * _player.PushbackRadiusScale);
                // recover from flatten speed
                Invoke("RecoverFlattenSpeed", 1.0f);
            }
        }

        /// <summary>
        /// Checks for nearby enemies and hits those that are inside the
        /// sphere radius, also adds push back force from the sphere origin
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="radius">The radius.</param>
        private void HitPushback(Vector3 position, float radius)
        {
            #if UNITY_EDITOR
            DebugExtension.DebugWireSphere(position, radius, 2);
            #endif
            Collider[] colliders = Physics.OverlapSphere(position, radius, _enemiesMask);

            for(int i = 0; i < colliders.Length; i++)
            {
                var rb = colliders[i].GetComponent<Rigidbody>();

                if (null != rb && colliders[i].transform != transform)
                {
                    // if enemy and hittable, register a hit
                    if(rb.tag == "Enemy")
                    {
                        var enemy = rb.GetComponent(typeof(IHittable)) as IHittable;

                        if (enemy != null) enemy.Hit();
                    }

                    rb.AddExplosionForce(_player.PushbackForceRatio * radius, position,
                                         radius, _player.UpwardModifier);
                }
            }
        }
    }
}

