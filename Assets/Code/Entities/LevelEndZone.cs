using General;
using UnityEngine;

namespace Entities
{
    /// <summary>
    /// Describes an level advancement zone, once the players
    /// enters this area its ready to advance to a next level
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Collider))]
    public class LevelEndZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            if (!col.CompareTag("Player")) return;

            // user has reached the end of the level
            EventManager.TriggerEvent("GoToNextLevel");
        }
    }
}
