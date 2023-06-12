using Game.Entities;
using Game.Entities.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    public class MoveBasedOnTarget : State
    {
        private MoveBasedOnTargetData _data;
        private HashSet<MoveBasedOnTargetData.Action> _blockedActions;

        private Transform _target;
        private bool _isTargetDead = false;
        private Vector3 _targetPosition = Vector3.zero;
        private Vector3 _direction;
        private MovementController _movementController;
        private Vector3 _offset;

        private Coroutine _randomizeOffsetCoroutine;
        private Coroutine _updatePositionCoroutine;
        private Coroutine _checkActionsCoroutine;

        private AttributeController _targetAttributeController;

        public MoveBasedOnTarget(StateMachine stateMachine, MoveBasedOnTargetData data) : base(stateMachine, data)
        {
            _data = data;
            _blockedActions = new HashSet<MoveBasedOnTargetData.Action>();
            _movementController = stateMachine.GetComponent<MovementController>();
        }

        public override void StateStart()
        {
            if (_stateMachine.PastContext.Target != null)
            {
                _target = _stateMachine.PastContext.Target.Target;
                RandomizeOffset();

                if (_data.RandomizeOffsetMultipleTimes)
                {
                    _randomizeOffsetCoroutine = _stateMachine.StartCoroutine(RandomizeOffsetCoroutine());
                }

                if (_data.NewPositionInterval > 0)
                {
                    _updatePositionCoroutine = _stateMachine.StartCoroutine(UpdatePositionCoroutine());
                }

                if (_data.CheckActionsInterval > 0)
                {
                    _checkActionsCoroutine = _stateMachine.StartCoroutine(CheckActionsCoroutine());
                }

                _targetAttributeController = _target.GetComponent<AttributeController>();
            } else
            {
                _isTargetDead = true;
            }
        }

        public override void StateUpdate()
        {
            if (_data.NewPositionInterval <= 0)
            {
                UpdatePosition();
            }

            UpdateDirection();

            _isTargetDead = _target == null;

            if (_data.CheckActionsInterval <= 0)
            {
                CheckActions();
            }
        }

        public override void StateFixedUpdate()
        {
            if (_direction != Vector3.zero)
            {
                _movementController.Move(_direction);
            }
        }

        public override void StateFinish()
        {
            if (_randomizeOffsetCoroutine != null)
            {
                _stateMachine.StopCoroutine(_randomizeOffsetCoroutine);
            } 

            if (_updatePositionCoroutine != null)
            {
                _stateMachine.StopCoroutine(_updatePositionCoroutine);
            }

            if (_checkActionsCoroutine != null)
            {
                _stateMachine.StopCoroutine(_checkActionsCoroutine);
            }
        }

        public override Vector3 GetLookDirection()
        {
            return Vector3.zero;
        }

        private void UpdateDirection()
        {
            if (_targetPosition != Vector3.zero)
            {
                _direction = (_targetPosition - _stateMachine.transform.position).normalized;
            } else
            {
                _direction = Vector3.zero;
            }
        }

        private void UpdatePosition()
        {
            if (_target != null)
            {
                if (Vector3.Distance(_stateMachine.transform.position, _target.position) > _data.MinimumDistanceToNewPosition)
                {
                    if (_data.UseTargetViewDirection && _target.rotation.eulerAngles != Vector3.zero)
                    {
                        _targetPosition = _target.position + (_target.rotation * _offset);
                    } else
                    {
                        _targetPosition = _target.position + _offset;
                    }
                }
            }
        }

        private void RandomizeOffset()
        {
            float x;
            float y;
            float z;

            x = GetRandomizedFloat(_data.MinPositionOffset.x, _data.MaxPositionOffset.x);
            y = GetRandomizedFloat(_data.MinPositionOffset.y, _data.MaxPositionOffset.y);
            z = GetRandomizedFloat(_data.MinPositionOffset.z, _data.MaxPositionOffset.z);

            _offset = new Vector3(x, y, z);
        }

        private float GetRandomizedFloat(float minValue, float maxValue)
        {
            if (minValue > maxValue)
            {
                return 0f;
            } else if (minValue == maxValue)
            {
                return minValue;
            }
            else
            {
                return Random.Range(minValue, maxValue);
            }
        }

        private IEnumerator RandomizeOffsetCoroutine()
        {
            yield return new WaitForSeconds(_data.RandomizeOffsetInterval);

            RandomizeOffset();

            _randomizeOffsetCoroutine = _stateMachine.StartCoroutine(RandomizeOffsetCoroutine());
        }

        private IEnumerator UpdatePositionCoroutine()
        {
            yield return new WaitForSeconds(_data.NewPositionInterval);

            UpdatePosition();

            _updatePositionCoroutine = _stateMachine.StartCoroutine(UpdatePositionCoroutine());
        }

        private IEnumerator CheckActionsCoroutine()
        {
            yield return new WaitForSeconds(_data.CheckActionsInterval);

            CheckActions();

            _checkActionsCoroutine = _stateMachine.StartCoroutine(CheckActionsCoroutine());
        }

        protected virtual void CheckActions()
        {
            State nextState;

            MoveBasedOnTargetData.Action[] validActions = _data.ValidActions;

            for (int i = 0; i < validActions.Length; i++)
            {
                if (!_blockedActions.Contains(validActions[i]))
                {
                    if (TryRunAction(validActions[i], out nextState))
                    {
                        _stateMachine.ChangeCurrentState(nextState);
                        return;
                    }
                }
            }
        }

        protected virtual bool TryRunAction(MoveBasedOnTargetData.Action action, out State nextState)
        {
            switch (action)
            {
                case MoveBasedOnTargetData.Action.TargetDead:
                    nextState = TargetDeadAction();
                    break;
                case MoveBasedOnTargetData.Action.TargetCloseToDeath:
                    nextState = TargetCloseToDeathAction();
                    break;
                case MoveBasedOnTargetData.Action.CloseToDeath:
                    nextState = CloseToDeathAction();
                    break;
                default:
                    nextState = null;
                    break;
            }

            return nextState != null;
        }

        private State TargetDeadAction()
        {
            if (_isTargetDead)
            {
                return _data.GetTransitionState(_stateMachine, _blockedActions, MoveBasedOnTargetData.Action.TargetDead);
            }

            return null;
        }

        private State TargetCloseToDeathAction()
        {
            if (_targetAttributeController == null)
            {
                _blockedActions.Add(MoveBasedOnTargetData.Action.CloseToDeath);
                return null;
            }

            NonPersistentAttribute np = _targetAttributeController.GetNonPersistentAttribute(AttributeType.Health);

            if (np.MaxValue == 0)
            {
                return null;
            }

            if (np.CurrentValue / np.MaxValue < _data.CloseToDeathHealthThreshold)
            {
                return _data.GetTransitionState(_stateMachine, _blockedActions, MoveBasedOnTargetData.Action.TargetCloseToDeath);
            }

            return null;
        }

        private State CloseToDeathAction()
        {
            if (_stateMachine.AttributeController == null)
            {
                _blockedActions.Add(MoveBasedOnTargetData.Action.CloseToDeath);
                return null;
            }

            NonPersistentAttribute np = _stateMachine.AttributeController.GetNonPersistentAttribute(AttributeType.Health);

            if (np.MaxValue == 0)
            {
                return null;
            }

            if (np.CurrentValue / np.MaxValue < _data.CloseToDeathHealthThreshold)
            {
                return _data.GetTransitionState(_stateMachine, _blockedActions, MoveBasedOnTargetData.Action.CloseToDeath);
            }

            return null;
        }

    }
}