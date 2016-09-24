using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace General
{
    /// <summary>
    /// This <see cref="MonoBehaviour"/> instance can be used to
    /// run coroutines outside a <see cref="MonoBehaviour"/> class
    /// </summary>
    /// <seealso cref="CoroutineUtils" />
    public class CoroutineUtils : Singleton<CoroutineUtils>
    {
        /// <summary>
        /// Chains the specified coroutines.
        /// </summary>
        /// <param name="actions">The coroutines.</param>
        /// <returns></returns>
        public static IEnumerator Chain(params IEnumerator[] actions)
        {
            foreach (var action in actions)
            {
                yield return Instance.StartCoroutine(action);
            }
        }

        /// <summary>
        /// Executes the given action after a delay
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="delay">The delay.</param>
        /// <returns></returns>
        public static IEnumerator DelaySeconds(UnityAction action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        /// <summary>
        /// Executes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns></returns>
        public static IEnumerator Do(UnityAction action)
        {
            action.Invoke();
            yield return 0;
        }
    }
}
