using System.Collections;
using UnityEngine;

namespace Camera
{
    // [ExecuteInEditMode]
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
            }
        }

        private void Update()
        {
            Vector3 target = _player.position - transform.forward * _orbitRadius;
            transform.position = Vector3.SmoothDamp(transform.position, target,
                                                    ref _velocity, _damping * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.J) && null == _orientationChange)
            {
                _orientationChange = StartCoroutine(OrientationChangeCo(-1));
            }
            else if (Input.GetKeyDown(KeyCode.K) && null == _orientationChange)
            {
                _orientationChange = StartCoroutine(OrientationChangeCo(1));
            }
        }

        private IEnumerator OrientationChangeCo(int sign)
        {
            float t = 0.0f;
            Quaternion dstRot = Quaternion.AngleAxis(sign * 90, Vector3.up) *
                                transform.rotation;
            Quaternion srcRot = transform.rotation;

            while (t < _orientationChangeDuration)
            {
                t += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(srcRot, dstRot,
                                                     t / _orientationChangeDuration);
                yield return new WaitForEndOfFrame();
            }

            transform.rotation = dstRot;
            _orientationChange = null;
        }
    }
}
