﻿using System.Collections;
using Actors;
using Extensions;
using General;
using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Handles the movement state of this object
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementState : MonoBehaviour
    {
        public enum States
        {
            Grounded,
            OnJump,
            Falling,
            Floating
        }

        [SerializeField]
        private States _state;
        [SerializeField]
        private States _previousState;
        [SerializeField]
        private Collider _modelCollider;
        [SerializeField]
        private LayerMask _groundLayerMask;

        private PlayerActor _player;
        private Rigidbody _rigidbody;
        private Coroutine _toggleCoroutine;
        private bool _pressedJump;

        public States State
        {
            get
            {
                return _state;
            }

            private set
            {
                // set previous state only if state has changed
                _previousState = _state != value ? _state : _previousState;
                _state = value;
            }
        }

        public LayerMask GroundLayerMask
        {
            get { return _groundLayerMask; }
        }

        private void Start()
        {
            _player = GameSettings.Instance.PlayerSettings;

            if (!_player)
            {
                Debug.LogError("No " + typeof(PlayerActor) + " found. " +
                               "Disabling script...");
                enabled = false;
            }

            // neccesary components for the script to work
            this.GetNeededComponent(ref _rigidbody);
            this.GetNeededComponent(ref _modelCollider);
        }

        private void Update()
        {
            _pressedJump = Input.GetAxisRaw("Vertical") > 0;
        }

        private void FixedUpdate()
        {
            // refresh state
            UpdateMovementState();
        }

        /// <summary>
        /// Compares the current state with the given state
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public bool Is(States state)
        {
            return State == state;
        }

        /// <summary>
        /// Compares the previous state with the given state
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns></returns>
        public bool Was(States state)
        {
            return _previousState == state;
        }

        /// <summary>
        /// Updates the movement state depending on the rigidbody sorroudings
        /// and velocity
        /// </summary>
        private void UpdateMovementState()
        {
            // if it's floating keep it's state, managed from toggle
            if (State == States.Floating)
            {
                return;
            }

            // check if the player is touching the ground, otherwise asume it's jumping
            bool isGrounded = _modelCollider.IsGrounded(_groundLayerMask);

            if (isGrounded && !_pressedJump)
            {
                State = States.Grounded;
            }

            if (!isGrounded && _pressedJump)
            {
                State = States.OnJump;
                _pressedJump = false;
            }

            if (!isGrounded && _rigidbody.velocity.y < 0.0f)
            {
                State = States.Falling;
            }
        }

        /// <summary>
        /// Toggles the movement state to floating mode, after the player's
        /// floating time it's toggled back again
        /// </summary>
        public void ToggleFloating()
        {
            if (_toggleCoroutine != null)
            {
                _toggleCoroutine.Stop();
            }

            _toggleCoroutine = ToggleFloating(_player.FloatingTime).Start();
        }

        /// <summary>
        /// Toggles floating mode, after a given time floating mode is toggled
        /// again to recover it's original status mode
        /// </summary>
        /// <param name="floatingTime">The floating time.</param>
        /// <returns></returns>
        private IEnumerator ToggleFloating(float floatingTime)
        {
            // if the player is jumping wait for falling to avoid
            // vertical force stacking
            while (State == States.OnJump)
            {
                yield return new WaitForFixedUpdate();
            }

            // if it's already inflated then reset and deflate, asume it's
            // falling UpdateMovementState will set the correct state
            State = State == States.Floating ? States.Falling : States.Floating;

            // if no input is received the floating mode will eventually timeout
            if (State == States.Floating)
            {
                // floating time for reset back to falling state
                yield return new WaitForSeconds(floatingTime);
                _toggleCoroutine = ToggleFloating(0.0f).Start();
            }

            _toggleCoroutine = null;
        }
    }
}

