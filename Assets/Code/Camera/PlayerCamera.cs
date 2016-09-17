using UnityEngine;

namespace Camera
{
    public class PlayerCamera : MonoBehaviour 
    {
        [SerializeField]
        private float _damping = 5.0f;

        private Transform _player;
        private Vector3 _translation;
        private Vector3 _velocity = Vector3.zero;

        // Use this for initialization
        private void Start () 
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
                _translation = transform.position - _player.position;
            }
        }

        private void Update()
        {
            Vector3 target = _player.position + _translation;
            transform.position = Vector3.SmoothDamp(transform.position, target, 
                ref _velocity, _damping * Time.deltaTime);
        }
    }
}
