using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class MovementState : MonoBehaviour
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
    private Collider _groundCollider;

    private Rigidbody _rigidbody;
    private Coroutine _routine;

    public States State
    {
        get
        {
            return _state;
        }
        private set 
        { 
            _previousState = _state != value ? _state : _previousState;
            _state = value;
        }
    }

    public bool Current(States state)
    {
        return State == state;
    }

    public bool Previous(States state)
    {
        return _previousState == state;
    }

    // Use this for initialization
    void Start()
    {
        // neccesary components for the script to work
        this.GetNeededComponent(ref _rigidbody);
        this.GetNeededComponent(ref _groundCollider);
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
	    // refresh state
        UpdateMovementState();
    }
        
    private void UpdateMovementState()
    {
        // if it's floating keep it's state, managed from toggle
        if (State == States.Floating)
        {
            return;
        }

        // check if the player is touching the ground, otherwise asume it's jumping
        bool isGrounded = _groundCollider.IsGrounded();

        if (_rigidbody.velocity.y > 0.0f)
        {
            State = States.OnJump;
        }
        else if (!isGrounded && _rigidbody.velocity.y < 0.0f)
        {
            State = States.Falling;
        }
        else if (isGrounded)
        {
            State = States.Grounded;
        }
           
    }
        
    public void ToggleFloating(float floatingTime)
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
        }
        
        _routine = StartCoroutine(ToggleFloatingCo(floatingTime));
    }

    public IEnumerator ToggleFloatingCo(float floatingTime)
    {
        // if it's already inflated then reset and deflate, asume it's 
        // falling UpdateMovementState will set the correct state
        State = State == States.Floating ? States.Falling : States.Floating;

        // if no input is received the floating mode will eventually timeout
        if (State == States.Floating)
        {
            yield return new WaitForSeconds(floatingTime);
            _routine = StartCoroutine(ToggleFloatingCo(0.0f));
        }

        _routine = null;
    }
}

