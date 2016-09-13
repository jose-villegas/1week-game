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
    private Vector3 _airStrafingSpeed;
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

    public Vector3 AirStrafingSpeed
    {
        get
        {
            return _airStrafingSpeed;
        }
    }
}
