﻿using Actors;
using Extensions;
using General;
using Interfaces;
using UnityEngine;

namespace Behaviors
{
    /// <summary>
    /// Handles the player's health points. Requires collider
    /// to register hits from other objects
    /// </summary>
    /// <seealso cref="Interfaces.IRestartable" />
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="Interfaces.IHittable" />
    public class PlayerHealth : MonoBehaviour, IHittable, IRestartable
    {
        private PlayerActor _player;
        private bool _immunityActive;
        private GameplaySettings _gameplay;

        public int Health { get; private set; }

        private void Start()
        {
            _player = GameSettings.Instance.PlayerSettings;
            _gameplay = GameSettings.Instance.GameplaySettings;

            // without the GameplaySettings scriptableobject there
            // are no defined parameters for the player health
            if (!_gameplay)
            {
                Debug.LogError("No " + typeof(GameplaySettings) + " found. " +
                               "Disabling script...");
                enabled = false;
            }
            else
            {
                Health = _gameplay.HealthPoints;
            }

            // event to receive hits from other objects
            EventManager.StartListening("HitPlayer", Hit);
            // restore health on level reset
            EventManager.StartListening("LevelReset", Restart);
        }

        /// <summary>
        /// Actives temporal immunity to hits for the given
        /// <see cref="DynamicActor.ImmunityTime"/>
        /// </summary>
        public void TemporalImmunity()
        {
            _immunityActive = true;

            // disables immunity after immunity time
            if (_player != null)
            {
                CoroutineUtils.DelaySeconds(() =>
                {
                    _immunityActive = false;
                }, _player.ImmunityTime).Start();
            }
        }

        /// <summary>
        /// Reduces the player's health points and activates temporal immunity
        /// </summary>
        public void Hit()
        {
            if (Health <= 0 || _immunityActive) return;

            Health--;
            TemporalImmunity();
            EventManager.TriggerEvent("HealthReduced");

            if (Health == 0)
            {
                EventManager.TriggerEvent("PlayerDied");
            }
        }

        public void Restart()
        {
            Health = _gameplay.HealthPoints;
        }
    }
}
