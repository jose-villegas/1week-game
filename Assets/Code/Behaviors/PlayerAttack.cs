using Actors;
using Extensions;
using Movement;
using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(Rigidbody), typeof(MovementState))]
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField]
        private PlayerActor _player;
        [SerializeField]
        private Animator _animator;

        private Rigidbody _rigidbody;
        private MovementState _state;
        private float _verticalInput;
        private bool _isAttacking;
        private int _animFlattenSpeed;

        public PlayerAttack(PlayerActor player)
        {
            _player = player;
        }

        // Use this for initialization
        private void Start()
        {
            // without the PlayerActor scriptableobject there
            // is no defined parameters for the player movement
            if (!_player)
            {
                Debug.LogError("No PlayerActor found. Disabling script...");
                enabled = false;
            }

            // neccesary components for the script to work
            this.GetNeededComponent(ref _rigidbody);
            this.GetNeededComponent(ref _state);
            this.GetNeededComponent(ref _animator);
            // get animator parameters ids
            _animFlattenSpeed = Animator.StringToHash("FlattenSpeed");
        }

        private void RecoverFlattenSpeed()
        {
            _animator.SetFloat(_animFlattenSpeed, 1.0f);
        }
	
        // Update is called once per frame
        private void Update()
        {
            _verticalInput = Input.GetAxis("Vertical");
        }

        private void FixedUpdate()
        {
            // to enter attack mode downward force is added on the already falling player
            if (_state.Current(MovementState.States.Falling) && _verticalInput < 0.0f)
            {
                _rigidbody.AddForce(-Vector3.up * _player.AttackVerticalForce);
                _isAttacking = true;
            }
        }

        private void OnCollisionEnter(Collision col)
        {
            if (!_state.Current(MovementState.States.Falling) && _isAttacking)
            {
                _isAttacking = false;
                _animator.SetFloat(_animFlattenSpeed, col.impulse.y * 0.75f);
                HitPushback(col.contacts[0].point, col.impulse.y * _player.PushbackRadiusScale);
                Invoke("RecoverFlattenSpeed", 1.0f);
            }
        }

        private void HitPushback(Vector3 position, float radius)
        {
#if UNITY_EDITOR
            DebugExtension.DebugWireSphere(position, radius, 2);
#endif

            Collider[] colliders = Physics.OverlapSphere(position, radius);

            for(int i = 0; i < colliders.Length; i++)
            {
                var rb = colliders[i].GetComponent<Rigidbody>();

                if (null != rb && colliders[i].transform != transform)
                {
                    // switch physics
                    if(rb.tag == "Enemy")
                    {
                        rb.GetComponent<NavMeshAgent>().enabled = false;
                        rb.isKinematic = false;
                    }

                    rb.AddExplosionForce(_player.PushbackForceRatio * radius, position, radius, _player.UpwardModifier);
                }
            }
        }
    }
}

