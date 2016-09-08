using UnityEngine;
using System;

[CreateAssetMenu]
public class PlayerActor : DynamicActor
{
    [SerializeField, Header("Floating Mode")]
    private Vector3 _floatingForce;
    [SerializeField]
    private float _floatingTime = 3.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    private float _onFloatForceReduction = 0.5f;
    [SerializeField]
    private float _upwardBuildupTime = 1.0f;

    public Vector3 FloatingForce
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

    public float OnFloatForceReduction
    {
        get
        {
            return _onFloatForceReduction;
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
