using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.ProceduralAnimation
{
    public class SimpleMovementController : MonoBehaviour
    {
        [SerializeField]
        private float _movementSpeed = 5f;

        private Vector3 _movementInput;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _movementInput.x = Input.GetAxisRaw("Horizontal");
            _movementInput.z = Input.GetAxisRaw("Vertical");
        }

        private void FixedUpdate()
        {
            if (_movementInput != Vector3.zero)
            {
                _rb.AddForce(_movementInput * _movementSpeed, ForceMode.Force);
            }
        }
    } 
}
