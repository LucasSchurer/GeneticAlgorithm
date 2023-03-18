using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.AI
{
    public class EnemyStateMachine : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private EnemyState _initialState;
        private MovementController _movementController;
        private EntityEventController _eventController;
        private EnemyState _currentState;

        private Dictionary<EnemyStateType, EnemyState> _states;

        private BehaviourType _behaviourType = BehaviourType.Aggressive;

        public MovementController MovementController => _movementController;

        public BehaviourType BehaviourType { get => _behaviourType; set => _behaviourType = value; }

        private void Awake()
        {
            _movementController = GetComponent<MovementController>();
            _eventController = GetComponent<EntityEventController>();
            _states = new Dictionary<EnemyStateType, EnemyState>();
            _initialState = new EnemyIdleState(this);
            _states.Add(EnemyStateType.Idle, _initialState);
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

        public void ChangeCurrentState(EnemyStateType type)
        {
            _currentState.StateFinish();

            if (_states.TryGetValue(type, out EnemyState state))
            {
                _currentState = state;
            } else
            {
                EnemyState newState = GetEnemyState(this, type);
                
                if (newState != null)
                {
                    _states.Add(type, newState);

                    _currentState = newState;
                }
            }
            
            _currentState.StateStart();
        }

        public static EnemyState GetEnemyState(EnemyStateMachine stateMachine, EnemyStateType type)
        {
            switch (type)
            {
                case EnemyStateType.Idle:
                    return new EnemyIdleState(stateMachine);
                case EnemyStateType.Run:
                    return new EnemyRunState(stateMachine);
                case EnemyStateType.Chase:
                    return new EnemyChaseState(stateMachine);
            }

            return null;
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
