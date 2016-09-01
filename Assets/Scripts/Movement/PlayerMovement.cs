using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private DynamicActor _player;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private Vector3 _movementVector = Vector3.zero;
    private bool _onJump;

    private void Start()
    {
        // player's rigidbody for movement
        _rigidbody = GetComponent<Rigidbody>();  

        if (!_rigidbody)
        {
            Debug.LogError("Couldn't obtain Rigidbody component in " + this);
            enabled = false;
        }

        _collider = GetComponent<Collider>();

        if (!_collider)
        {
            Debug.LogError("Couldn't obtain Collider component in " + this);
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
        bool grounded = IsGrounded();
        float verticalAxis = Input.GetAxisRaw("Vertical");
        _movementVector = Vector3.zero;
        // horizontal movement - forward and backwards, check if player 
        // is grounded to avoid air strafing
        if (grounded)
        {
            _movementVector.x = Input.GetAxis("Horizontal") * _player.HorizontalForce;
        }

        // jumping only when the character is on the ground
        if (verticalAxis > 0.0f && grounded && !_onJump)
        {
            _movementVector.y = _player.VerticalForce;
            _onJump = true;
        }

        // reset on jump once the player is on the ground
        if (grounded && _movementVector.y == 0.0f)
        {
            _onJump = false;
        }

        _rigidbody.AddForce(_movementVector);
    }
}
