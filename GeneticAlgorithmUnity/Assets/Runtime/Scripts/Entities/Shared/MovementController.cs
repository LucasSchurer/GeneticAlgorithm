using UnityEngine;

namespace Game.Entities.Shared
{
    public class MovementController : MonoBehaviour
    {
        private AttributeController _attributeController;
        private NonPersistentAttribute _movementSpeed;
        private NonPersistentAttribute _rotationSpeed;
        private Rigidbody _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _attributeController = GetComponent<AttributeController>();
        }

        private void Start()
        {
            if (_attributeController)
            {
                _movementSpeed = _attributeController.GetNonPersistentAttribute(AttributeType.MovementSpeed);
                _rotationSpeed = _attributeController.GetNonPersistentAttribute(AttributeType.RotationSpeed);
            } else
            {
                _movementSpeed = new NonPersistentAttribute();
                _rotationSpeed = new NonPersistentAttribute();
            }
        }

        public void Move(Vector3 direction)
        {
            if (direction != Vector3.zero)
            {
                _rb.AddForce(direction * _movementSpeed.CurrentValue * 10f, ForceMode.Force);
            }
        }

        public void Rotate(Vector3 direction)
        {
            if (direction == Vector3.zero) return;

            Quaternion smoothRotation = Quaternion.LookRotation(direction);

            smoothRotation = Quaternion.Slerp(transform.rotation, smoothRotation, Time.fixedDeltaTime * _rotationSpeed.CurrentValue);

            transform.rotation = Quaternion.Euler(new Vector3(0, smoothRotation.eulerAngles.y));
        }

        public void Jump(float force, Vector3 direction)
        {
            _rb.AddForce(direction * force, ForceMode.Impulse);
        }
    } 
}
