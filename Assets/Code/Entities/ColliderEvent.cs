using UnityEngine;
using UnityEngine.Events;

namespace Entities
{
    /// <summary>
    /// Executes unity events on collider events
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(Collider))]
    class ColliderEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onTriggerEnter;
        [SerializeField] private UnityEvent _onTriggerExit;
        [SerializeField] private UnityEvent _onTriggerStay;

        [SerializeField] private UnityEvent _onCollisionEnter;
        [SerializeField] private UnityEvent _onCollisionExit;
        [SerializeField] private UnityEvent _onCollisionStay;

        public void OnCollisionEnter(Collision collision)
        {
            if (null != _onCollisionEnter) _onCollisionEnter.Invoke();
        }

        public void OnCollisionStay(Collision collision)
        {
            if (null != _onCollisionStay) _onCollisionStay.Invoke();
        }

        public void OnCollisionExit(Collision collision)
        {
            if (null != _onCollisionExit) _onCollisionExit.Invoke();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (null != _onTriggerEnter) _onTriggerEnter.Invoke();
        }

        public void OnTriggerStay(Collider other)
        {
            if (null != _onTriggerStay) _onTriggerStay.Invoke();
        }

        public void OnTriggerExit(Collider other)
        {
            if (null != _onTriggerExit) _onTriggerExit.Invoke();
        }
    }
}