using Game.Entities;
using Game.Entities.Enemy;
using Game.Entities.Shared;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    public class MoveBasedOnTarget : State
    {
        private MoveBasedOnTargetData _data;
        private HashSet<MoveBasedOnTargetData.Action> _blockedActions;
        private Dictionary<MoveBasedOnTargetData.Action, ActionCallback> _validActions;

        private Transform _target;
        private Transform _targetRoot;
        private bool _isTargetDead = false;
        private Vector3 _targetPosition = Vector3.zero;
        private Vector3 _direction;
        private MovementController _movementController;
        private Vector3 _offset;

        private Coroutine _randomizeOffsetCoroutine;
        private Coroutine _updatePositionCoroutine;
        private Coroutine _checkActionsCoroutine;
        private Coroutine _findLookTargetCoroutine;

        private bool _hasReachedTarget = false;

        private Transform _faceTransform;
        private BotLookTowards _lookTowards;

        private AttributeController _targetAttributeController;

        public MoveBasedOnTarget(StateMachine stateMachine, MoveBasedOnTargetData data) : base(stateMachine, data)
        {
            _data = data;
            _blockedActions = new HashSet<MoveBasedOnTargetData.Action>();
            _movementController = stateMachine.GetComponent<MovementController>();
            _validActions = BuildActionCallbackDictionary(_data.ValidActions, GetCallback);
        }

        public override void StateStart()
        {
            if (_stateMachine.PastContext.Target != null)
            {
                _target = _stateMachine.PastContext.Target.Target;
                _targetRoot = _stateMachine.PastContext.Target.TargetRoot;
                RandomizeOffset();
                _targetAttributeController = _targetRoot.GetComponent<AttributeController>();

                _lookTowards = _stateMachine.GetComponentInChildren<BotLookTowards>();

                StartCoroutines();
            } else
            {
                _isTargetDead = true;
            }
        }

        private IEnumerator SetLookTarget()
        {
            if (_stateMachine.transform != null && _targetRoot != null && _lookTowards.Target == null)
            {
                switch (_data.Facing)
                {
                    case MoveBasedOnTargetData.FacingType.Target:
                        _lookTowards.SetTarget(_targetRoot);
                        break;
                    case MoveBasedOnTargetData.FacingType.Ally:
                        _lookTowards.SetTarget(FindLookTarget(_stateMachine.Entity.AllyLayer));
                        break;
                    case MoveBasedOnTargetData.FacingType.Enemy:
                        _lookTowards.SetTarget(FindLookTarget(_stateMachine.Entity.EnemyLayer));
                        break;
                }

                if (_lookTowards.Target != null)
                {
                    _faceTransform = _lookTowards.transform;
                }
            }

            if (_data.Facing == MoveBasedOnTargetData.FacingType.Target)
            {
                yield return null;
            } else
            {
                yield return new WaitForSeconds(_data.NewTargetDetectionTime);

                _findLookTargetCoroutine = _stateMachine.StartCoroutine(SetLookTarget());
            }
        }

        private Transform FindLookTarget(LayerMask layer)
        {
            Collider[] hits = Physics.OverlapSphere(_stateMachine.transform.position, 300f, layer);

            return null;
        }

        private void StartCoroutines()
        {
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

            if (_data.NewTargetDetectionTime > 0)
            {
                _checkActionsCoroutine = _stateMachine.StartCoroutine(SetLookTarget());
            }
        }

        public override void StateUpdate()
        {
            Fire();

            if (_data.NewPositionInterval <= 0)
            {
                UpdatePosition();
            }

            UpdateDirection();

            _isTargetDead = _target == null;

            if (_data.CheckActionsInterval <= 0)
            {
                CheckActions(_validActions, _blockedActions);
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

            if (_findLookTargetCoroutine != null)
            {
                _stateMachine.StopCoroutine(_findLookTargetCoroutine);
            }
        }

        public override Vector3 GetLookDirection()
        {
            if ( _faceTransform != null)
            {
                return _faceTransform.forward;
            } else
            {
                return Vector3.zero;
            }
        }

        private void Fire()
        {
            if (_stateMachine.EventController && _data.CanFire)
            {
                Vector3 lookDirection = GetLookDirection();

                if (lookDirection != Vector3.zero)
                {
                    EntityEventContext ctx = new EntityEventContext();
                    ctx.Movement = new EntityEventContext.MovementPacket() { LookDirection = lookDirection };

                    _stateMachine.EventController.TriggerEvent(EntityEventType.OnPrimaryActionPerformed, ctx);
                }
            }
        }

        private void UpdateDirection()
        {
            if (_targetPosition != Vector3.zero)
            {
                float distanceToTarget = Vector3.Distance(_targetPosition, _stateMachine.transform.position);

                if (distanceToTarget < _data.RadiusToStopMoving)
                {
                    return;
                }

                if (distanceToTarget > _data.RadiusToStartMoving)
                {
                    _direction = (_targetPosition - _stateMachine.transform.position).normalized;
                } else
                {
                    _direction = Vector3.zero;
                }
                
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

            if (!CheckActions(_validActions, _blockedActions))
            {
                _checkActionsCoroutine = _stateMachine.StartCoroutine(CheckActionsCoroutine());
            }
        }

        protected virtual ActionCallback GetCallback(MoveBasedOnTargetData.Action action)
        {
            switch (action)
            {
                case MoveBasedOnTargetData.Action.TargetDead:
                    return TargetDeadAction;
                case MoveBasedOnTargetData.Action.TargetCloseToDeath:
                    return TargetCloseToDeathAction;
                case MoveBasedOnTargetData.Action.CloseToDeath:
                    return CloseToDeathAction;
            }

            return null;
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
                _blockedActions.Add(MoveBasedOnTargetData.Action.TargetCloseToDeath);
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