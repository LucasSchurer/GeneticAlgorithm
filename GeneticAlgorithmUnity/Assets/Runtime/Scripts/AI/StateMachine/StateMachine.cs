using Game.AI.States;
using Game.Entities;
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
        private bool _initializeOnStart = false;
        [SerializeField]
        private bool _debug = false;
        [SerializeField]
        private string _currentStateName;
        [SerializeField]
        private StateData _currentStateData;
        [SerializeField]
        private StateMachineData _data;
        private Entity _entity;
        private EntityEventController _eventController;
        private AttributeController _attributeController;
        private NonPersistentAttribute _health;

        private State _initialState;
        private State _currentState;

        private bool _canMove = true;
        private bool _hasStarted = false;

        private StateContext _pastContext;
        private StateContext _currentContext;

        public EntityEventController EventController => _eventController;
        public AttributeController AttributeController => _attributeController;
        public NonPersistentAttribute Health => _health != null ? _health : _health = AttributeController.GetNonPersistentAttribute(AttributeType.Health);

        public Entity Entity => _entity;

        public StateContext PastContext => _pastContext != null ? _pastContext : new StateContext();
        public StateContext CurrentContext => _currentContext != null ? _currentContext : new StateContext();

        public void Initialize(StateMachineData data)
        {
            _eventController = GetComponent<EntityEventController>();
            _attributeController = GetComponent<AttributeController>();
            _entity = GetComponent<Entity>();

            _data = data;
            _initialState = _data.GetInitialState(this);

            _currentState = _initialState;

            if (_currentState != null)
            {
                if (_debug)
                {
                    _currentStateName = _currentState.ToString();
                    _currentStateData = _currentState.StateDataDebug;
                    Debug.Log("Started " + _currentStateName);
                }

                _currentState.StateStart();
            }

            _hasStarted = true;
        }

        private void Start()
        {
            if (!_hasStarted && _initializeOnStart)
            {
                Initialize(_data);
            }
        }

        private void Update()
        {
            Debug.DrawRay(transform.position, transform.rotation * Vector3.forward);

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

        public void ChangeCurrentState(State state)
        {
            if (_debug)
            {
                Debug.Log("Finished " + _currentStateName);
            }

            _currentState.StateFinish();

            _pastContext = _currentContext;
            _currentContext = new StateContext();

            if (state != null)
            {
                _currentState = state;
            } else
            {
                if (_debug)
                {
                    _currentStateName = "null";
                }

                _currentState = null;

                return;
            }

            if (_debug)
            {
                _currentStateName = _currentState.ToString();
                _currentStateData = _currentState.StateDataDebug;
                Debug.Log("Started " + _currentStateName);
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
