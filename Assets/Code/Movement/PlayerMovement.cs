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
    private PlayerActor _player;
    [SerializeField]
    private Collider _collider;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private MovementState _state;

    private Rigidbody _rigidbody;
    private Vector3 _movement;
    private float _upwardTimer = 0.0f;
    private bool _onFloatForceReduction = false;

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
        this.GetNeededComponent(ref _collider);
        this.GetNeededComponent(ref _animator);
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

    private void FixedUpdate()
    {
        // store input
        _movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
        // check the current state of the player and update properly
        UpdateMovementState();
        // on ground movement logic
        GroundMovement();
        // jump movement logic
        JumpMovement();
        // floating mode movement logic
        FloatingMovement();
        // finally move the player
        _rigidbody.MovePosition(transform.position + _movement * Time.deltaTime);
    }

    private void GroundMovement()
    {
        // horizontal movement - forward and backwards, 
        // check if grounded to avoid air strafing on jump
        if (_state == MovementState.Grounded && _movement.x != 0.0f)
        {
            _movement.x = _movement.x * _player.MovementSpeed.x;
        }
    }

    void JumpMovement()
    {
        if (_state == MovementState.OnJump) { _movement.x = 0.0f; }

        // jumping has to happen on the ground
        if(_state == MovementState.Grounded && _movement.y > 0.0f)
        {
            // jump direction force
            _rigidbody.AddForce
            (
                Vector3.right * _movement.x * _player.JumpingForce.x +
                Vector3.up * _player.JumpingForce.y
            );
        }
    }

    void FloatingMovement()
    {
        if (_state != MovementState.Floating) { return; }

        // velocity falloff on floating mode
        if (_onFloatForceReduction)
        {
            _rigidbody.AddForce(-_rigidbody.velocity * _player.OnFloatForceReduction);
            _onFloatForceReduction = false;
        }

        Vector3 fForce = Vector3.zero;
        fForce.y = -Physics.gravity.y + _player.FloatingForce.y;

        // floating upward force
        if (_upwardTimer < _player.UpwardBuildupTime)
        {
            fForce.y = Mathf.Lerp(_rigidbody.velocity.y, fForce.y, _upwardTimer / _player.UpwardBuildupTime);
            _upwardTimer = _upwardTimer + Time.fixedDeltaTime;
        }

        // floating horizontal push
        if(_movement.x != 0.0f)
        {
            fForce.x = _movement.x * _player.FloatingForce.x;
        }
            
        _rigidbody.AddForce(fForce);
    }

    private void ToggleFloating()
    {
        // if it's already inflated then reset and deflate, asume it's 
        // falling UpdateMovementState will set the correct state
        _state = _state == MovementState.Floating ? MovementState.Falling : MovementState.Floating;
        _animator.SetBool("Inflate", _state == MovementState.Floating);
        _onFloatForceReduction = true;
        _upwardTimer = 0.0f; 

        // if no input is received the floating mode will eventually timeout
        if (_state == MovementState.Floating)
        {
            Invoke("ToggleFloating", _player.FloatingTime);
        }
    }

    private void UpdateMovementState()
    {
        // if it's floating keep it's state, managed from keypress
        if (_state == MovementState.Floating)
        {
            return;
        }

        // check if the player is touching the ground, otherwise asume it's jumping
        bool isGrounded = _player.IsGrounded(transform, _collider.bounds.extents.y);
        _state = isGrounded ? MovementState.Grounded : MovementState.OnJump;

        // if the object is going downward and not on the ground then it's falling
        if (_state == MovementState.OnJump && _rigidbody.velocity.y < 0.0f)
        {
            _state = MovementState.Falling;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (_state == MovementState.Floating && col.contacts.Length > 0)
        {
            Vector3 colDir = (col.contacts[0].point - transform.position).normalized;

            if (Vector3.Angle(colDir, Vector3.up) < 5.0)
            {
                CancelInvoke();
                ToggleFloating(); 
            }
        }
    }
}
