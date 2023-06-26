using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    public class FindTarget : State
    {
        private FindTargetData _data;
        private HashSet<FindTargetData.Action> _blockedActions;
        private Dictionary<FindTargetData.Action, ActionCallback> _validActions;

        private LayerMask _targetLayerMask;
        private LayerMask _secondaryTargetLayerMask;

        public FindTarget(StateMachine stateMachine, FindTargetData data) : base(stateMachine, data)
        {
            _data = data;
            _targetLayerMask = _data.GetTargetLayerMask(_data.Target, stateMachine.Entity);
            _secondaryTargetLayerMask = _data.GetTargetLayerMask(_data.SecondaryTarget, stateMachine.Entity);
            _blockedActions = new HashSet<FindTargetData.Action>();
            _validActions = BuildActionCallbackDictionary(_data.ValidActions, GetCallback);
        }

        public override void StateStart()
        {
            CheckActions(_validActions, _blockedActions);
        }

        protected virtual ActionCallback GetCallback(FindTargetData.Action action)
        {
            switch (action)
            {
                case FindTargetData.Action.FindTarget:
                    return FindPrimaryTarget;
                case FindTargetData.Action.FindSecondaryTarget:
                    return FindSecondaryTarget;
            }

            return null;
        }

        private State FindPrimaryTarget()
        {
            Transform target;
            Transform targetRoot;

            GetTargetTransform(_targetLayerMask, out target, out targetRoot, _data.TryToGetFace);

            if (target != null && targetRoot != null)
            {
                _stateMachine.CurrentContext.Target = new StateContext.TargetPacket() { Target = target, TargetRoot = targetRoot };

                return _data.GetTransitionState(_stateMachine, _blockedActions, FindTargetData.Action.FindTarget);
            }

            return null;
        }

        private State FindSecondaryTarget()
        {
            Transform target;
            Transform targetRoot;

            GetTargetTransform(_secondaryTargetLayerMask, out target, out targetRoot, _data.TryToGetFace);

            if (target != null && targetRoot != null)
            {
                _stateMachine.CurrentContext.Target = new StateContext.TargetPacket() { Target = target, TargetRoot = targetRoot };

                return _data.GetTransitionState(_stateMachine, _blockedActions, FindTargetData.Action.FindSecondaryTarget);
            }

            return null;
        }

        private void GetTargetTransform(LayerMask targetLayerMask, out Transform target, out Transform targetRoot, bool getFace = false)
        {
            target = null;
            targetRoot = null;

            Collider[] hits = Physics.OverlapSphere(_stateMachine.transform.position, _data.DetectionRadius, targetLayerMask);

            if (hits.Length > 0)
            {
                switch (_data.Choice)
                {
                    case FindTargetData.ChoiceType.First:
                        targetRoot = FindFirst(hits);
                        break;
                    case FindTargetData.ChoiceType.Random:
                        targetRoot = FindRandom(hits);
                        break;
                    case FindTargetData.ChoiceType.Nearest:
                        targetRoot = FindNearest(hits);
                        break;
                    case FindTargetData.ChoiceType.Furthest:
                        targetRoot = FindFurthest(hits);
                        break;
                }

                if (targetRoot != null)
                {
                    target = targetRoot;

                    if (getFace)
                    {
                        Transform face = target.GetComponentInChildren<Entities.Enemy.BotLookTowards>()?.transform;

                        if (face != null)
                        {
                            target = face;
                        }
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

        private Transform FindNearest(Collider[] hits)
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
                return hits[minIndex].transform;
            }

            return null;
        }

        private Transform FindFurthest(Collider[] hits)
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
                return hits[maxIndex].transform;
            }

            return null;
        }

        private Transform FindRandom(Collider[] hits)
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
                return validTransforms[Random.Range(0, validTransforms.Count - 1)];
            }

            return null;
        }

        private Transform FindFirst(Collider[] hits)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (_stateMachine.transform == hits[i].transform)
                {
                    continue;
                }

                return hits[i].transform;
            }

            return null;
        }
    }
}