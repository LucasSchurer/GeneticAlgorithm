using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Events
{
    public class Event<Context>
        where Context: EventContext
    {
        public delegate void Delegate(ref Context ctx);

        private Delegate _before;
        private Delegate _standard;
        private Delegate _after;

        public void AddListener(Delegate callback, EventExecutionOrder order)
        {
            switch (order)
            {
                case EventExecutionOrder.Before:
                    _before += callback;
                    break;
                case EventExecutionOrder.Standard:
                    _standard += callback;
                    break;
                case EventExecutionOrder.After:
                    _after += callback;
                    break;
                default:
                    _standard += callback;
                    break;
            }
        }

        public void RemoveListener(Delegate callback, EventExecutionOrder order)
        {
            switch (order)
            {
                case EventExecutionOrder.Before:
                    _before -= callback;
                    break;
                case EventExecutionOrder.Standard:
                    _standard -= callback;
                    break;
                case EventExecutionOrder.After:
                    _after -= callback;
                    break;
            }
        }

        public void Invoke(Context ctx)
        {
            _before?.Invoke(ref ctx);
            _standard?.Invoke(ref ctx);
            _after?.Invoke(ref ctx);
        }
    }
}
