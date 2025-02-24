using System;
using UnityEngine;

namespace Utilities.ObjectPooling
{
    /// <summary>
    /// Base class for objects that can be pooled.
    /// </summary>
    public class PoolableObject : MonoBehaviour
    {
        public bool IsInPool => _isInPool;
        bool _isInPool = true;

        /// <summary>
        /// Called when the object is retrieved from the pool.
        /// </summary>
        public virtual void OnRetrieveFromPool()
        {
            if (!_isInPool)
            {
                Debug.LogError($"Object '{gameObject.name}' is being retrieved from the pool but is not in the pool. This should never happen.");
                return;
            }

            _isInPool = false;
        }

        /// <summary>
        /// Called when the object is returned to the pool.
        /// </summary>
        public virtual void OnReturnToPool()
        {
            if (_isInPool)
            {
                Debug.LogError($"Object '{gameObject.name}' is being returned to the pool but is already in the pool. This should never happen.");
                return;
            }

            _isInPool = true;
        }
    }
}
