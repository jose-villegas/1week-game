using System.Collections;
using UnityEngine;

namespace Behaviors
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField]
        private float _damping = 5.0f;
        [SerializeField]
        private float _orientationChangeDuration = 0.5f;
        [SerializeField]
        private float _orbitRadius = 10.0f;

        private Transform _player;
        private Vector3 _velocity = Vector3.zero;
        private Coroutine _orientationChange;

        public static Vector3 MovementOrientation { get; private set; }

        // Use this for initialization
        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            if (null == player)
            {
                Debug.LogError("No player transform found, please use the Player tag");
                enabled = false;
            }
            else
            {
                _player = player.transform;
                transform.rotation = Quaternion.Euler(45, -45, 0);
                transform.position = _player.position - transform.forward * _orbitRadius;
                MovementOrientation = Vector3.right;
            }
        }

        private void Update()
        {
            if (null != _orientationChange) { return; }

            Vector3 target = _player.position - transform.forward * _orbitRadius;
            transform.position = Vector3.SmoothDamp(transform.position, target,
                                                    ref _velocity, _damping *
                                                    Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.J))
            {
                _orientationChange = StartCoroutine(OrientationChangeCo(-90));
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                _orientationChange = StartCoroutine(OrientationChangeCo(90));
            }
        }

        /// <summary>
        /// Changes the orientation angle along the y axis of the camera
        /// given an angle, this also modifies the movement orientation
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns></returns>
        private IEnumerator OrientationChangeCo(float angle)
        {
            float t = 0.0f;
            Quaternion rotate = Quaternion.AngleAxis(angle, Vector3.up);
            Quaternion dstRot = rotate * transform.rotation;
            Quaternion srcRot = transform.rotation;
            Vector3 targetFwd = rotate * transform.forward;
            Vector3 srcPos = transform.position;
            Vector3 dstPos = _player.position - targetFwd * _orbitRadius;
            Vector3 srcOrient = MovementOrientation;
            Vector3 dstOrient = rotate * MovementOrientation;

            while (t < _orientationChangeDuration)
            {
                t += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(srcRot, dstRot,
                                                     t / _orientationChangeDuration);
                transform.position = Vector3.Lerp(srcPos, dstPos,
                                                  t / _orientationChangeDuration);
                MovementOrientation = Vector3.Lerp(srcOrient, dstOrient,
                                                   t / _orientationChangeDuration);
                yield return new WaitForEndOfFrame();
            }

            // set to final position and rotation
            transform.position = dstPos;
            transform.rotation = dstRot;
            MovementOrientation = dstOrient;
            _orientationChange = null;
        }
    }
}
