using System;
using System.Collections.Generic;

namespace Utilities
{
    public class Updater : MonoSingleton<Updater>
    {
        readonly List<Action> _updateActions = new List<Action>();
        readonly List<Action> _fixedUpdateActions = new List<Action>();

        int _updateActionCount = 0;
        int _fixedUpdateActionCount = 0;

        public void AddUpdateAction(Action action)
        {
            _updateActions.Add(action);
            _updateActionCount++;
        }

        public void RemoveUpdateAction(Action action)
        {
            _updateActions.Remove(action);
            _updateActionCount--;
        }

        public void AddFixedUpdateAction(Action action)
        {
            _fixedUpdateActions.Add(action);
            _fixedUpdateActionCount++;
        }

        public void RemoveFixedUpdateAction(Action action)
        {
            if (!_fixedUpdateActions.Contains(action))
                return;

            _fixedUpdateActions.Remove(action);
            _fixedUpdateActionCount--;
        }

        void Update()
        {
            if (_updateActionCount == 0)
                return;

            foreach (var action in _updateActions)
            {
                action?.Invoke();
            }
        }

        void FixedUpdate()
        {
            if (_fixedUpdateActionCount == 0)
                return;

            foreach (var action in _fixedUpdateActions)
            {
                action?.Invoke();
            }
        }
    }
}
