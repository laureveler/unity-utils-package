using System;
using System.Collections.Generic;

namespace Utilities
{
    public class TaskQueue
    {
        private readonly Queue<Action<Action>> _taskQueue = new Queue<Action<Action>>();
        private bool _isProcessing;

        public void Enqueue(Action<Action> task)
        {
            _taskQueue.Enqueue(task);
            if (!_isProcessing)
            {
                ProcessNextTask();
            }
        }

        private void ProcessNextTask()
        {
            if (_taskQueue.Count > 0)
            {
                _isProcessing = true;
                var task = _taskQueue.Dequeue();
                task(() =>
                {
                    _isProcessing = false;
                    ProcessNextTask();
                });
            }
            else
            {
                _isProcessing = false;
            }
        }
    }
}