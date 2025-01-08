using System;
using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using Unity.EditorCoroutines.Editor;
#endif

namespace Utilities
{
    public class CoroutineUtil : MonoSingleton<CoroutineUtil>
    {
        public static bool IsEditorMode()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying && Application.isEditor)
            {
                return true;
            }
#endif
            return false;
        }

        public static Coroutine Start(IEnumerator routine)
        {
            if (Instance == null)
            {
                Debug.LogError("CoroutineUtil not found!");
                return null;
            }

            if (IsEditorMode())
            {
#if UNITY_EDITOR
                EditorCoroutineUtility.StartCoroutine(routine, Instance);
#endif
                return null;
            }
            else
            {
                return Instance.StartCoroutine(routine);
            }
        }

        public static void WaitFor(Func<bool> condition, Action callback)
        {
            Start(WaitForCondition(condition, callback));
        }

        public static void WaitForSeconds(WaitForSeconds waitTime, Action callback)
        {
            Start(WaitFor(waitTime, callback));
        }

        static IEnumerator WaitForCondition(Func<bool> condition, Action callback)
        {
            yield return new WaitUntil(() => condition());
            callback();
        }

        static IEnumerator WaitFor(WaitForSeconds waitTime, Action callback)
        {
            yield return waitTime;
            callback();
        }

        public static void Stop(Coroutine routine)
        {
            Instance.StopCoroutine(routine);
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}

