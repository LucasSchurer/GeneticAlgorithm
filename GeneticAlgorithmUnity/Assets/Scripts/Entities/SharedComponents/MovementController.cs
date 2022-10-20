using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities
{
    public class MovementController : MonoBehaviour
    {
        [SerializeField]
        private ScriptableObjects.Movement _settings;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        public void Move(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                _rb.AddForce(direction * _settings.movementSpeed * 10f, ForceMode.Force);
            }
        }

        public void Rotate(Vector3 direction)
        {
            if (direction == Vector3.zero) return;

            Quaternion smoothRotation = Quaternion.LookRotation(direction);

            smoothRotation = Quaternion.Slerp(transform.rotation, smoothRotation, Time.fixedDeltaTime * _settings.rotationSpeed);

            transform.rotation = Quaternion.Euler(new Vector3(0, smoothRotation.eulerAngles.y));
        }
    } 
}
