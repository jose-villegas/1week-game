using Actors;
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
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    /// <seealso cref="Interfaces.IHittable" />
    [RequireComponent(typeof(Collider))]
    public class PlayerHealth : MonoBehaviour, IHittable, IRestartable
    {
        private PlayerActor _player;
        private bool _immunityActive;

        public int Health { get; private set; }

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
                Health = _player.HealthPoints;
            }

            // event to receive hits from other objects
            EventManager.StartListening("HitPlayer", Hit);
            // restore health on level reset
            EventManager.StartListening("LevelReset", Restart);
        }

        /// <summary>
        /// Actives temporal immunity to hits for the given
        /// <see cref="DynamicActor.AfterHitImmunityTime"/>
        /// </summary>
        public void TemporalImmunity()
        {
            _immunityActive = true;
            // disables immunity after immunity time
            CoroutineUtils.DelaySeconds(() =>
            {
                _immunityActive = false;
            }, _player.AfterHitImmunityTime).Start();
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
        }

        public void Restart()
        {
            Health = _player.HealthPoints;
        }
    }
}
