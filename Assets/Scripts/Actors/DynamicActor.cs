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
}
