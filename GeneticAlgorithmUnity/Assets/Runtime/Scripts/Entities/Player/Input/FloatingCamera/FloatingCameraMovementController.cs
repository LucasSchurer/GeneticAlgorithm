using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dnd.Player
{
    public class FloatingCameraMovementController : MonoBehaviour
    {
        [SerializeField]
        private float _movementSpeed;
        [SerializeField]
        private float _rotationSpeed;

        public void Move(Vector3 direction, Space space = Space.Self)
        {
            if (direction == Vector3.zero)
                return;

            transform.Translate(direction * _movementSpeed * Time.deltaTime, space);
        }

        public void Rotate(Vector3 direction)
        {
            if (direction == Vector3.zero) 
                return;

            Quaternion smoothRotation = Quaternion.LookRotation(direction);

            smoothRotation = Quaternion.Slerp(transform.rotation, smoothRotation, Time.deltaTime * _rotationSpeed);

            transform.rotation = smoothRotation;
        }
    } 
}
