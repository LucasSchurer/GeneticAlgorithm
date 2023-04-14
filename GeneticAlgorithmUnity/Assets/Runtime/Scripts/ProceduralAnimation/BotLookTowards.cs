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

        private Vector3 DirectionToTarget => (_target.position - transform.position).normalized;

        private void Awake()
        {
            _target = GameObject.FindGameObjectWithTag(_targetTag).transform;
        }

        private void Update()
        {
            Quaternion newRotation = Quaternion.LookRotation(DirectionToTarget);
            newRotation.eulerAngles = new Vector3(0f, newRotation.eulerAngles.y, 0f);

            transform.rotation = newRotation;
        }
    } 
}
