using UnityEngine;

namespace Utilities.ObjectPooling
{
    public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
    {
        public static Transform PoolsParent => Instance.transform;
    }
}