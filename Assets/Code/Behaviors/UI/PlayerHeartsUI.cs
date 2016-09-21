using UnityEngine;

namespace Behaviors.UI
{
    public class PlayerHeartsUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _heartAsset;
        [SerializeField]
        private GameObject _player;
        [SerializeField]
        private PlayerHealth _playerHealth;

        // Use this for initialization
        void Start ()
        {
            if (null == _player)
            {
                _player = GameObject.FindGameObjectWithTag("Player");

                if (null == _player)
                {
                    Debug.LogError("No PlayerActor found. Disabling script...");
                    enabled = false;
                }
            }
            else
            {
                if (null == _playerHealth)
                {
                    _playerHealth = _player.GetComponent<PlayerHealth>();

                    if (null == _playerHealth)
                    {
                        Debug.LogError("No PlayerHealth component on " + _player);
                        enabled = false;
                    }
                }
            }
        }

        void OnDrawGizmos()
        {
            if (Application.isPlaying) return;

            // take advantage of OnDrawGizmos editor update to add hearts ui elements
            if (_playerHealth != null && transform.childCount != _playerHealth.Health)
            {
                // remove children from transforms
                for (int i = 0; i < transform.childCount; i++)
                {
                    DestroyImmediate(transform.GetChild(i));
                }

                // instantiate hearts ui
                for (int i = 0; i < _playerHealth.Health; i++)
                {
                    GameObject go = Instantiate(_heartAsset);
                    go.transform.SetParent(transform, false);
                }
            }
        }
    }
}
