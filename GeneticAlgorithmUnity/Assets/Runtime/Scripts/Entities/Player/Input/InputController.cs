using UnityEngine;
using Game.Events;
using Game.Entities.Shared;

/// <summary>
/// Handles player input
/// </summary>
namespace Game.Entities.Player
{
    public class InputController : MonoBehaviour, IEntityController
    {
        [SerializeField]
        private Transform _cameraTransform;
        [SerializeField]
        private Transform _weaponBulletSocket;
        [SerializeField]
        private float _jumpForce;

        private bool _canMove = true;

        public struct InputData
        {
            public Vector3 movementDirection;
            public Vector3 lookDirection;

            public bool isPressingPrimaryAction;
        }

        [SerializeField]
        private bool _usingGamepad = false;

        private EntityEventController _eventController;

        private MovementController _movementController;

        private PlayerInputActions _playerInput;
        private InputData _inputData;

        public Vector3 GetLookDirection()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                return (hit.point - _weaponBulletSocket.position).normalized;
            }
            else
            {
                return ray.direction;
            }
        }

        private void Awake()
        {
            _movementController = GetComponent<MovementController>();
            _eventController = GetComponent<EntityEventController>();
            _playerInput = new PlayerInputActions();
        }

        private void Update()
        {
            UpdateInputData();

            if (_inputData.isPressingPrimaryAction)
            {
                _eventController?.TriggerEvent(EntityEventType.OnPrimaryActionPerformed, new EntityEventContext() {Direction = GetLookDirection()});
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            _movementController.Rotate(_inputData.lookDirection);
            
            if (_canMove)
            {
                _movementController.Move(transform.rotation * _inputData.movementDirection);
            }
        }

        private void Jump()
        {
            _movementController.Jump(_jumpForce, Vector3.up);
        }

        private void UpdateInputData()
        {
            SetLookDirection();
            Vector2 axis = _playerInput.Gameplay.Movement.ReadValue<Vector2>();

            _inputData.movementDirection.x = axis.x;
            _inputData.movementDirection.z = axis.y;
            _inputData.isPressingPrimaryAction = _playerInput.Gameplay.PrimaryButton.IsPressed();
        }

        private void SetLookDirection()
        {
            if (_usingGamepad)
            {
                _inputData.lookDirection = _playerInput.Gameplay.RightStickRotation.ReadValue<Vector3>();
            }
            else
            {
                _inputData.lookDirection = _cameraTransform.forward;
            }
        }

        private void OnEnable()
        {
            EnableInputActions();
        }

        private void OnDisable()
        {
            DisposeInputActions();
        }

        private void EnableInputActions()
        {
            _playerInput.Gameplay.Enable();
        }

        private void DisposeInputActions()
        {
            _playerInput.Gameplay.PrimaryButton.Dispose();
            _playerInput.Gameplay.Disable();
        }

        public void SetCanMove(bool canMove)
        {
            _canMove = canMove;
        }

        public bool CanMove()
        {
            return _canMove;
        }
    } 
}
