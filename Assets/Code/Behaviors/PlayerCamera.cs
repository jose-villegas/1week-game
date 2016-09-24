using System.Collections;
using Extensions;
using General;
using Interfaces;
using UnityEngine;

namespace Behaviors
{
    /// <summary>
    /// Main camera movement logic, follows the player and handles
    /// movement orientation
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerCamera : MonoBehaviour, IRestartable
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
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (null == player)
            {
                Debug.LogError("No player transform found, use the Player tag");
                enabled = false;
            }
            else
            {
                _player = player.transform;
                transform.rotation = Quaternion.Euler(45, -45, 0);
                transform.position = _player.position - transform.forward * _orbitRadius;
                MovementOrientation = Vector3.right;
            }

            // on level reset restore original rotation
            EventManager.StartListening("LevelReset", Restart);
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
                _orientationChange = OrientationChangeCo(90).Start();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                _orientationChange = OrientationChangeCo(-90).Start(); ;
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
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
            // camera final rotatio
            Quaternion srcRot = transform.rotation;
            Quaternion dstRot = rotation * transform.rotation;
            // camera final position
            Vector3 targetForward = rotation * transform.forward;
            Vector3 srcPos = transform.position;
            Vector3 dstPos = _player.position - targetForward * _orbitRadius;
            // final movement orientation
            Vector3 srcOrient = MovementOrientation;
            Vector3 dstOrient = rotation * MovementOrientation;

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
            // reset coroutine holder
            _orientationChange = null;
        }

        /// <summary>
        /// Restores the original movement orientation to the right vector
        /// </summary>
        public void Restart()
        {
            if(null != _orientationChange) StopCoroutine(_orientationChange);

            float angle = Vector3.Angle(MovementOrientation, Vector3.right);
            _orientationChange = OrientationChangeCo(angle).Start();
        }
    }
}
