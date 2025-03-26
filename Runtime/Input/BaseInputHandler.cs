using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utilities.Input
{
    public interface IBaseInputHandler
    {
        public void EnableActionMap(string mapName);
        public void DisableActionMap(string mapName);
    }

    /// <summary>
    /// This class is responsible for handling input actions and invoking events when an action is performed.
    /// It should be inherited from and setup in the project to handle specific input actions.
    /// </summary>
    public abstract class BaseInputHandler : MonoSingleton<BaseInputHandler> , IBaseInputHandler
    {
        [SerializeField] protected InputActionAsset _actionAsset;
        [SerializeField] bool _debugEnabled;

        protected abstract void HandleActionPerformed(string actionName, InputAction.CallbackContext context);

        protected const string UI_ACTIONMAP = "UI";

        public InputActionAsset GetActionAsset() => _actionAsset;

        readonly Dictionary<string, ActionMapHandler> _actionMapHandlers = new Dictionary<string, ActionMapHandler>();

        protected virtual void Start()
        {
            foreach (var map in _actionAsset.actionMaps)
            {
                if (map.name == UI_ACTIONMAP)
                    continue;

                _actionMapHandlers.Add(map.name, new ActionMapHandler(map, _debugEnabled));
            }

            foreach (var entry in _actionMapHandlers)
            {
                entry.Value.ActionPerformed += HandleActionPerformed;
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (var entry in _actionMapHandlers)
            {
                entry.Value.ActionPerformed -= HandleActionPerformed;
                entry.Value.Dispose();
            }
        }

        public void EnableActionMap(string mapName)
        {
            if (_actionMapHandlers.TryGetValue(mapName, out var handler))
            {
                handler.Enable();
            }
            else
            {
                Debug.LogWarning($"ActionMap '{mapName}' not found.");
            }
        }

        public void DisableActionMap(string mapName)
        {
            if (_actionMapHandlers.TryGetValue(mapName, out var handler))
            {
                handler.Disable();
            }
            else
            {
                Debug.LogWarning($"ActionMap '{mapName}' not found.");
            }
        }
    }
}

