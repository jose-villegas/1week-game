using UnityEngine;
using System;

[CreateAssetMenu]
public class DynamicActor : ScriptableObject
{
    [SerializeField, Header("Movement")]
    private Vector3 _axisMovementSpeed;
    [SerializeField]
    private Vector3 _jumpingForce;
    [SerializeField]
    private Vector3 _fallingMovementSpeed;
    [SerializeField, Header("Limits")]
    private float _maximumVelocityMagnitude;

    public Vector3 MovementSpeed
    {
        get
        {
            return _axisMovementSpeed;
        }
    }

    public Vector3 JumpingForce
    {
        get
        {
            return _jumpingForce;
        }
    }

    public Vector3 FallingMovementSpeed
    {
        get
        {
            return _fallingMovementSpeed;
        }
    }
}
