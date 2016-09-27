using System;
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
        /// Obtains the surface normal below a collider if raycast is sucessfull
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <param name="groundNormal">The ground normal.</param>
        /// <returns></returns>
        public static bool GroundNormal(this Collider actor, out Vector3 groundNormal)
        {
            RaycastHit hit;
            Ray ray = new Ray(actor.transform.position, -Vector3.up);
            bool ground = Physics.Raycast(ray, out hit, actor.bounds.extents.y + 0.1f);
            groundNormal = ground ? hit.normal : Vector3.zero;
            return ground;
        }

        /// <summary>
        /// Determines whether the actor is squeezed between two colliders
        /// </summary>
        /// <param name="actor">The actor.</param>
        /// <returns>
        ///   <c>true</c> if the specified actor is squashed; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSquashed(this Collider actor)
        {
            bool top = Physics.Raycast(actor.transform.position, Vector3.up,
                                       actor.bounds.extents.y + 0.1f);
            return top && actor.IsGrounded();
        }

        /// <summary>
        /// Copies the transform parameters from source.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void Copy(this Transform target, Transform source)
        {
            target.position = source.position;
            target.rotation = source.rotation;
            target.localScale = source.localScale;
        }

        /// <summary>
        /// Starts the specified coroutine on the <see cref="CoroutineUtils"/>
        /// instance
        /// </summary>
        /// <param name="coroutine">The coroutine.</param>
        /// <returns></returns>
        public static Coroutine Start(this IEnumerator coroutine)
        {
            if (null == coroutine) return null;

            return CoroutineUtils.Instance.StartCoroutine(coroutine);
        }

        /// <summary>
        /// Stops the specified coroutine on the <see cref="CoroutineUtils"/>
        /// instance
        /// </summary>
        /// <param name="coroutine">The coroutine.</param>
        /// <returns></returns>
        public static void Stop(this Coroutine coroutine)
        {
            if (null == coroutine) return;

            CoroutineUtils.Instance.StopCoroutine(coroutine);
        }

        /// <summary>
        /// Removes a element from an array and returns the resized array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            T[] dest = new T[source.Length - 1];

            if (index > 0)
            {
                Array.Copy(source, 0, dest, 0, index);
            }

            if (index < source.Length - 1)
            {
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);
            }

            return dest;
        }
    }
}

