﻿using System.Collections;
using Extensions;
using General;
using Interfaces;
using UnityEngine;

namespace Behaviors
{
    /// <summary>
    /// Describes a level coin meant to be collected by the player
    /// </summary>
    /// <seealso cref="Interfaces.IRestartable" />
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Collider))]
    public class CollectibleCoin : MonoBehaviour, IRestartable
    {
        private AudioSource _audio;
        private Animator _animator;
        private Collider _collider;
        private int _dissapearAnimation;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
            _audio = GetComponent<AudioSource>();
            _dissapearAnimation = Animator.StringToHash("Dissapear");
            EventManager.StartListening("GamePaused", OnGamePaused);
            EventManager.StartListening("GameResumed", OnGameResumed);
        }

        private void OnGamePaused()
        {
            if (null != _animator && enabled)
            {
                _animator.enabled = false;
                _collider.enabled = false;
            }
        }

        private void OnGameResumed()
        {
            if (null != _animator && enabled)
            {
                _animator.enabled = true;
                _collider.enabled = true;
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            var rb = col.attachedRigidbody;

            if (null == rb || !rb.CompareTag("Player")) return;

            // dissapear animation then destory the object
            if (null != _animator)
            {
                _animator.SetBool(_dissapearAnimation, true);
                _collider.enabled = false;
                // dissapear the game object after animation finishes
                CoroutineUtils.DelaySeconds(() =>
                {
                    gameObject.SetActive(false);
                }, 1.5f).Start();
            }

            if(null != _audio) { _audio.Play(); }

            EventManager.TriggerEvent("CoinCollected");
        }

        public void Restart()
        {
            gameObject.SetActive(true);
            _collider.enabled = true;

            if (null != _animator)
            {
                _animator.SetBool(_dissapearAnimation, false);
            }
        }
    }
}
