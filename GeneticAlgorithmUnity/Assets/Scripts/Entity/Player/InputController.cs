using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Events;

/// <summary>
/// Handles player input
/// </summary>
namespace Game.Entities.Player
{
    public class InputController : MonoBehaviour
    {
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
                _eventController?.EventTrigger(EntityEventType.OnPrimaryActionPerformed, new EntityEventContext());
            }
        }

        private void FixedUpdate()
        {
            _movementController.Move(_inputData.movementDirection);
            _movementController.Rotate(_inputData.lookDirection);
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
                Ray ray = Camera.main.ScreenPointToRay(_playerInput.Gameplay.MousePosition.ReadValue<Vector2>());

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    _inputData.lookDirection = (hit.point - transform.position).normalized;
                }
                else
                {
                    _inputData.lookDirection = Vector3.zero;
                }
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
