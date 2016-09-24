using System.Collections;
using Extensions;
using General;
using UnityEngine;

namespace Behaviors
{
    /// <summary>
    /// Describes a level coin meant to be collected by the player
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Collider))]
    public class CollectibleCoin : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag("Player")) return;

            // dissapear animation then destory the object
            if (null != _animator)
            {
                _animator.SetBool("Dissapear", true);
                // dissapear the game object after animation finishes
                CoroutineUtils.DelaySeconds(() =>
                {
                    gameObject.SetActive(false);
                }, 1.5f).Start();
            }

            EventManager.TriggerEvent("CoinCollected");
        }
    }
}
