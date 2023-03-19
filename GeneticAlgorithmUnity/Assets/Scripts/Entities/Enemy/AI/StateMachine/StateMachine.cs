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

        private State _initialState;
        private State _defaultState;
        private MovementController _movementController;
        private EntityEventController _eventController;
        private State _currentState;

        [SerializeField]
        private List<StateTransition> _transitions;

        private Dictionary<StateType, State> _states;

        private BehaviourType _behaviourType = BehaviourType.Aggressive;

        public MovementController MovementController => _movementController;

        public BehaviourType BehaviourType { get => _behaviourType; set => _behaviourType = value; }

        private void Awake()
        {
            _movementController = GetComponent<MovementController>();
            _eventController = GetComponent<EntityEventController>();
            _states = new Dictionary<StateType, State>();
            _initialState = new IdleState(this);
            _defaultState = _initialState;
            _states.Add(StateType.Idle, _initialState);
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
                State newState = GetEnemyState(this, type);
                
                if (newState != null)
                {
                    _states.Add(type, newState);

                    _currentState = newState;
                }
            }
            
            _currentState.StateStart();
        }

        public static State GetEnemyState(StateMachine stateMachine, StateType type)
        {
            switch (type)
            {
                case StateType.Idle:
                    return new IdleState(stateMachine);
                case StateType.Run:
                    return new RunState(stateMachine);
                case StateType.Chase:
                    return new ChaseState(stateMachine);
            }

            return null;
        }

        public StateType GetTransitionState(StateType fromState)
        {
            StateType toState = _defaultState.GetStateType();

            foreach (StateTransition transition in _transitions)
            {
                if (transition.FromState == fromState)
                {
                    return transition.GetTransition();
                }
            }

            return toState;
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
