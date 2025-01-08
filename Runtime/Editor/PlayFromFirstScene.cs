#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class PlayFromFirstScene : EditorWindow
{
    [MenuItem("Tools/Play From First Scene")]
    public static void PlayFromFirstSceneMenu()
    {
        if (EditorBuildSettings.scenes.Length > 0)
        {
            string firstScenePath = EditorBuildSettings.scenes[0].path;

            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(firstScenePath);                
                EditorApplication.isPlaying = true;
            }
        }
        else
        {
            Debug.LogError("No scenes are listed in the Build Settings.");
        }
    }
}
#endif