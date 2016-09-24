using System.Collections;
using General;
using UnityEngine;

namespace Behaviors
{
    /// <summary>
    /// Collects all the coin objects that collide with the GameObject
    /// collider component, adding to the coin score. Also destroys the
    /// coin's GameObject after they are collected
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Collider))]
    public class CoinCollector : MonoBehaviour
    {
        /// <summary>
        /// Destroys the given object after 1.5 seconds
        /// </summary>
        /// <param name="coin">The coin.</param>
        /// <returns></returns>
        private static IEnumerator DestroyCoin(Object coin)
        {
            // wait a number of seconds for the animation to play
            // before flagging for destroy
            yield return new WaitForSeconds(1.5f);
            // mark for destroy
            Destroy(coin);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag("Coin")) return;

            col.gameObject.tag = "Untagged";
            // obtain coin animator to make it dissapear
            var anim = col.GetComponent<Animator>();

            // dissapear animation then destory the object
            if (null != anim)
            {
                anim.SetBool("Dissapear", true);
                StartCoroutine(DestroyCoin(col.gameObject));
            }

            EventManager.TriggerEvent("CoinCollected");
        }
    }
}
