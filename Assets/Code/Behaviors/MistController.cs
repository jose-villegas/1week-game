using Entities;
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
        private PlayerHealth _playerHealth = null;

        // Use this for initialization
        private void Start()
        {
            // ignore last layer, final layer registers hits
            _layers = new Transform[transform.childCount - 1];

            for (int i = 0; i < transform.childCount - 1; i++)
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

            for (int i = 0; _layers != null && i < _layers.Length; i++)
            {
                int l = _layers.Length - 1 - i;
                _sumOfSin += Mathf.Sin(Time.time * _speed + i) * _wavingScale;
                _layers[l].position = _layers[l].position - Vector3.up * _sumOfSin;
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.tag != "Player") return;

            if (null == _playerHealth)
            {
                _playerHealth = col.transform.GetComponent<PlayerHealth>();
            }

            if (null == _playerHealth) return;

            _playerHealth.Hit();
            col.transform.position = SpawnZone.CurrentSpawnPoint.position;
        }
    }
}