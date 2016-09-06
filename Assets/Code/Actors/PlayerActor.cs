using UnityEngine;
using System;

[CreateAssetMenu]
public class PlayerActor : DynamicActor
{
    [SerializeField, Header("Stabilization"), Range(0.01f, 1.0f)]
    private float _brakeSpeed = 0.975f;
    [SerializeField]
    private float _flattenStabilizationSpeed = 50.0f;
    [SerializeField, Header("Floating Mode")]
    private float _floatingForce = 0.15f;
    [SerializeField]
    private float _floatingTime = 3.0f;
    [SerializeField]
    private float _floatingMovementForce = 1.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float _onFloatVelocityFalloff = 0.5f;
    [SerializeField]
    private float _onFloatVelocityFalloffTime = 1.0f;
    [SerializeField]
    private float _upwardBuildupTime = 1.0f;

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

    public float FloatingMovementForce
    {
        get
        { 
            return _floatingMovementForce;
        }
    }

    public float FloatVelocityFalloff
    {
        get
        {
            return _onFloatVelocityFalloff;
        }
    }

    public float FloatVelocityFalloffTime
    {
        get
        {
            return _onFloatVelocityFalloffTime;
        }
    }

    public float UpwardBuildupTime
    {
        get
        {
            return _upwardBuildupTime;
        }
    }
}
