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
    private bool _onFloatForceReduction = true;
    private int _animInflate;
    private int _animFlatten;

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
        if (!_state.Current(MovementState.States.Floating) && _animator.GetBool(_animInflate))
        {
            _animator.SetBool(_animInflate, false);
        }

        // reset floating mode timer
        if (_state.Previous(MovementState.States.Floating))
        {
            _onFloatForceReduction = true;
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
            (_state.Current(MovementState.States.Falling) && 
                _state.Previous(MovementState.States.Grounded)) 
            && _inputAxis.x != 0.0f)
        {
            _movement.x = _inputAxis.x * _player.MovementSpeed.x;
        }

        // air strafing happens on falling state
        if (_state.Current(MovementState.States.Falling) && 
            (_state.Previous(MovementState.States.OnJump) 
                || _state.Previous(MovementState.States.Floating)) 
            && _inputAxis.x != 0.0f)
        {
            _movement.x = _inputAxis.x * _player.AirStrafingSpeed.x;
        }

        // flatten with push down on ground
        if (_state.Current(MovementState.States.Grounded) && _inputAxis.y < 0.0f && !_animator.GetBool("Flatten"))
        {
            _animator.SetBool(_animFlatten, true);
        }
        else if(_animator.GetBool(_animFlatten) && _inputAxis.y >= 0.0f)
        {
            _animator.SetBool(_animFlatten, false);
        }

        if (_state.Current(MovementState.States.Grounded) || _state.Current(MovementState.States.Falling))
        {
            _rigidbody.MovePosition(transform.position + _movement * Time.deltaTime);
        }
    }

    void JumpMovement()
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

    void FloatingMovement()
    {
        if (!_state.Current(MovementState.States.Floating)) { return; }

        // velocity falloff on floating mode
        if (_onFloatForceReduction)
        {
            _rigidbody.AddForce(-_rigidbody.velocity * _player.OnFloatForceReduction);
            _onFloatForceReduction = false;
        }

        Vector3 fForce = Vector3.zero;
        float t = _upwardTimer / _player.UpwardBuildupTime;
        // floating upward force
        fForce.y = Mathf.Lerp(0.0f, _player.FloatingForce.y, t);
        _upwardTimer = _upwardTimer + Time.fixedDeltaTime;

        // floating horizontal push
        if(_inputAxis.x != 0.0f)
        {
            fForce.x = _inputAxis.x * _player.FloatingForce.x;
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
            }
        }
    }
}
