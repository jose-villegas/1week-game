using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private enum MovementState
    {
        Grounded,
        OnJump,
        Falling
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


    private void Start()
    {
        // player's rigidbody for movement
        _rigidbody = GetComponent<Rigidbody>();  

        if (!_rigidbody)
        {
            Debug.LogError("Couldn't obtain Rigidbody component in " + this);
            enabled = false;
        }

        if (!_collider)
        {
            Debug.LogError("No Collider component provided");
            enabled = false;
        }

        if (!_animator)
        {
            Debug.LogError("No Animator component provided");
            enabled = false;
        }

        if (!_player)
        {
            Debug.LogError("No DynamicActor provided");
            enabled = false;
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
        _state = IsGrounded() ? MovementState.Grounded : MovementState.OnJump;

        if (_state == MovementState.OnJump && _rigidbody.velocity.y < 0.0f)
        {
            _state = MovementState.Falling;
        }

        HorizontalMovement(ref _movementVector);
        VerticalMovement(ref _movementVector);
        _rigidbody.AddForce(_movementVector);
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

        // deacceleration, brakes
        if (Input.GetKey(KeyCode.Space) && _state == MovementState.Grounded)
        {
            movement = Vector3.zero;
            _rigidbody.AddTorque(-_rigidbody.angularVelocity * _player.BrakeSpeed);
        }
    }

    private void VerticalMovement(ref Vector3 movement)
    {
        float verticalAxisRaw = Input.GetAxisRaw("Vertical");

        // jumping only when the character is on the ground
        if (verticalAxisRaw > 0.0f && _state == MovementState.Grounded)
        {
            movement.y = _player.VerticalForce;
        }
    }
}
