using Game.AI;
using Game.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ProceduralAnimation
{
    public class BotLookTowards : MonoBehaviour
    {
        [SerializeField]
        private string _targetTag;
        private Transform _target;
        [SerializeField]
        private StateMachine _stateMachine;
        [SerializeField]
        private AttributeController _attributeController;
        private NonPersistentAttribute _rotationSpeed;

        private Vector3 DirectionToTarget => (_target.position - transform.position).normalized;

        private void Awake()
        {
            _target = GameObject.FindGameObjectWithTag(_targetTag).transform;
        }

        private void Start()
        {
            if (_attributeController)
            {
                _rotationSpeed = _attributeController.GetNonPersistentAttribute(AttributeType.RotationSpeed);
            }
            else
            {
                _rotationSpeed = new NonPersistentAttribute();
            }
        }

        private void FixedUpdate()
        {
            if (_stateMachine.CanMove())
            {
                if (DirectionToTarget == Vector3.zero) return;

                Quaternion smoothRotation = Quaternion.LookRotation(DirectionToTarget);

                smoothRotation = Quaternion.Slerp(transform.rotation, smoothRotation, Time.fixedDeltaTime * _rotationSpeed.CurrentValue);

                transform.rotation = Quaternion.Euler(new Vector3(0, smoothRotation.eulerAngles.y));
            }
        }
    } 
}
