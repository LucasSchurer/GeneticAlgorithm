using Game.Managers;
using System.Collections;
using UnityEngine;

namespace Game.Entities.AI
{
    public class EnemyIdleState : EnemyState
    {
        private MovementController _movementController;
        private Coroutine _idleCoroutine;
        private Vector3 _direction = new Vector3(1, 0, 1);
        private Vector2 _turnTimeRange = new Vector2(1, 4);
        private float _detectionRange = 10f;

        public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void StateStart()
        {
            _idleCoroutine = _stateMachine.StartCoroutine(IdleCoroutine());
            _movementController = _stateMachine.GetComponent<MovementController>();
        }

        public override void StateUpdate()
        {
            Collider[] colliders = Physics.OverlapSphere(_stateMachine.transform.position, _detectionRange, GameManager.Instance.playerLayerMask);

            if (colliders.Length > 0)
            {
                _stateMachine.ChangeCurrentState(_stateMachine.BehaviourType == BehaviourType.Aggressive ? EnemyStateType.Chase : EnemyStateType.Run);
            }
        }

        public override void StateFixedUpdate()
        {
            if (_movementController)
            {
                try
                {
                    _movementController.Rotate(_direction);
                    _movementController.Move(_stateMachine.transform.forward);
                } catch (System.NullReferenceException e)
                {
                    Debug.Log(e.Message);
                }
            }
        }

        public override void StateFinish()
        {
            if (_idleCoroutine != null)
            {
                _stateMachine.StopCoroutine(_idleCoroutine);
            }
        }

        private IEnumerator IdleCoroutine()
        {
            Vector3 direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

            _direction = direction;

            yield return new WaitForSeconds(Random.Range(_turnTimeRange.x, _turnTimeRange.y));

            _idleCoroutine = _stateMachine.StartCoroutine(IdleCoroutine());
        }
    }
}
