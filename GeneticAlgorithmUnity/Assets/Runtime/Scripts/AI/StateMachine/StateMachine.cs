using Game.Entities.Shared;
using Game.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI
{
    public class StateMachine : MonoBehaviour, IEventListener, IEntityController
    {
        [Tooltip("If initialized on awake, it will need a StateMachineData assigned")]
        [SerializeField]
        private bool _initializeOnAwake = false;
        [SerializeField]
        private StateMachineData _data;
        private EntityEventController _eventController;

        private State _initialState;
        private State _defaultState;
        private State _currentState;

        private Dictionary<StateType, State> _states;

        private bool _canMove = true;
        private bool _hasStarted = false;

        public EntityEventController EventController => _eventController;

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();

            if (_initializeOnAwake)
            {
                Initialize(_data);
            }
        }

        public void Initialize(StateMachineData data)
        {
            _data = data;
            _initialState = _data.GetInitialState(this);
            _defaultState = _data.GetDefaultState(this);
            _states = _data.GetStates(this);

            _currentState = _initialState;

            Start();
        }

        private void Start()
        {   
            if (_currentState != null && !_hasStarted)
            {
                _currentState.StateStart();
                _hasStarted = true;
            }
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

        public Vector3 GetLookDirection()
        {
            if (_currentState != null)
            {
                return _currentState.GetLookDirection();
            } else
            {
                return Vector3.zero;
            }
        }

        public void SetCanMove(bool canMove)
        {
            _canMove = canMove;
        }

        public bool CanMove()
        {
            return _canMove;   
        }
    }
}
