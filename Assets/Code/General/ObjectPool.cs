using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace General
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField]
        private GameObject _poolObject;
        [SerializeField]
        private int _capacity;

        private Stack<GameObject> _inactives;
        private Queue<GameObject> _actives;

        private static Dictionary<GameObject, ObjectPool> _pools = new
        Dictionary<GameObject, ObjectPool>();

        /// <summary>
        /// Initializes a <see cref="ObjectPool"/> class.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        /// <param name="capacity">The capacity.</param>
        protected void Initialize(GameObject poolObject, int capacity)
        {
            _poolObject = poolObject;
            _capacity = capacity;
            _inactives = new Stack<GameObject>(_capacity);
            _actives = new Queue<GameObject>(_capacity);
        }

        private void Start()
        {
            DontDestroyOnLoad(transform.gameObject);

            if (null == _poolObject)
            {
                Debug.LogError("The given poolObject is null. Destroying this object...");
                Destroy(gameObject);
                return;
            }

            if (null != _poolObject && !_pools.ContainsKey(_poolObject))
            {
                _pools.Add(_poolObject, this);
            }
            else
            {
                Debug.LogWarning("There is already an " + typeof(ObjectPool) +
                                 " for " + _poolObject + ". Destroying this object...");
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Spawns this pool's <see cref="_poolObject"/>
        /// with the corresponding parameters
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="rotation">The rotation.</param>
        /// <param name="localScale">The local scale.</param>
        /// <returns></returns>
        public GameObject Spawn(Vector3 position = default(Vector3),
                                Quaternion rotation = default(Quaternion),
                                Vector3? localScale = null)
        {
            if (localScale == null) { localScale = new Vector3(1, 1, 1); }

            GameObject obj;
            PoolMember member = null;

            if (_actives.Count < _capacity)
            {
                if (_inactives.Count == 0)
                {
                    // the object isn't on the pool, thus it's instantiated
                    obj = Instantiate(_poolObject);
                    obj.name = _poolObject.name + " (" + obj.GetInstanceID() + ")";
                    obj.transform.SetParent(transform);
                    // link with this object pool
                    member = obj.GetComponent<PoolMember>();

                    if (null == member)
                    {
                        obj.AddComponent<PoolMember>().Pool = this;
                    }
                    else
                    {
                        member.Pool = this;
                    }
                }
                else
                {
                    // grab the last inactive
                    obj = _inactives.Pop();
                }
            }
            else
            {
                obj = _actives.Dequeue();
            }

            // the expected obj has been destroyed somewhere else
            if (obj == null)
            {
                return Spawn(position, rotation, localScale);
            }

            if (null != member)
            {
                member.OnSpawn();
            }
            else
            {
                obj.GetComponent<PoolMember>().OnSpawn();
            }

            // set transform parameters
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.localScale = localScale.Value;
            obj.SetActive(true);
            _actives.Enqueue(obj);
            return obj;
        }

        /// <summary>
        /// Despawns the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public void Despawn(GameObject obj)
        {
            PoolMember pm = obj.GetComponent<PoolMember>();

            if (null == pm)
            {
                Debug.LogError("Object " + obj + "wasn't spawned from a pool. " +
                               "Destroying instead...");
                Destroy(obj);
            }
            else
            {
                pm.Pool.Despawn(pm);
            }
        }

        /// <summary>
        /// Puts the despawned object back to the inactives stack
        /// and deactivates it's <see cref="GameObject"/>
        /// </summary>
        /// <param name="member">The member.</param>
        private void Despawn(PoolMember member)
        {
            member.gameObject.SetActive(false);
            member.OnDespawn();
            _inactives.Push(member.gameObject);
        }

        /// <summary>
        /// Instances the specified pool object. If an <see cref="ObjectPool"/>
        /// for <see cref="poolObject"/> already exist, that <see cref="ObjectPool"/>
        /// is returned instead.
        /// </summary>
        /// <param name="poolObject">The pool object.</param>
        /// <param name="capacity">The pool capacity.</param>
        /// <returns></returns>
        public static ObjectPool Instance(GameObject poolObject, int capacity = 5)
        {
            if (_pools.ContainsKey(poolObject))
            {
                return _pools[poolObject];
            }

            // instance new pool
            var go = new GameObject();
            var pool = go.AddComponent<ObjectPool>();
            pool.Initialize(poolObject, capacity);
            go.name = typeof(ObjectPool) + " (" + poolObject + ")";
            return pool;
        }

        /// <summary>
        /// Pre-instances all the pool members objects
        /// </summary>
        public void Preload()
        {
            GameObject[] objs = new GameObject[_capacity];

            // spawn all the pool members
            for (int i = 0; i < _capacity; i++)
            {
                objs[i] = Spawn();
            }

            // now despawn them
            for (int i = 0; i < _capacity; i++)
            {
                Despawn(objs[i]);
            }
        }
    }
}
