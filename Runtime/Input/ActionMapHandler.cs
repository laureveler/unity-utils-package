using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utilities.Input
{
    public interface IActionMapHandler
    {
        event Action<string, InputAction.CallbackContext> ActionPerformed;

        bool IsValid { get; }

        void Enable();
        void Disable();
    }

    public class ActionMapHandler : Disposable, IActionMapHandler
    {
        public event Action<string, InputAction.CallbackContext> ActionPerformed;

        public bool IsValid => _actionMap != null;

        private readonly InputActionMap _actionMap;
        private readonly List<(InputAction action, Action<InputAction.CallbackContext> callback)> _subscriptions = new();
        private readonly bool _debug;

        private bool _enabled = false;

        public ActionMapHandler(InputActionMap map, bool debug = false)
        {
            _actionMap = map;
            _debug = debug;
            _enabled = map.enabled;

            if (_enabled)
                SubscribeToActions();
        }

        private void SubscribeToActions()
        {
            foreach (var action in _actionMap.actions)
            {
                Action<InputAction.CallbackContext> callback = context => OnActionPerformed(action.name, context);

                _subscriptions.Add((action, callback));
                action.performed += callback;
            }
        }

        private void UnsubscribeFromActions()
        {
            foreach (var (action, callback) in _subscriptions)
            {
                action.performed -= callback;
            }

            _subscriptions.Clear();
        }

        private void OnActionPerformed(string actionName, InputAction.CallbackContext context)
        {
            ActionPerformed?.Invoke(actionName, context);

            if (_debug)
            {
                Debug.Log($"Action performed: {actionName}, value: {context.ReadValueAsObject()}");
            }
        }

        protected override void DisposeManagedResources()
        {
            Disable();
        }

        public void Enable()
        {
            if (!IsValid)
            {
                Debug.LogError($"Cannot enable ActionMapHandler for '{_actionMap.name}' because it is invalid.");
                return;
            }

            if (!_enabled)
            {
                _actionMap.Enable();
                SubscribeToActions();

                if (_debug)
                {
                    Debug.Log($"ActionMap '{_actionMap.name}' enabled.");
                }

                _enabled = true;
            }
        }

        public void Disable()
        {
            if (!IsValid)
            {
                Debug.LogError($"Cannot disable ActionMapHandler for '{_actionMap.name}' because it is invalid.");
                return;
            }

            if (_enabled)
            {
                _actionMap.Disable();
                UnsubscribeFromActions();

                if (_debug)
                {
                    Debug.Log($"ActionMap '{_actionMap.name}' disabled.");
                }

                _enabled = false;
            }
        }
    }
}
