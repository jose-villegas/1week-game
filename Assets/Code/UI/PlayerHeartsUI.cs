using Actors;
using Behaviors;
using Entities;
using General;
using UnityEngine;

namespace UI
{
    public class PlayerHeartsUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _heartAsset;

        private Transform _player;
        private Animator[] _heartsAnimators = new Animator[0];

        public PlayerHeartsUI(GameObject heartAsset)
        {
            _heartAsset = heartAsset;
        }

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
            }

            BuildHeartsUI();
        }

        public void DissapearHeart(int index)
        {
            // obtain animators from child heartUI assets
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

        /// <summary>
        /// Instantiates the heart asset as children of this transform,
        /// the number of distances depends on the current health of the player
        /// </summary>
        private void BuildHeartsUI()
        {
            PlayerActor player = GameSettings.Instance.PlayerSettings;

            if (player != null && transform.childCount != player.HealthPoints)
            {
                // remove children from transforms
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }

                // obtain the animators as well for animations on health loss
                _heartsAnimators = new Animator[player.HealthPoints];

                // instantiate hearts ui
                for (int i = 0; i < player.HealthPoints; i++)
                {
                    GameObject go = Instantiate(_heartAsset);
                    go.transform.SetParent(transform, false);
                    _heartsAnimators[i] = go.GetComponentInChildren<Animator>();
                }
            }
        }
    }
}
