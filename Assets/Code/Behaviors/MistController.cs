using UnityEngine;

namespace Behaviors
{
    public class MistController : MonoBehaviour
    {
        [SerializeField]
        private float _wavingScale = 0.005f;
        [SerializeField]
        private float _speed = 2.0f;

        private Vector3 _newPosition = Vector3.zero;
        private Transform[] _layers;
        private float _sumOfSin;

        // Use this for initialization
        private void Start()
        {
            _layers = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                _layers[i] = transform.GetChild(i);
            }
        }

        // Update is called once per frame
        private void Update()
        {
            // move horizontally with camera
            _newPosition.x = UnityEngine.Camera.main.transform.position.x;
            _newPosition.z = UnityEngine.Camera.main.transform.position.z;
            _newPosition.y = transform.position.y;
            // update position
            transform.position = _newPosition;

            // move layers for waving effect
            _sumOfSin = 0.0f;

            for (int i = 0; i < _layers.Length; i++)
            {
                _sumOfSin += Mathf.Sin(Time.time * _speed + i) * _wavingScale / (i + 1);
                _layers[i].position = _layers[i].position - Vector3.up * _sumOfSin;
            }
        }
    }
}