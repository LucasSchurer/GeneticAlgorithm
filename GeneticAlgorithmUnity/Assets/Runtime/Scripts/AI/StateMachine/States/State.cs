using Game.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    public abstract class State
    {
        protected delegate State ActionCallback();
        protected delegate ActionCallback GetCallback<A>(A action);

        protected StateMachine _stateMachine;

        private StateData _stateDataDebug;
        public StateData StateDataDebug => _stateDataDebug;

        public State(StateMachine stateMachine, StateData data)
        {
            _stateMachine = stateMachine;
            _stateDataDebug = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <param name="actions"></param>
        /// <param name="blockedActions"></param>
        /// <returns>True if an action change has occurred</returns>
        protected virtual bool CheckActions<A>(Dictionary<A, ActionCallback> actions, HashSet<A> blockedActions)
        {
            State nextState = null;

            foreach (KeyValuePair<A, ActionCallback> validAction in actions)
            {
                if (!blockedActions.Contains(validAction.Key))
                {
                    nextState = validAction.Value.Invoke();

                    if (nextState != null)
                    {
                        _stateMachine.ChangeCurrentState(nextState);
                        return true;
                    }
                } else
                {
                    actions.Remove(validAction.Key);
                }
            }

            return false;
        }

        protected virtual Dictionary<A, ActionCallback> BuildActionCallbackDictionary<A>(A[] validActions, GetCallback<A> getCallback)
        {
            Dictionary<A, ActionCallback> actions = new Dictionary<A, ActionCallback>();

            for (int i = 0; i < validActions.Length; i++)
            {
                ActionCallback callback = getCallback?.Invoke(validActions[i]);

                if (callback != null)
                {
                    actions.TryAdd(validActions[i], callback);
                }
            }

            return actions;
        }

        public abstract void StateStart();
        public abstract void StateUpdate();
        public abstract void StateFixedUpdate();
        public abstract void StateFinish();
        public abstract Vector3 GetLookDirection();
    }
}
