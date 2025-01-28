using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneLoader : Singleton<SceneLoader>
{
    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene((int)scene);
    }

    public void LoadSceneAsync(Scene scene, Action onComplete)
    {
        CoroutineUtil.Instance.StartCoroutine(LoadSceneAsyncRoutine(scene, LoadSceneMode.Single, onComplete));
    }

    public void LoadSceneAdditive(Scene scene)
    {
        SceneManager.LoadScene((int)scene, LoadSceneMode.Additive);
    }

    public void LoadSceneAdditiveAsync(Scene scene, Action onComplete)
    {
        CoroutineUtil.Instance.StartCoroutine(LoadSceneAsyncRoutine(scene, LoadSceneMode.Additive, onComplete));
    }

    public void UnloadSceneAsync(Scene scene, Action onComplete)
    {
        CoroutineUtil.Instance.StartCoroutine(UnloadSceneAsyncRoutine(scene, onComplete));
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
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

    IEnumerator UnloadSceneAsyncRoutine(Scene scene, Action onComplete)
    {
        var asyncOp = SceneManager.UnloadSceneAsync((int)scene);
        while (!asyncOp.isDone)
        {
            yield return null;
        }

        onComplete?.Invoke();
    }

    IEnumerator LoadSceneAsyncRoutine(Scene scene, LoadSceneMode mode, Action onComplete)
    {
        var asyncOp = SceneManager.LoadSceneAsync((int)scene, mode);
        while (!asyncOp.isDone)
        {
            yield return null;
        }

        onComplete?.Invoke();
    }
}

public enum Scene
{
    Core = 0,
    MainMenu = 1,
    EndlessRunner = 2
}
