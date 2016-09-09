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
    private Collider _groundCollider;

    private Rigidbody _rigidbody;

    public bool Equals(States state)
    {
        return _state == state;
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
        if (_state == States.Floating)
        {
            return;
        }

        // check if the player is touching the ground, otherwise asume it's jumping
        bool isGrounded = _groundCollider.IsGrounded();
        _state = isGrounded ? States.Grounded : States.OnJump;

        // if the object is going downward and not on the ground then it's falling
        if (_state == States.OnJump && _rigidbody.velocity.y < 0.0f)
        {
            _state = States.Falling;
        }
    }
        
    public void ToggleFloating(float floatingTime)
    {
        CancelInvoke();
        // if it's already inflated then reset and deflate, asume it's 
        // falling UpdateMovementState will set the correct state
        _state = _state == States.Floating ? States.Falling : States.Floating;

        // if no input is received the floating mode will eventually timeout
        if (_state == States.Floating)
        {
            Invoke("ToggleFloating", floatingTime);
        }
    }
}

