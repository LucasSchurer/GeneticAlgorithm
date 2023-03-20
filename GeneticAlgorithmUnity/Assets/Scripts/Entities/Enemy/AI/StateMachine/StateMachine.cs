using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
{
    public class StateMachine : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private StateMachineData _data;
        private EntityEventController _eventController;

        private State _initialState;
        private State _defaultState;
        private State _currentState;

        private Dictionary<StateType, State> _states;

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
            _initialState = _data.GetInitialState(this);
            _defaultState = _data.GetDefaultState(this);
            _states = _data.GetStates(this);
        }

        private void Start()
        {
            _currentState = _initialState;
            _currentState.StateStart();
        }

        private void Update()
        {
            if (_currentState != null)
            {
                _currentState.StateUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (_currentState != null)
            {
                _currentState.StateFixedUpdate();
            }
        }

        public void ChangeCurrentState(StateType type)
        {
            _currentState.StateFinish();

            if (_states.TryGetValue(type, out State state))
            {
                _currentState = state;
            } else
            {
                _currentState = _defaultState;
            }
            
            _currentState.StateStart();

            Debug.Log($"Changed to {_currentState.GetStateType().ToString()} state");
        }

        private void OnDeath(ref EntityEventContext ctx)
        {
            if (_currentState != null)
            {
                _currentState.StateFinish();
                _currentState = null;
            }
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnDeath, OnDeath);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnDeath, OnDeath);
            }
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }
    }
}
