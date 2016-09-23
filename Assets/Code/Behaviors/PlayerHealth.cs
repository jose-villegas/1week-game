using Actors;
using Entities;
using General;
using Interfaces;
using UI;
using UnityEngine;

namespace Behaviors
{
    public class PlayerHealth : MonoBehaviour, IHittable
    {
        private PlayerActor _player;
        private PlayerHeartsUI _heartsInterface;
        private bool _immunityActive;
        private int _currentHealth;

        public int Health
        {
            get { return _currentHealth; }

            private set { _currentHealth = value; }
        }

        private void Start()
        {
            _player = GameSettings.Instance.PlayerSettings;

            // without the PlayerActor scriptableobject there
            // are no defined parameters for the player health
            if (!_player)
            {
                Debug.LogError("No " + typeof(PlayerActor) + " found. " +
                               "Disabling script...");
                enabled = false;
            }
            else
            {
                _currentHealth = _player.HealthPoints;
            }

            _heartsInterface = FindObjectOfType<PlayerHeartsUI>();
        }

        public void TemporalImmunity()
        {
            _immunityActive = true;
            CancelInvoke("DisableImmunity");
            Invoke("DisableImmunity", _player.AfterHitImmunityTime);
        }

        private void DisableImmunity()
        {
            _immunityActive = false;
        }

        public void Hit()
        {
            if (Health <= 0 || _immunityActive) return;

            Health--;

            if (null != _heartsInterface)
            {
                _heartsInterface.DissapearHeart(Health);
            }
        }
    }
}
