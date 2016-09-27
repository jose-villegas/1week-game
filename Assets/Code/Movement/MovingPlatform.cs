using Extensions;
using General;
using Interfaces;
using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Moving platform movement logic, handles position, rotation and scale.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class MovingPlatform : MonoBehaviour, IRestartable
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
        private float _elapsedTime;

        /// <summary>
        /// Initializes a <see cref="MovingPlatform"/> class.
        /// </summary>
        /// <param name="target">The target transform.</param>
        /// <param name="movementCurve">The movement curve.</param>
        /// <param name="speed">The speed.</param>
        public void Initialize(Transform target, AnimationCurve movementCurve,
                               float speed)
        {
            _target = target;
            _movementCurve = movementCurve;
            _speed = speed;
            _target = target;
        }

        private void Awake()
        {
            // store the original transform parameters in a hidden gameObject
            _source = new GameObject("_" + transform.name).transform;
            _source.name += _source.GetInstanceID();
            // store transform parameters
            _source.position = transform.position;
            _source.rotation = transform.rotation;
            _source.localScale = transform.localScale;
            _source.hideFlags = HideFlags.HideInHierarchy;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            // subscribe to game pause events
            EventManager.StartListening("GamePaused", () => { enabled = false; });
            EventManager.StartListening("GameResumed", () => { enabled = true; });
        }

        /// <summary>
        /// Aligns this GameObject transform parameters with the
        /// <see cref="_target"/> transform
        /// </summary>
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

        /// <summary>
        /// Draws the beginning point and the ending point
        /// </summary>
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

        public void Restart()
        {
            // restore to original transform
            _elapsedTime = 0.0f;
            transform.Copy(_source);
        }
    }
}
