using System.Collections;
using UI;
using UnityEngine;

namespace Behaviors
{
    [RequireComponent(typeof(Collider))]
    public class CoinCollector : MonoBehaviour
    {
        private CollectedCoinsUI _collectedCoinsUI;

        // Use this for initialization
        private void Start ()
        {
            _collectedCoinsUI = FindObjectOfType<CollectedCoinsUI>();

            if (null == _collectedCoinsUI)
            {
                Debug.Log("No " + typeof(CollectedCoinsUI) + " found. " +
                          "Disabling script...");
                enabled = false;
            }
        }

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
            if (col.gameObject.tag == "Coin")
            {
                col.gameObject.tag = "Untagged";
                // obtain coin animator to make it dissapear
                var anim = col.GetComponent<Animator>();

                if (null != anim)
                {
                    anim.SetBool("Dissapear", true);
                    StartCoroutine(DestroyCoin(col.gameObject));
                }

                _collectedCoinsUI.ScoreCoin();
            }
        }
    }
}
