using UnityEngine;
using Game.Events;
using Game.Entities.Shared;
using UnityEngine.InputSystem;
using System;
using System.Collections;

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
        private Crosshair _crosshair;
        [SerializeField]
        private Transform _weaponBulletSocket;
        [SerializeField]
        private float _jumpForce;

        private bool _canMove = true;

        [System.Serializable]
        public struct InputData
        {
            public Vector3 movementDirection;
            public Vector3 lookDirection;

            public bool isPressingPrimaryAction;
            public bool isPressingSecondaryAction;
            public bool isPressingSprint;
        }

        [SerializeField]
        private bool _usingGamepad = false;

        private EntityEventController _eventController;

        private MovementController _movementController;

        private PlayerInputActions _playerInput;
        [SerializeField]
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

            if ((_inputData.isPressingPrimaryAction || _inputData.isPressingSecondaryAction) && _eventController)
            {
                EntityEventContext context = new EntityEventContext();
                context.Movement = new EntityEventContext.MovementPacket() {
                    IsMoving = _inputData.movementDirection != Vector3.zero,
                    MovingDirection = _inputData.movementDirection,
                    LookDirection = GetLookDirection()
                };

                if (_inputData.isPressingPrimaryAction)
                {
                    _eventController.TriggerEvent(EntityEventType.OnPrimaryActionPerformed, context);
                }

                if (_inputData.isPressingSecondaryAction)
                {
                    _eventController.TriggerEvent(EntityEventType.OnSecondaryActionPerformed, context);
                }
            }            

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }

        private void FixedUpdate()
        {
            _movementController.Rotate(_inputData.lookDirection);
            
            if (_canMove && _inputData.movementDirection != Vector3.zero)
            {
                _movementController.Move(transform.rotation * _inputData.movementDirection);
                _crosshair.SetState(Crosshair.CrosshairState.Running);
            } else
            {
                _crosshair.SetState(Crosshair.CrosshairState.Standing);
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
            _inputData.isPressingSecondaryAction = _playerInput.Gameplay.SecondaryButton.IsPressed();

            bool isPressingSprint = _playerInput.Gameplay.Sprint.IsPressed();

            if (isPressingSprint && !_inputData.isPressingSprint)
            {
                _eventController.TriggerEvent(EntityEventType.OnSprintButtonStarted, new EntityEventContext() { });
            } else if (!isPressingSprint && _inputData.isPressingSprint)
            {
                _eventController.TriggerEvent(EntityEventType.OnSprintButtonEnded, new EntityEventContext() { });
            }

            _inputData.isPressingSprint = isPressingSprint;
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

            _playerInput.Gameplay.Swap1.performed += OnSwap1Performed;
            _playerInput.Gameplay.Swap2.performed += OnSwap2Performed;
            _playerInput.Gameplay.Swap3.performed += OnSwap3Performed;
        }

        private void OnSwap1Performed(InputAction.CallbackContext obj)
        {
            _eventController.TriggerEvent(EntityEventType.OnSwap1Performed, new EntityEventContext());
        }

        private void OnSwap2Performed(InputAction.CallbackContext obj)
        {
            _eventController.TriggerEvent(EntityEventType.OnSwap2Performed, new EntityEventContext());
        }

        private void OnSwap3Performed(InputAction.CallbackContext obj)
        {
            _eventController.TriggerEvent(EntityEventType.OnSwap3Performed, new EntityEventContext());
        }

        private void DisposeInputActions()
        {
            _playerInput.Gameplay.PrimaryButton.Dispose();
            _playerInput.Gameplay.SecondaryButton.Dispose();
            _playerInput.Gameplay.Sprint.Dispose();
            _playerInput.Gameplay.Swap1.Dispose();
            _playerInput.Gameplay.Swap2.Dispose();
            _playerInput.Gameplay.Swap3.Dispose();
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
