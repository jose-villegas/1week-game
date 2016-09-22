using Extensions;
using UnityEngine;

namespace Movement
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;
        [SerializeField]
        private AnimationCurve _movementCurve;
        [SerializeField]
        private float _speed;
        [SerializeField]
        private bool _useRigidbody = true;

        private Transform _source;
        private Rigidbody _rigidbody;
        private float _elapsedTime = 0.0f;

        public MovingPlatform(Transform target, AnimationCurve movementCurve,
                              float speed)
        {
            _target = target;
            _movementCurve = movementCurve;
            _speed = speed;
            _target = target;
        }

        // Use this for initialization
        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            // store the original transform parameters in a hidden gameObject
            _source = new GameObject("_" + transform.name).transform;
            _source.name += _source.GetInstanceID();
            // store transform parameters
            _source.localPosition = transform.position;
            _source.localRotation = transform.localRotation;
            _source.localScale = transform.localScale;
            _source.hideFlags = HideFlags.HideInHierarchy;
        }

        private void AlignWithTransform()
        {
            // curve determines the interpolation behavior
            float t = _movementCurve.Evaluate(_elapsedTime);
            // linearly interpolate between original transform and target transform
            var pos = Vector3.Lerp(_source.position, _target.position, t);
            var rot = Quaternion.Lerp(_source.rotation, _target.rotation, t);
            var sca = Vector3.Lerp(_source.localScale, _target.localScale, t);

            if (_useRigidbody && null != _rigidbody)
            {
                _rigidbody.MovePosition(pos);
                _rigidbody.MoveRotation(rot);
                _elapsedTime += Time.fixedDeltaTime * _speed;
            }
            else if(!_useRigidbody)
            {
                transform.position = pos;
                transform.rotation = rot;
                _elapsedTime += Time.deltaTime * _speed;
            }

            transform.localScale = sca;
        }

        private void Update()
        {
            if (_useRigidbody) return;

            AlignWithTransform();
        }

        private void FixedUpdate()
        {
            if (!_useRigidbody) return;

            AlignWithTransform();
        }

        private void OnDrawGizmos()
        {
            if (null == _target) return;

            Transform src = null == _source ? transform : _source;
            // target position
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_target.position, 0.1f);
            // source position
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(src.position, 0.1f);
            // transition
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(src.position, _target.position);
            // indicator
            Gizmos.DrawIcon((src.position + _target.position) / 2.0f, "pokecog");
        }
    }
}
