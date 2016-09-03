using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Dynamic Actor", order = 1)]
public class DynamicActor : ScriptableObject
{
    [SerializeField]
    private float _horizontalForce = 5.0f;
    [SerializeField]
    private float _verticalForce = 50.0f;
    [SerializeField]
    private float _distanceToGround = 0.1f;
    [SerializeField, Range(0.01f, 1.0f)]
    private float _brakeSpeed = 0.975f;

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
}
