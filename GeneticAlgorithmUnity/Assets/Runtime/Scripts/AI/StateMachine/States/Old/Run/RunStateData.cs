using UnityEngine;

namespace Game.AI.States
{
    [CreateAssetMenu(fileName = "RunStateData", menuName = "AI/StateMachine/States/RunStateData")]
    public class RunStateData : StateData
    {
        [SerializeField]
        private string _targetTag;
        [SerializeField]
        private float _maxRunDistance = 10f;

        public string TargetTag => _targetTag;
        public float MaxRunDistance => _maxRunDistance;

        public override State GetState(StateMachine stateMachine)
        {
            return new RunState(stateMachine, this);
        }
    } 
}
