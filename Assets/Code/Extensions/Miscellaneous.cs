using System.Collections;
using General;
using UnityEngine;

namespace Extensions
{
    /// <summary>
    /// Collection of extension methods
    /// </summary>
    public static class Miscellaneous
    {
        /// <summary>
        /// Tries to obtain a neccesary component for a behavior to work, first looks into the
        /// behavior's components, then the behavior's gameObject children. Disables the behavior
        /// if the component is not found since it's considered vital for the behavior to work.
        /// </summary>
        /// <param name="behavior">Behavior.</param>
        /// <param name="target">Target.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static void GetNeededComponent<T>(this Behaviour behavior,
                ref T target) where T : Component
        {
            if (null != target) return;

            target = behavior.GetComponent<T>();

            if (target != null) return;

            target = behavior.GetComponentInChildren<T>();

            if (target != null) return;

            Debug.LogError("No " + typeof(T) + "found. Disabling script...");
            behavior.enabled = false;
        }

        /// <summary>
        /// Determines whether the actor is on the ground, on top of a collider
        /// </summary>
        /// <returns>
        ///  <c>true</c> if the actor is on top of a collider; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="actor">The actor.</param>
        public static bool IsGrounded(this Collider actor)
        {
            return Physics.Raycast(actor.transform.position, -Vector3.up,
                                   actor.bounds.extents.y + 0.1f);
        }

        /// <summary>
        /// Starts the specified coroutine with the <see cref="CoroutineUtils"/>
        /// instance
        /// </summary>
        /// <param name="coroutine">The coroutine.</param>
        /// <returns></returns>
        public static Coroutine Start(this IEnumerator coroutine)
        {
            return CoroutineUtils.Instance.StartCoroutine(coroutine);
        }
    }
}

