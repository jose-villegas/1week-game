﻿using System;
using Actors;
using Behaviors;
using Extensions;
using General;
using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Handles the player's movement input and logic
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Rigidbody), typeof(PlayerMovementState))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Collider _modelCollider;

        private PlayerActor _player;
        private GameplaySettings _gameplay;
        private Rigidbody _rigidbody;
        private PlayerMovementState _state;
        private Vector2 _inputAxis = Vector2.zero;
        private Vector3 _movement;
        private float _upwardTimer;
        private int _animInflate;
        private int _animFlatten;
        private Vector3 _savedVelocity;
        private Vector3 _savedAngularVelocity;

        private void Start()
        {
            _player = GameSettings.Instance.PlayerSettings;
            _gameplay = GameSettings.Instance.GameplaySettings;

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
            this.GetNeededComponent(ref _modelCollider);
            // prefetch animator parameters ids
            _animInflate = Animator.StringToHash("Inflate");
            _animFlatten = Animator.StringToHash("Flatten");
            // subscribe to player death event to stop movement
            EventManager.StartListening("PlayerDied", () => { enabled = false; });
            // subscribe to level reset to start movement again
            EventManager.StartListening("LevelReset", () => { enabled = true; });
            // subscribe to game pause events
            EventManager.StartListening("GamePaused", OnGamePaused);
            EventManager.StartListening("GameResumed", OnGameResumed);
        }

        private void OnGameResumed()
        {
            _rigidbody.isKinematic = false;
            _rigidbody.AddTorque(_savedAngularVelocity, ForceMode.VelocityChange);
            _rigidbody.AddForce(_savedVelocity, ForceMode.VelocityChange);
            enabled = true;
        }

        private void OnGamePaused()
        {
            _savedAngularVelocity = _rigidbody.angularVelocity;
            _savedVelocity = _rigidbody.velocity;
            _rigidbody.isKinematic = true;
            enabled = false;
        }

        private void Update()
        {
            // store input
            _inputAxis.Set(Input.GetAxis("Horizontal"), Input.GetAxisRaw("Vertical"));
            // handle transform orientation for movement direction
            Orientate();

            // inflate player and set the status
            if (Input.GetKeyDown(KeyCode.F))
            {
                _state.ToggleFloating();
            }

            bool isFloating = _state.Is(PlayerMovementState.States.Floating);
            _animator.SetBool(_animInflate, isFloating);

            // reset floating mode timer
            if (_state.Was(PlayerMovementState.States.Floating))
            {
                _upwardTimer = 0.0f;
            }
        }

        /// <summary>
        /// Handles the transform orientation depending
        /// on <see cref="PlayerCamera.MovementOrientation"/>
        /// </summary>
        private void Orientate()
        {
            // look to orientation
            if (_state.Is(PlayerMovementState.States.Grounded))
            {
                Vector3 orientation = PlayerCamera.MovementOrientation;
                // transform rotation extract
                Vector3 up;

                if (!_modelCollider.GroundNormal(out up) || up == Vector3.zero) return;

                Vector3 right = Quaternion.AngleAxis(-90, Vector3.up) * orientation;
                Vector3 forward = Vector3.Cross(up, right);
                // final rotation, oriented with ground normal and proper forward
                Quaternion rotation = Quaternion.LookRotation(forward, up);
                // lerp with angular speed
                rotation = Quaternion.Lerp(transform.rotation, rotation,
                                           Time.deltaTime * _player.AngularSpeed);
                transform.rotation = rotation;
            }
        }

        private void FixedUpdate()
        {
            _movement = Vector3.zero;
            // horizontal on movement orientation logic
            HorizontalMovement();
            // flatten state logic
            FlattenMovement();
            // jump movement logic
            JumpMovement();
            // floating mode movement logic
            FloatingMovement();
        }

        /// <summary>
        /// Handles the horizontal movement depending on the current
        /// <see cref="PlayerCamera.MovementOrientation"/>
        /// </summary>
        private void HorizontalMovement()
        {
            // for on ground movement, first it has to be ground
            bool movementCondition = _state.Is(PlayerMovementState.States.Grounded);
            // falling off slopes or platforms condition; same speed as on ground
            movementCondition |= _state.Is(PlayerMovementState.States.Falling) &&
                                 _state.Was(PlayerMovementState.States.Grounded);

            if (movementCondition && Math.Abs(_inputAxis.x) > Mathf.Epsilon)
            {
                _movement = PlayerCamera.MovementOrientation * _inputAxis.x *
                            _player.Speed;
            }

            // for air strafing horizontal movement; only enabled on falling mode
            movementCondition = _state.Is(PlayerMovementState.States.Falling);
            // previous state cannot be on ground; this follows normal speed
            movementCondition &= !_state.Was(PlayerMovementState.States.Grounded);

            if (movementCondition && Math.Abs(_inputAxis.x) > Mathf.Epsilon)
            {
                _movement = PlayerCamera.MovementOrientation * _inputAxis.x *
                            _player.AirStrafingSpeed;
            }

            _movement *= _gameplay.PlayerSpeedMultiplier;
            _rigidbody.MovePosition(transform.position + _movement * Time.deltaTime);
        }

        /// <summary>
        /// Handles the "flatten" state, smaller collider for small corridors
        /// </summary>
        private void FlattenMovement()
        {
            // squashed between two colliders, keep the flatten animation
            bool isSquashed = _animator.GetBool(_animFlatten) &&
                              _modelCollider.IsSquashed(_state.GroundLayerMask);

            // flatten with push down on ground
            if (isSquashed || _state.Is(PlayerMovementState.States.Grounded) &&
                    _inputAxis.y < 0.0f && !_animator.GetBool(_animFlatten))
            {
                _animator.SetBool(_animFlatten, true);
            }
            else if (_animator.GetBool(_animFlatten) && _inputAxis.y >= 0.0f)
            {
                _animator.SetBool(_animFlatten, false);
            }
        }

        /// <summary>
        /// Adds the jumping vertical force on input
        /// </summary>
        private void JumpMovement()
        {
            // jumping has to happen on the ground
            if(_state.Is(PlayerMovementState.States.Grounded) && _inputAxis.y > 0.0f)
            {
                // jump direction force
                _rigidbody.AddForce(_movement * _player.JumpForce.x + Vector3.up *
                                    _player.JumpForce.y, ForceMode.Impulse);
            }
        }

        /// <summary>
        /// Horizontal and vertical movement on floating mode
        /// </summary>
        private void FloatingMovement()
        {
            if (!_state.Is(PlayerMovementState.States.Floating)) { return; }

            Vector3 fForce = Vector3.up * _player.FloatingForce.y;

            // floating upward force
            if(_upwardTimer <  _player.UpwardBuildupTime)
            {
                float t = _upwardTimer / _player.UpwardBuildupTime;
                fForce.y = Mathf.Lerp(0.0f, _player.FloatingForce.y, t);
                _upwardTimer = _upwardTimer + Time.fixedDeltaTime;
            }

            // floating horizontal push
            if(Math.Abs(_inputAxis.x) > Mathf.Epsilon)
            {
                fForce += PlayerCamera.MovementOrientation * _inputAxis.x *
                          _player.FloatingForce.x;
            }

            _rigidbody.AddForce(fForce);
        }

        private void OnCollisionEnter(Collision col)
        {
            // collided with something while floating
            if (_state.Is(PlayerMovementState.States.Floating) &&
                    col.contacts.Length > 0 && !col.collider.isTrigger)
            {
                Vector3 collisionDir = col.contacts[0].normal;

                // checks whether we collided with somethong -above- the player,
                // 5 degree cone area check, stop floating mode if that's the case
                if (Vector3.Angle(collisionDir, -Vector3.up) < 10.0)
                {
                    CancelInvoke();
                    _state.ToggleFloating();
                    _animator.SetBool(_animInflate, false);
                    _upwardTimer = 0.0f;
                }
            }
        }
    }
}
