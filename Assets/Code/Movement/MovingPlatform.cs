using Extensions;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatform : MonoBehaviour 
    {
        [SerializeField]
        private Vector3 _translation;
        [SerializeField]
        private AnimationCurve _movementCurve;
        [SerializeField]
        private float _speed;

        private Rigidbody _rigidbody;
        private Vector3 _sourcePosition;
        private float _elapsedTime = 0.0f;

        public MovingPlatform(Vector3 translation, AnimationCurve movementCurve, float speed)
        {
            _translation = translation;
            _movementCurve = movementCurve;
            _speed = speed;
        }

        // Use this for initialization
        private void Start() 
        {
            _sourcePosition = transform.position;
            this.GetNeededComponent(ref _rigidbody);
        }

        private void Update()
        {
            float t = _movementCurve.Evaluate(_elapsedTime);
            _rigidbody.MovePosition(Vector3.Lerp(_sourcePosition, _sourcePosition + _translation, t));
            _elapsedTime += Time.deltaTime * _speed;
        }

        private void OnDrawGizmos()
        {
            bool isPlaying = Application.isPlaying;
            Vector3 src = isPlaying ? _sourcePosition : transform.position;
            Vector3 dst = isPlaying ? _sourcePosition + _translation : transform.position + _translation;
            // target position
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(dst, 0.1f);
            // source position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(src, 0.1f);
            // transition
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(src, dst);
            // indicator
            Gizmos.DrawIcon((src + dst) / 2.0f, "pokecog");
        }
    }
}
