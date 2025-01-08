using UnityEngine;

namespace Utilities
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        public bool Initialised { get; protected set; }

        public static T Instance
        { 
            get
            {
                if (_instance == null)
                    CreateInstance();

                return _instance;
            } 
        }
        private static T _instance;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                CreateInstance();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected static void CreateInstance()
        {
            _instance = (T)FindObjectOfType(typeof(T), true);

            if (_instance == null)
            {
                var root = GetSingletonRoot();
                var go = new GameObject(typeof(T).Name);
                go.transform.SetParent(root.transform, false);
                _instance = go.AddComponent<T>();
            }
        }

        protected static GameObject GetSingletonRoot()
        {
            const string RootName = "MonoSingletons";
            var root = GameObject.Find(RootName);
            if (root == null)
            {
                root = new GameObject(RootName);
                root.transform.localPosition = Vector3.zero;
                root.transform.localRotation = Quaternion.identity;
            }

            return root;
        }
    }
}