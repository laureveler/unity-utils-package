using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.SceneManagement
{
    public class SceneLoader : Singleton<SceneLoader>
    {
        private HashSet<int> _loadedScenes = new HashSet<int>();

        public SceneLoader() : base() 
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;

            for (int i=0; i< SceneManager.sceneCount; i++)
            {
                _loadedScenes.Add(SceneManager.GetSceneAt(i).buildIndex);
            }
        }

        ~SceneLoader()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        public void LoadScene(int scene)
        {
            if (!IsValidSceneIndex(scene) || SceneAlreadyLoaded(scene)) 
                return;

            SceneManager.LoadScene(scene);
        }

        public void LoadSceneAsync(int scene, Action onComplete, Action onError = null)
        {
            if (!IsValidSceneIndex(scene) || SceneAlreadyLoaded(scene))
            {
                onError?.Invoke();
                return;
            }

            CoroutineUtil.Instance.StartCoroutine(LoadSceneAsyncRoutine(scene, LoadSceneMode.Single, onComplete));
        }

        public void LoadSceneAdditive(int scene)
        {
            if (!IsValidSceneIndex(scene) || SceneAlreadyLoaded(scene))
                return;

            SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        }

        public void LoadSceneAdditiveAsync(int scene, Action onComplete, Action onError = null)
        {
            if (!IsValidSceneIndex(scene) || SceneAlreadyLoaded(scene))
            {
                onError?.Invoke();
                return;
            }

            CoroutineUtil.Instance.StartCoroutine(LoadSceneAsyncRoutine(scene, LoadSceneMode.Additive, onComplete));
        }

        public void UnloadSceneAsync(int scene, Action onComplete, Action onError = null)
        {
            if (!IsValidSceneIndex(scene) || SceneNotLoaded(scene))
            {
                onError?.Invoke();
                return;
            }                

            CoroutineUtil.Instance.StartCoroutine(UnloadSceneAsyncRoutine(scene, onComplete));
        }

        public void LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (currentSceneIndex + 1 >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError("No next scene available. Already at the last scene.");
                return;
            }

            SceneManager.LoadScene(currentSceneIndex + 1);
        }

        public void ReloadCurrentScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        public void QuitApplication()
        {
            Application.Quit();
#if UNITY_EDITOR
            // If we are running in the Unity editor
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        private bool IsValidSceneIndex(int scene)
        {
            if (scene < 0 || scene >= SceneManager.sceneCountInBuildSettings)
            {
                Debug.LogError($"Invalid scene index {scene}. Ensure it's within valid range (0-{SceneManager.sceneCountInBuildSettings - 1}).");
                return false;
            }

            return true;
        }

        private bool SceneAlreadyLoaded(int scene)
        {
            if (_loadedScenes.Contains(scene))
            {
                Debug.LogWarning($"Scene {scene} is already loaded.");
                return true;
            }

            return false;
        }

        private bool SceneNotLoaded(int scene)
        {
            if (!_loadedScenes.Contains(scene))
            {
                Debug.LogWarning($"Scene {scene} is not loaded.");
                return true;
            }

            return false;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _loadedScenes.Add(scene.buildIndex);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            _loadedScenes.Remove(scene.buildIndex);
        }

        IEnumerator UnloadSceneAsyncRoutine(int scene, Action onComplete)
        {
            var asyncOp = SceneManager.UnloadSceneAsync(scene);
            while (!asyncOp.isDone)
            {
                yield return null;
            }

            onComplete?.Invoke();
        }

        IEnumerator LoadSceneAsyncRoutine(int scene, LoadSceneMode mode, Action onComplete)
        {
            var asyncOp = SceneManager.LoadSceneAsync(scene, mode);
            while (!asyncOp.isDone)
            {
                yield return null;
            }

            onComplete?.Invoke();
        }
    }
}
