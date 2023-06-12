using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    public class FindAlly : State
    {
        private FindAllyData _data;
        private HashSet<FindAllyData.Action> _blockedActions;

        private Transform _target;

        public FindAlly(StateMachine stateMachine, FindAllyData data) : base(stateMachine, data)
        {
            _data = data;
            _blockedActions = new HashSet<FindAllyData.Action>();
        }

        public override void StateStart()
        {
            Collider[] hits = Physics.OverlapSphere(_stateMachine.transform.position, _data.DetectionRadius, _data.AllyLayerMask);

            if (hits.Length > 0)
            {
                switch (_data.Choice)
                {
                    case FindAllyData.ChoiceType.First:
                        FindFirst(hits);
                        break;
                    case FindAllyData.ChoiceType.Random:
                        FindRandom(hits);
                        break;
                    case FindAllyData.ChoiceType.Nearest:
                        FindNearest(hits);
                        break;
                    case FindAllyData.ChoiceType.Furthest:
                        FindFurthest(hits);
                        break;
                }

                if (_target != null)
                {
                    _stateMachine.CurrentContext.Target = new StateContext.TargetPacket() { Target = _target };

                    if (!_blockedActions.Contains(FindAllyData.Action.FoundAlly))
                    {
                        _stateMachine.ChangeCurrentState(_data.GetTransitionState(_stateMachine, _blockedActions, FindAllyData.Action.FoundAlly));
                    }
                }
            }
        }

        public override void StateUpdate()
        {
            
        }

        public override void StateFixedUpdate()
        {
            
        }

        public override void StateFinish()
        {
            
        }

        public override Vector3 GetLookDirection()
        {
            return Vector3.zero;
        }

        private void FindNearest(Collider[] hits)
        {
            float minDistance = Mathf.Infinity;
            int minIndex = -1;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform == _stateMachine.transform)
                {
                    continue;
                }

                float distance = Vector3.Distance(_stateMachine.transform.position, hits[i].transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    minIndex = i;
                }
            }

            if (minIndex != -1)
            {
                _target = hits[minIndex].transform;
            }
        }

        private void FindFurthest(Collider[] hits)
        {
            float maxDistance = Mathf.NegativeInfinity;
            int maxIndex = -1;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform == _stateMachine.transform)
                {
                    continue;
                }

                float distance = Vector3.Distance(_stateMachine.transform.position, hits[i].transform.position);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxIndex = i;
                }
            }

            if (maxIndex != -1)
            {
                _target = hits[maxIndex].transform;
            }
        }

        private void FindRandom(Collider[] hits)
        {
            List<Transform> validTransforms = new List<Transform>();

            for (int i = 0; i < hits.Length; i++)
            {
                if (_stateMachine.transform == hits[i].transform)
                {
                    continue;
                }

                validTransforms.Add(hits[i].transform);
            }

            if (validTransforms.Count > 0)
            {
                _target = validTransforms[Random.Range(0, validTransforms.Count - 1)];
            }
        }

        private void FindFirst(Collider[] hits)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (_stateMachine.transform == hits[i].transform)
                {
                    continue;
                }

                _target = hits[i].transform;
            }
        }
    }
}