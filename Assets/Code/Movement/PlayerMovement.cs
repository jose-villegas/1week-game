using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private enum MovementState
    {
        Grounded,
        OnJump,
        Falling,
        Floating
    }

    [SerializeField]
    private DynamicActor _player;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private MovementState _state;

    private Rigidbody _rigidbody;
    private RigidbodyConstraints _rigidbodyConstraints;

    private void Start()
    {
        // without the dynamic actor scriptableobject there
        // is no defined parameters for the player movement
        if (!_player)
        {
            Debug.LogError("No DynamicActor found. Disabling script...");
            enabled = false;
        }

        // neccesary components for the script to work
        this.GetNeededComponent(ref _rigidbody);
        this.GetNeededComponent(ref _collider);
        this.GetNeededComponent(ref _animator);

        // backup rigidbody constrains
        if (null != _rigidbody)
        {
            _rigidbodyConstraints = _rigidbody.constraints;
        }
    }

    /// <summary>
    /// Determines whether the player is on the ground, on top of a collider
    /// </summary>
    /// <returns><c>true</c> if the player is on top of a collider; otherwise, <c>false</c>.</returns>
    private bool IsGrounded()
    {
        float extend = _collider.bounds.extents.y;
        return Physics.Raycast(transform.position, -Vector3.up, extend + _player.DistanceToGround);
    }

    private void FixedUpdate()
    {
        Vector3 _movementVector = Vector3.zero;
        UpdateMovementState();
        HorizontalMovement(ref _movementVector);
        VerticalMovement(ref _movementVector);
        _rigidbody.AddForce(_movementVector);
 
        // limit player speed
        if (_rigidbody.velocity.magnitude > _player.MaximumVelocity)
        {
            _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, _player.MaximumVelocity);
        }
    }

    private void Update()
    {
        // inflate player and set the status
        if (Input.GetKeyDown(KeyCode.F))
        {
            // terminal timeout float cancelation
            if (_state == MovementState.Floating)
            {
                CancelInvoke();   
            }

            ToggleFloating();
        }
    }

    private void ToggleFloating()
    {
        // if it's already inflated then reset and deflate, asume it's 
        // falling UpdateMovementState will set the correct state
        _state = _state == MovementState.Floating ? MovementState.Falling : MovementState.Floating;
        _animator.SetBool("Inflate", _state == MovementState.Floating);

        // if no input is received the floating mode will eventually timeout
        if (_state == MovementState.Floating)
        {
            Invoke("ToggleFloating", _player.FloatingTime);
        }
    }

    private void UpdateMovementState()
    {
        // if it's floating keep it's state, managed from keypress
        if (_state == MovementState.Floating) { return; }

        // check if the player is touching the ground, otherwise asume it's jumping
        _state = IsGrounded() ? MovementState.Grounded : MovementState.OnJump;

        // if the object is going downward and not on the ground then it's falling
        if (_state == MovementState.OnJump && _rigidbody.velocity.y < 0.0f)
        {
            _state = MovementState.Falling;
        }
    }

    private void HorizontalMovement(ref Vector3 movement)
    {
        float horizontalAxis = Input.GetAxis("Horizontal");

        // horizontal movement - forward and backwards, check if player 
        // is grounded to avoid air strafing
        if (_state == MovementState.Grounded && horizontalAxis != 0.0f)
        {
            movement.x = horizontalAxis * _player.HorizontalForce;
        }
        // on floating add horizontal push force
        else if (_state == MovementState.Floating && horizontalAxis != 0.0f)
        {
            movement.x = horizontalAxis * _player.FloatingPush;
        }

        // deacceleration, brakes
        if (Input.GetKey(KeyCode.Space) && _state == MovementState.Grounded)
        {
            movement = Vector3.zero;
            _rigidbody.AddForce(-_rigidbody.velocity * _player.BrakeSpeed);
            _rigidbody.AddTorque(-_rigidbody.angularVelocity * _player.BrakeSpeed);
        }
    }

    private void VerticalMovement(ref Vector3 movement)
    {
        float verticalAxisRaw = Input.GetAxisRaw("Vertical");

        // jumping only when the character is on the ground
        if (_state == MovementState.Grounded && verticalAxisRaw > 0.0f)
        {
            movement.y = _player.VerticalForce;
        }
            
        // flatten on downward force and the player is grounded
        if (_state == MovementState.Grounded && verticalAxisRaw < 0.0f)
        {
            // for the flatte animation to properly work the player has to jave
            // forward, and up equivalent to world forward and word up
            var rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            var t = Time.fixedDeltaTime * _player.FlattenStabilizationSpeed;
            // stops physics control of rotation to enable flatten animation
            _rigidbody.freezeRotation = true;
            _rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation, rotation, t));

            // if rotation target is nearly reached, flatten
            if (Quaternion.Angle(transform.rotation, rotation) <= 1.0f)
            {
                _animator.SetBool("Flatten", true);
            }
        }
        else
        {
            // recover physics control of rotation
            _rigidbody.freezeRotation = false;
            // recover constraints on rotation
            _rigidbody.constraints = _rigidbodyConstraints;
            _animator.SetBool("Flatten", false);
        }

        // if the player is inflate add constant upward force
        if (_state == MovementState.Floating)
        {
            movement -= Physics.gravity;
            movement.y += _player.FloatingForce;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer != LayerMask.NameToLayer("Ground") && _state == MovementState.Floating)
        {
            CancelInvoke();
            ToggleFloating();
        }
    }
}
