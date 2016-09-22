using Behaviors;
using UnityEngine;

namespace UI
{
    public class PlayerHeartsUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _heartAsset;
        [SerializeField]
        private GameObject _player;
        [SerializeField]
        private PlayerHealth _playerHealth;

        private Animator[] _heartsAnimators = new Animator[0];

        public PlayerHeartsUI(GameObject heartAsset)
        {
            _heartAsset = heartAsset;
        }

        // Use this for initialization
        private void Start ()
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

        public void DissapearHeart(int index)
        {
            // obtain animators from childs, heart assets
            if (null == _heartsAnimators)
            {
                _heartsAnimators = new Animator[transform.childCount];

                for (int i = 0; i < transform.childCount; i++)
                {
                    Transform t = transform.GetChild(i);
                    _heartsAnimators[i] = t.GetComponentInChildren<Animator>();
                }
            }

            // out of range
            if (index > _heartsAnimators.Length || index < 0) return;

            if (null != _heartsAnimators[index])
            {
                // animate to dissapear state
                _heartsAnimators[index].SetBool("Dissapear", true);
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

                _heartsAnimators = new Animator[_playerHealth.Health];

                // instantiate hearts ui
                for (int i = 0; i < _playerHealth.Health; i++)
                {
                    GameObject go = Instantiate(_heartAsset);
                    go.transform.SetParent(transform, false);
                    _heartsAnimators[i] = go.GetComponentInChildren<Animator>();
                }
            }
        }
    }
}
