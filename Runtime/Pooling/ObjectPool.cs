using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.ObjectPooling
{
    /// <summary>
    /// A generic object pool implementation.
    /// </summary>
    public class ObjectPool<T> : Disposable where T : PoolableObject
    {
        private readonly List<T> _pool;
        private readonly HashSet<T> _activeObjects;
        private readonly HashSet<T> _allObjects;
        private List<T> _activeObjectsCopy;

        private readonly Action<T> _onRetrieve;
        private readonly Action<T> _onReturn;
        private readonly Func<T> _createObject;
        private readonly Transform _parentTransform;

        public HashSet<T> GetActiveObjects => _activeObjects;

        /// <summary>
        /// Constructs an object pool.
        /// </summary>
        public ObjectPool(Func<T> createObject, int initialSize = 0, Action<T> onRetrieve = null, Action<T> onReturn = null, string poolParentName = "")
        {
            _pool = new List<T>();
            _activeObjects = new HashSet<T>();
            _allObjects = new HashSet<T>();
            _activeObjectsCopy = new List<T>();

            _createObject = createObject ?? throw new ArgumentNullException(nameof(createObject));
            _onRetrieve = onRetrieve;
            _onReturn = onReturn;

            // Create a parent transform to hold inactive objects
            string name = string.IsNullOrEmpty(poolParentName) ? $"{typeof(T).Name} Pool" : poolParentName;
            GameObject parentObject = new GameObject(name);
            parentObject.transform.SetParent(ObjectPoolManager.PoolsParent);

            _parentTransform = parentObject.transform;

            // Pre-populate the pool with the initial size
            for (int i = 0; i < initialSize; i++)
            {
                T obj = _createObject();
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(_parentTransform);
                _allObjects.Add(obj);
                _pool.Add(obj);
            }
        }

        /// <summary>
        /// Retrieves an object from the pool.
        /// </summary>
        public T Get()
        {
            T obj;

            if (_pool.Count > 0)
            {                
                obj = _pool[0];
            }
            else
            {
                obj = _createObject();
                obj.gameObject.SetActive(false);
                obj.transform.SetParent(_parentTransform);
                _allObjects.Add(obj);
                _pool.Add(obj);
                return Get();
            }

            if (!obj.IsInPool)
            {
                Debug.LogError("Object is not in the pool. This should never happen.");
            }

            if (_activeObjects.Contains(obj))
            {
                Debug.LogError("Object is already active. This should never happen.");
            }

            _pool.Remove(obj);
            obj.OnRetrieveFromPool();
            obj.gameObject.SetActive(true);
            _activeObjects.Add(obj);
            _onRetrieve?.Invoke(obj);

            return obj;
        }

        /// <summary>
        /// Returns an object to the pool.
        /// </summary>
        public void Return(T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Cannot return a null object to the pool.");
            }

            if (obj.IsInPool)
            {
                Debug.LogError($"Object {obj.name} is already in the pool. This should never happen. Duplicate return?");
                return;
            }

            if (!_allObjects.Contains(obj))
            {
                Debug.LogError($"Object {obj.name} is being returned to the pool but is not in the all objects list. This should never happen.");
                return;
            }

            if (!_activeObjects.Contains(obj))
            {
                Debug.LogError($"Object {obj.name}, id: {obj.GetInstanceID()} is being returned to the pool but is not in the active objects list. This is a duplicate return.");
                return;
            }

            if (_pool.Contains(obj))
            {
                Debug.LogError($"Object {obj.name} is being returned to the pool but is already in the pool. This should never happen.");
                return;
            }

            _activeObjects.Remove(obj);
            obj.OnReturnToPool();

            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_parentTransform); // Reattach to the pool hierarchy

            _onReturn?.Invoke(obj);
            _pool.Add(obj);
        }

        /// <summary>
        /// Returns all active objects to the pool.
        /// </summary>
        public void ReturnAll()
        {
            _activeObjectsCopy.Clear();
            _activeObjectsCopy = new List<T>(_activeObjects);

            foreach (T obj in _activeObjectsCopy)
            {
                Return(obj);
            }
        }

        /// <summary>
        /// Clears the pool and disposes all objects.
        /// </summary>
        public void Clear()
        {
            while (_pool.Count > 0)
            {
                T obj = _pool[0];
                _pool.Remove(obj);
                if (obj != null)
                {                    
                    GameObject.Destroy(obj.gameObject);
                }
            }

            _activeObjects.Clear();
            if (_parentTransform != null)
            {
                GameObject.Destroy(_parentTransform.gameObject);
            }
        }

        /// <summary>
        /// Disposes of the managed resources.
        /// </summary>
        protected override void DisposeManagedResources()
        {
            Clear();
        }
    }
}