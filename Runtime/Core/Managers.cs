using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities
{
    public static class Managers
    {
        private static readonly Dictionary<Type, object> _managers = new Dictionary<Type, object>();

        public static void Register<T>(T manager) where T : class
        {
            var type = typeof(T);
            if (_managers.ContainsKey(type))
            {
                Debug.LogWarning($"Manager of type {type.Name} is already registered. Overwriting.");
            }

            _managers[type] = manager;
        }

        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            if (_managers.TryGetValue(type, out var instance))
            {
                return instance as T;
            }

            Debug.LogWarning($"Manager of type {type.Name} has not been registered.");
            return null;
        }

        public static void Unregister<T>() where T : class
        {
            _managers.Remove(typeof(T));
        }

        public static void Clear()
        {
            _managers.Clear();
        }
    }

    public abstract class Manager<T> : Disposable where T : Manager<T>
    {
        protected Manager()
        {
            Managers.Register(this as T);
        }

        protected override void DisposeManagedResources()
        {
            Managers.Unregister<T>();
        }
    }
}

