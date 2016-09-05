using UnityEngine;
using System.Collections;

public static class BehaviorExtension
{
    /// <summary>
    /// Tries to obtain a neccesary component for a behavior to work, first looks into the
    /// behavior's components, then the behavior's gameObject children. Disables the behavior
    /// if the component is not found since it's vital for the behavior to work.
    /// </summary>
    /// <param name="behavior">Behavior.</param>
    /// <param name="target">Target.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static void GetNeededComponent<T>(this Behaviour behavior, ref T target) where T : Component
    {
        target = behavior.GetComponent<T>();

        if (target == null)
        {
            target = behavior.GetComponentInChildren<T>();

            if (target == null)
            {
                Debug.LogError("No " + typeof(T) + "found. Disabling script...");
                behavior.enabled = false;
            }
        }
    }
}

