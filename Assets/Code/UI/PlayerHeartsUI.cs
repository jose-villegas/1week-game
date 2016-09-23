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
        private Animator[] _heartsAnimators = new Animator[0];

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

            BuildHeartsInterface();
        }

        /// <summary>
        /// Dissapears
        /// </summary>
        public void DissapearHeart()
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

            // current heart being removed
            int index = _playerHealth.Health;

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
