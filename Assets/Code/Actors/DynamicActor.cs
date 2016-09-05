using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dynamic Actor", order = 1)]
public class DynamicActor : ScriptableObject
{
    [SerializeField, Header("On Ground Movement")]
    private float _horizontalForce = 5.0f;
    [SerializeField]
    private float _verticalForce = 50.0f;
    [SerializeField, Range(0.01f, 1.0f)]
    private float _brakeSpeed = 0.975f;
    [SerializeField]
    private float _flattenStabilizationSpeed = 50.0f;
    [SerializeField, Header("Limits")]
    private float _maximumVelocity = 10.0f;
    [SerializeField]
    private float _distanceToGround = 0.1f;
    [SerializeField, Header("Floating Mode")]
    private float _floatingForce = 0.15f;
    [SerializeField]
    private float _floatingTime = 3.0f;
    [SerializeField]
    private float _floatingPushForce = 1.0f;

    public float HorizontalForce
    {
        get
        {
            return _horizontalForce;
        }
    }

    public float VerticalForce
    {
        get
        {
            return _verticalForce;
        }
    }

    public float DistanceToGround
    {
        get
        { 
            return _distanceToGround;
        }
    }

    public float BrakeSpeed
    {
        get
        { 
            return _brakeSpeed;
        }
    }

    public float FlattenStabilizationSpeed
    {
        get
        { 
            return _flattenStabilizationSpeed;
        }
    }

    public float MaximumVelocity
    {
        get
        { 
            return _maximumVelocity;
        }
    }

    public float FloatingForce
    {
        get
        { 
            return _floatingForce;
        }
    }

    public float FloatingTime
    {
        get
        { 
            return _floatingTime;
        }
    }

    public float FloatingPush
    {
        get
        { 
            return _floatingPushForce;
        }
    }
}
