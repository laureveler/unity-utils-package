using UnityEngine;
using UnityEditor;

namespace Utilities.Input.Editor
{
    [CustomEditor(typeof(BaseInputHandler), true)]
    public class BaseInputHandlerEditor : UnityEditor.Editor
    {
        private BaseInputHandler _baseInputHandler;

        private void OnEnable()
        {
            _baseInputHandler = (BaseInputHandler)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (_baseInputHandler == null || _baseInputHandler.GetActionAsset() == null)
            {
                EditorGUILayout.HelpBox("InputActionAsset is not set in the InputSettings.", MessageType.Warning);
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Action Maps", EditorStyles.boldLabel);

            var actionAsset = _baseInputHandler.GetActionAsset();
            foreach (var actionMap in actionAsset.actionMaps)
            {
                GUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(actionMap.name);

                if (EditorApplication.isPlaying)
                {
                    if (actionMap.enabled)
                    {
                        if (GUILayout.Button("Disable"))
                        {
                            _baseInputHandler.DisableActionMap(actionMap.name);
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Enable"))
                        {
                            _baseInputHandler.EnableActionMap(actionMap.name);
                        }
                    }
                }
                else
                {
                    GUI.enabled = false;
                    GUILayout.Button("Enable/Disable (Play Mode Only)");
                    GUI.enabled = true;
                }

                GUILayout.EndHorizontal();
            }

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Action map controls are only functional in Play Mode.", MessageType.Info);
            }
        }
    }
}

