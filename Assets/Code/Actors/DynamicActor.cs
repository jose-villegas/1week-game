using UnityEngine;
using System;

[CreateAssetMenu]
public class DynamicActor : ScriptableObject
{
    [SerializeField, Header("Movement")]
    private Vector3 _axisMovementSpeed;
    [SerializeField]
    private Vector3 _jumpingForce;
    [SerializeField, Header("Limits")]
    private float _maximumVelocityMagnitude;
    [SerializeField]
    private float _distanceToGround = 0.1f;

    public Vector3 MovementSpeed
    {
        get
        {
            return _axisMovementSpeed;
        }
    }

    public float DistanceToGround
    {
        get
        { 
            return _distanceToGround;
        }
    }


    public Vector3 JumpingForce
    {
        get
        {
            return _jumpingForce;
        }
    }


    /// <summary>
    /// Determines whether the actor is on the ground, on top of a collider
    /// </summary>
    /// <returns<c>true</c> if the actor is on top of a collider; otherwise, <c>false</c>.</returns>
    /// <param name="actor">The actor.</param>
    /// <param name="extend">Actor vertical extend, collider.extend.y is recommended.</param>
    public bool IsGrounded(Transform actor, float extent)
    {
        return Physics.Raycast(actor.position, -Vector3.up, extent + _distanceToGround);
    }
}
