using UnityEngine;

namespace Actors
{
    /// <summary>
    /// Contains the parameters for a player actor, a player actor
    /// receives direct input from the user
    /// </summary>
    /// <seealso cref="Actors.DynamicActor" />
    [CreateAssetMenu]
    public class PlayerActor : DynamicActor
    {
        [SerializeField, Header("Floating Mode")]
        private Vector3 _floatingForce = Vector3.zero;
        [SerializeField]
        private float _floatingTime = 3.0f;
        [SerializeField]
        private float _upwardBuildupTime = 1.0f;
        [SerializeField, Header("Attack")]
        private float _attackVerticalForce = 5.0f;
        [SerializeField]
        private float _pushbackRadiusScale = 0.15f;
        [SerializeField]
        private float _upwardModifier = 30.0f;
        [SerializeField]
        private float _pushbackForceRatio = 15.0f;

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

        public float UpwardBuildupTime
        {
            get
            {
                return _upwardBuildupTime;
            }
        }

        public float AttackVerticalForce
        {
            get
            {
                return _attackVerticalForce;
            }
        }

        public float PushbackRadiusScale
        {
            get
            {
                return _pushbackRadiusScale;
            }
        }

        public float UpwardModifier
        {
            get
            {
                return _upwardModifier;
            }
        }

        public float PushbackForceRatio
        {
            get
            {
                return _pushbackForceRatio;
            }
        }
    }
}
