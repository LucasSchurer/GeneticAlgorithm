using UnityEngine;
using Game.Events;
using Game.Entities.Shared;

/// <summary>
/// Handles player input
/// </summary>
namespace Game.Entities.Player
{
    public class InputController : MonoBehaviour
    {
        [SerializeField]
        private Transform _cameraTransform;
        [SerializeField]
        private Transform _weaponBulletSocket;
        [SerializeField]
        private float _jumpForce;

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
                _eventController?.TriggerEvent(EntityEventType.OnPrimaryActionPerformed, new EntityEventContext() { Origin = _weaponBulletSocket.position, Direction = _cameraTransform.forward});
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void OnDrawGizmos()
        {
            if (_cameraTransform != null)
            {
                Gizmos.DrawRay(transform.position, _cameraTransform.forward * 2f);
            }
        }

        private void FixedUpdate()
        {
            _movementController.Rotate(_inputData.lookDirection);
            _movementController.Move(transform.rotation * _inputData.movementDirection);
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
    } 
}
