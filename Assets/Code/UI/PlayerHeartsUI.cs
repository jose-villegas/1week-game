using System.Collections.Generic;
using Actors;
using Behaviors;
using Entities;
using General;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Handles the interface for the player's number of health points
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerHeartsUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject _heartAsset;

        private PlayerHealth _playerHealth;
        private List<Animator> _heartsAnimators;
        private int _availableHearts;
        private int _dissapearAnimation;
        private int _appearAnimation;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerHeartsUI"/> class.
        /// </summary>
        /// <param name="heartAsset">The heart asset.</param>
        public PlayerHeartsUI(GameObject heartAsset)
        {
            _heartAsset = heartAsset;
        }

        private void Start ()
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();

            if (null == _playerHealth)
            {
                Debug.LogError("No " + typeof(PlayerHealth) + " found. " +
                               "Disabling script...");
                enabled = false;
            }

            _dissapearAnimation = Animator.StringToHash("Dissapear");
            _appearAnimation = Animator.StringToHash("Appear");
            BuildHeartsInterface();
        }

        private void Update()
        {
            // health has been reduced
            if (_playerHealth.Health < _availableHearts)
            {
                _availableHearts--;
                AnimateHeart(_dissapearAnimation);
            }
        }

        /// <summary>
        /// Triggers the given animation for the heart controller
        /// at index <see cref="_availableHearts"/>
        /// </summary>
        public void AnimateHeart(int animationParameter)
        {
            // current heart being removed
            int index = _availableHearts;

            // out of range
            if (index > _heartsAnimators.Count || index < 0) return;

            if (null != _heartsAnimators[index])
            {
                // animate to dissapear state
                _heartsAnimators[index].SetTrigger(animationParameter);
            }
        }

        /// <summary>
        /// Instantiates the heart asset as children of this transform,
        /// the number of distances depends on the current health of the player
        /// </summary>
        private void BuildHeartsInterface()
        {
            PlayerActor player = GameSettings.Instance.PlayerSettings;

            if (player != null && transform.childCount != player.HealthPoints)
            {
                // remove children from transforms
                for (int i = 0; i < transform.childCount; i++)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }

                _availableHearts = player.HealthPoints;
                _heartsAnimators = new List<Animator>();

                // instantiate hearts ui
                for (int i = 0; i < _availableHearts; i++)
                {
                    GameObject go = Instantiate(_heartAsset);
                    go.transform.SetParent(transform, false);
                    _heartsAnimators.Add(go.GetComponentInChildren<Animator>());
                }
            }
        }
    }
}
