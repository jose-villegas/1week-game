﻿using System;
using Actors;
using Camera;
using Extensions;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody), typeof(MovementState))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private PlayerActor _player;
        [SerializeField]
        private Animator _animator;

        private Rigidbody _rigidbody;
        private MovementState _state;
        private Vector2 _inputAxis = Vector2.zero;
        private Vector3 _movement;
        private float _upwardTimer;
        private int _animInflate;
        private int _animFlatten;

        public PlayerMovement(PlayerActor player)
        {
            _player = player;
        }

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
            _animInflate = Animator.StringToHash("Inflate");
            _animFlatten = Animator.StringToHash("Flatten");
        }

        private void Update()
        {
            // store input
            _inputAxis.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // inflate player and set the status
            if (Input.GetKeyDown(KeyCode.F))
            {
                _state.ToggleFloating(_player.FloatingTime);
                _animator.SetBool(_animInflate, _state.Current(MovementState.States.Floating));
            }

            // floating state exists for a given time, check if no longer floating to disable animation
            if (!_state.Current(MovementState.States.Floating) &&
                    _animator.GetBool(_animInflate))
            {
                _animator.SetBool(_animInflate, false);
            }

            // reset floating mode timer
            if (_state.Previous(MovementState.States.Floating))
            {
                _upwardTimer = 0.0f;
            }
        }

        private void FixedUpdate()
        {
            _movement = Vector3.zero;
            // on ground movement logic
            HorizontalMovement();
            // jump movement logic
            JumpMovement();
            // floating mode movement logic
            FloatingMovement();
        }

        private void HorizontalMovement()
        {
            // horizontal movement - forward and backwards,
            // check if grounded to avoid air strafing on jump
            if (_state.Current(MovementState.States.Grounded) ||
                    _state.Current(MovementState.States.Falling) &&
                    _state.Previous(MovementState.States.Grounded)
                    && Math.Abs(_inputAxis.x) > Mathf.Epsilon)
            {
                _movement = PlayerCamera.MovementOrientation * _inputAxis.x *
                            _player.MovementSpeed;
            }

            // air strafing happens on falling state
            if (_state.Current(MovementState.States.Falling) &&
                    (_state.Previous(MovementState.States.OnJump)
                     || _state.Previous(MovementState.States.Floating))
                    && Math.Abs(_inputAxis.x) > Mathf.Epsilon)
            {
                _movement = PlayerCamera.MovementOrientation * _inputAxis.x *
                            _player.AirStrafingSpeed;
            }

            // flatten with push down on ground
            if (_state.Current(MovementState.States.Grounded) && _inputAxis.y < 0.0f &&
                    !_animator.GetBool("Flatten"))
            {
                _animator.SetBool(_animFlatten, true);
            }
            else if(_animator.GetBool(_animFlatten) && _inputAxis.y >= 0.0f)
            {
                _animator.SetBool(_animFlatten, false);
            }

            if (_state.Current(MovementState.States.Grounded) ||
                    _state.Current(MovementState.States.Falling))
            {
                _rigidbody.MovePosition(transform.position + _movement * Time.deltaTime);
            }
        }

        private void JumpMovement()
        {
            // jumping has to happen on the ground
            if(_state.Current(MovementState.States.Grounded) && _inputAxis.y > 0.0f)
            {
                // jump direction force
                _rigidbody.AddForce
                (
                    Vector3.right * _movement.x * _player.JumpingForce.x +
                    Vector3.up * _player.JumpingForce.y
                );
            }
        }

        private void FloatingMovement()
        {
            if (!_state.Current(MovementState.States.Floating)) { return; }

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
            if (_state.Current(MovementState.States.Floating) && col.contacts.Length > 0)
            {
                Vector3 colDir = (col.contacts[0].point - transform.position).normalized;

                if (Vector3.Angle(colDir, Vector3.up) < 5.0)
                {
                    CancelInvoke();
                    _state.ToggleFloating(_player.FloatingTime);
                    _animator.SetBool(_animInflate, false);
                    _upwardTimer = 0.0f;
                }
            }
        }
    }
}
