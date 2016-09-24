using System.Collections;
using Extensions;
using General;
using Interfaces;
using UnityEngine;

namespace Behaviors
{
    /// <summary>
    /// Describes a level coin meant to be collected by the player
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Collider))]
    public class CollectibleCoin : MonoBehaviour, IRestartable
    {
        private Animator _animator;
        private Collider _collider;
        private int _dissapearAnimation;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _collider = GetComponent<Collider>();
            _dissapearAnimation = Animator.StringToHash("Dissapear");
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag("Player")) return;

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
