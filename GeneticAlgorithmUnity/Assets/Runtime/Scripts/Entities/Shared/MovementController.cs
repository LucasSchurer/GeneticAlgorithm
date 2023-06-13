using UnityEngine;

namespace Game.Entities.Shared
{
    public class MovementController : MonoBehaviour
    {
        public enum Status
        {
            Standing,
            Walking,
            Running,
            Air,
        }

        [Header("Settings")]
        [SerializeField]
        private LayerMask _groundLayerMask;
        [SerializeField]
        private float _groundDrag;
        [SerializeField]
        private float _airDrag;
        [SerializeField]
        [Tooltip("If false, will set drag to the _groundDrag value")]
        private bool _alterDrag = true;
        [SerializeField]
        private float _movementSpeedMultiplier = 10f;
        [SerializeField]
        private float _airMovementMultiplier = 0.3f;
        [SerializeField]
        private float _entityHeight = 2f;
        [SerializeField]
        private float _groundSphereRadius = 0.4f;
        [SerializeField]
        private Vector3 _groundCheckOffset;
        [SerializeField]
        private float _fallMultiplier = 2.5f;

        private AttributeController _attributeController;
        private NonPersistentAttribute _movementSpeed;
        private NonPersistentAttribute _rotationSpeed;
        private Rigidbody _rb;
        [SerializeField]
        private bool _isGrounded;
        private RaycastHit _slopeHit;
        private Vector3 _slopeMovementDirection;

        private Status _currentStatus = Status.Standing;

        public Status CurrentStatus => _currentStatus;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _attributeController = GetComponent<AttributeController>();
            _rb.drag = _groundDrag;
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

        private void Update()
        {
            UpdateIsGrounded();
            ConstraintVelocity();
            ControlDrag();

            if (_rb.velocity.y < 0)
            {
                _rb.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplier * -1) * Time.deltaTime;
            }

            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (_isGrounded)
            {
                if (_rb.velocity.x <= 0.1f && _rb.velocity.z <= 0.1f)
                {
                    _currentStatus = Status.Standing;
                }
                else
                {
                    _currentStatus = Status.Running;
                }
            } else
            {
                _currentStatus = Status.Air;
            }
        }

        private void UpdateIsGrounded()
        {
            _isGrounded = Physics.CheckSphere(transform.position + _groundCheckOffset, _groundSphereRadius, _groundLayerMask);
        }

        private bool IsOnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _entityHeight * 0.5f + 0.5f, _groundLayerMask))
            {
                return _slopeHit.normal != Vector3.up;
            }

            return false;
        }

        private void ControlDrag()
        {
            if (_alterDrag)
            {
                if (_isGrounded)
                {
                    _rb.drag = _groundDrag;
                }
                else
                {
                    _rb.drag = _airDrag;
                }
            }
        }

        private void ConstraintVelocity()
        {
            Vector3 currentVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if (currentVelocity.magnitude > _movementSpeed.CurrentValue)
            {
                Vector3 constraintVelocity = currentVelocity.normalized * _movementSpeed.CurrentValue;
                constraintVelocity.y = _rb.velocity.y;
                _rb.velocity = constraintVelocity;
            }
        }

        public void Move(Vector3 direction)
        {
            if (direction != Vector3.zero && _rb != null && enabled)
            {
                bool isOnSlope = IsOnSlope();

                if (_isGrounded && !isOnSlope)
                {
                    _rb.AddForce(direction * _movementSpeed.CurrentValue * _movementSpeedMultiplier, ForceMode.Acceleration);
                }
                else if (_isGrounded && isOnSlope)
                {
                    _slopeMovementDirection = Vector3.ProjectOnPlane(direction, _slopeHit.normal);
                    _rb.AddForce(_slopeMovementDirection * _movementSpeed.CurrentValue * _movementSpeedMultiplier, ForceMode.Acceleration);
                    
                } else if (!_isGrounded)
                {
                    _rb.AddForce(direction * _movementSpeed.CurrentValue * _movementSpeedMultiplier * _airMovementMultiplier, ForceMode.Acceleration);
                }
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
            if (_isGrounded)
            {
                force = Mathf.Sqrt(force * -2f * Physics.gravity.y);
                _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                _rb.AddForce(direction * force, ForceMode.VelocityChange);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position + _groundCheckOffset, 0.5f);
        }
    } 
}
