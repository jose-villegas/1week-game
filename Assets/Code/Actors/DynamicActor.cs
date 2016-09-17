using UnityEngine;

namespace Actors
{
    [CreateAssetMenu]
    public class DynamicActor : ScriptableObject
    {
        [SerializeField, Header("Movement")]
        private Vector3 _axisMovementSpeed = Vector3.right;
        [SerializeField]
        private Vector3 _jumpingForce = Vector3.up;
        [SerializeField]
        private Vector3 _airStrafingSpeed = Vector3.right;

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
}
