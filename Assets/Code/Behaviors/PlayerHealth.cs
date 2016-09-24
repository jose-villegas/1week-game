using Actors;
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
    public class PlayerHealth : MonoBehaviour, IHittable
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

            EventManager.StartListening("HitPlayer", Hit);
        }

        /// <summary>
        /// Actives temporal immunity to hits for the given
        /// <see cref="DynamicActor.AfterHitImmunityTime"/>
        /// </summary>
        public void TemporalImmunity()
        {
            _immunityActive = true;
            CancelInvoke("DisableImmunity");
            Invoke("DisableImmunity", _player.AfterHitImmunityTime);
        }

        /// <summary>
        /// Disables the temporal hit immunity.
        /// </summary>
        private void DisableImmunity()
        {
            _immunityActive = false;
        }

        public void Hit()
        {
            if (Health <= 0 || _immunityActive) return;

            Health--;
            TemporalImmunity();
            EventManager.TriggerEvent("HealthReduced");
        }
    }
}
