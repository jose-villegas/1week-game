using UnityEngine;

namespace Actors
{
    [CreateAssetMenu]
    public class DynamicActor : ScriptableObject
    {
        [SerializeField, Header("Movement")]
        private float _axisMovementSpeed = 5.0f;
        [SerializeField]
        private Vector2 _jumpingForce = new Vector2(35, 350);
        [SerializeField]
        private float _airStrafingSpeed = 1.0f ;

        public float MovementSpeed
        {
            get
            {
                return _axisMovementSpeed;
            }
        }

        public Vector2 JumpingForce
        {
            get
            {
                return _jumpingForce;
            }
        }

        public float AirStrafingSpeed
        {
            get
            {
                return _airStrafingSpeed;
            }
        }
    }
}
