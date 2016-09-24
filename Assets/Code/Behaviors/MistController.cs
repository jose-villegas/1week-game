using Entities;
using General;
using UnityEngine;

namespace Behaviors
{
    /// <summary>
    /// Controls the mist layers in a wave like motion, the layers are obtained
    /// from the <see cref="MistController.transform"/>'s childs. If the player
    /// collides with one of the layers damage is done to the player and the player
    /// is translated to the last spawn point.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class MistController : MonoBehaviour
    {
        [SerializeField]
        private float _wavingScale = 0.005f;
        [SerializeField]
        private float _speed = 2.0f;

        private Vector3 _newPosition = Vector3.zero;
        private Transform[] _layers;
        private float _sumOfSin;

        private void Start()
        {
            // ignore last layer, final layer registers hits
            _layers = new Transform[transform.childCount - 1];

            for (int i = 0; i < transform.childCount - 1; i++)
            {
                _layers[i] = transform.GetChild(i);
            }
        }

        private void Update()
        {
            // move horizontally with camera
            _newPosition.x = Camera.main.transform.position.x;
            _newPosition.z = Camera.main.transform.position.z;
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
            if (!col.CompareTag("Player")) return;

            EventManager.TriggerEvent("HitPlayer");
            col.transform.position = SpawnZone.CurrentSpawnPoint.position;
        }
    }
}