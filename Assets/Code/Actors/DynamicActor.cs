using UnityEngine;

namespace Actors
{
    [CreateAssetMenu]
    public class DynamicActor : ScriptableObject
    {
        [SerializeField, Header("Health")]
        private int _healthPoints = 1;
        [SerializeField]
        private float _afterHitImmunityTime = 3.0f;
        [SerializeField, Header("Movement")]
        private float _movementSpeed = 5.0f;
        [SerializeField]
        private Vector2 _jumpingForce = new Vector2(35, 350);
        [SerializeField]
        private float _airStrafingSpeed = 1.0f ;

        public float AirStrafingSpeed
        {
            get { return _airStrafingSpeed; }
        }

        public int HealthPoints
        {
            get { return _healthPoints; }
        }

        public float MovementSpeed
        {
            get { return _movementSpeed; }
        }

        public Vector2 JumpingForce
        {
            get { return _jumpingForce; }
        }

        public float AfterHitImmunityTime
        {
            get { return _afterHitImmunityTime; }
        }
    }
}
