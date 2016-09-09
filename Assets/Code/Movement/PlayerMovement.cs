using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(MovementState))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerActor _player;
    [SerializeField]
    private Animator _animator;

    private Rigidbody _rigidbody;
    private MovementState _state;
    private Vector2 _inputAxis;
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
        this.GetNeededComponent(ref _state);
        this.GetNeededComponent(ref _animator);
    }

    private void Update()
    {
        // store input
        _inputAxis.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // inflate player and set the status
        if (Input.GetKeyDown(KeyCode.F))
        {
            _state.ToggleFloating(_player.FloatingTime);
            _animator.SetBool("Inflate", _state.Equals(MovementState.States.Floating));
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
        if (_state.Equals(MovementState.States.Grounded) && _inputAxis.x != 0.0f)
        {
            _movement.x = _inputAxis.x * _player.MovementSpeed.x;
        }

        // air strafing can happen on falling state
        if (_state.Equals(MovementState.States.Falling) && _inputAxis.x != 0.0f)
        {
            _movement.x = _inputAxis.x * _player.FallingMovementSpeed.x;
        }

        if (_state.Equals(MovementState.States.Grounded) || _state.Equals(MovementState.States.Falling))
        {
            _rigidbody.MovePosition(transform.position + _movement * Time.deltaTime);
        }
    }

    void JumpMovement()
    {
        // jumping has to happen on the ground
        if(_state.Equals(MovementState.States.Grounded) && _inputAxis.y > 0.0f)
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
        if (!_state.Equals(MovementState.States.Floating)) { return; }

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
        if(_inputAxis.x != 0.0f)
        {
            fForce.x = _inputAxis.x * _player.FloatingForce.x;
        }
            
        _rigidbody.AddForce(fForce);
    }

    private void OnCollisionEnter(Collision col)
    {
        if (_state.Equals(MovementState.States.Floating) && col.contacts.Length > 0)
        {
            Vector3 colDir = (col.contacts[0].point - transform.position).normalized;

            if (Vector3.Angle(colDir, Vector3.up) < 5.0)
            {
                CancelInvoke();
                _state.ToggleFloating(_player.FloatingTime);
                _animator.SetBool("Inflate", false);
            }
        }
    }
}
