using UnityEngine;

namespace Actors
{
    /// <summary>
    /// Contains parameters for a dynamic actor, meaning it moves
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(fileName = "DynamicActor", menuName = "Actors/Dynamic Actor")]
    public class DynamicActor : ScriptableObject
    {
        [SerializeField]
        private float _immunityTime = 3.0f;
        [SerializeField, Header("Movement")]
        private float _speed = 5.0f;
        [SerializeField]
        private float _angularSpeed = 120.0f;
        [SerializeField]
        private Vector2 _jumpForce = new Vector2(35, 350);
        [SerializeField]
        private float _airStrafingSpeed = 1.0f ;

        public float AirStrafingSpeed
        {
            get { return _airStrafingSpeed; }
        }

        public float ImmunityTime
        {
            get { return _immunityTime; }
        }

        public float Speed
        {
            get { return _speed; }
        }

        public Vector2 JumpForce
        {
            get { return _jumpForce; }
        }

        public float AngularSpeed
        {
            get { return _angularSpeed; }
        }
    }
}
