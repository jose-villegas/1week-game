using System.Collections.Generic;
using Actors;
using General;
using Interfaces;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Handles the interface for the player's number of health points
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerHeartsUI : MonoBehaviour, IRestartable
    {
        [SerializeField]
        private GameObject _heartAsset;

        private GameplaySettings _gameplay;
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
            _gameplay = GameSettings.Instance.GameplaySettings;
            // prefetch animation parameter identifiers
            _dissapearAnimation = Animator.StringToHash("Dissapear");
            _appearAnimation = Animator.StringToHash("Appear");
            // add heart assets to build interface
            BuildHeartsInterface();
            // subscribe to health reduction event
            EventManager.StartListening("HealthReduced", () =>
            {
                _availableHearts--;
                AnimateHeart(_dissapearAnimation);
            });
            // subscribe to level restart event
            EventManager.StartListening("LevelReset", Restart);
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
            if (index >= _heartsAnimators.Count || index < 0) return;

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
            if (_gameplay == null || transform.childCount == _gameplay.HealthPoints)
            {
                return;
            }

            _availableHearts = _gameplay.HealthPoints;
            _heartsAnimators = new List<Animator>();

            // instantiate hearts ui
            for (int i = 0; i < _availableHearts; i++)
            {
                GameObject go = Instantiate(_heartAsset);
                go.transform.SetParent(transform, false);
                _heartsAnimators.Add(go.GetComponentInChildren<Animator>());
            }
        }

        /// <summary>
        /// Restores available hearts count and makes them appear again
        /// </summary>
        public void Restart()
        {
            _availableHearts = _gameplay.HealthPoints;

            for (int i = 0; i < _heartsAnimators.Count; i++)
            {
                if (null != _heartsAnimators[i])
                {
                    _heartsAnimators[i].SetTrigger(_appearAnimation);
                }
            }
        }
    }
}
