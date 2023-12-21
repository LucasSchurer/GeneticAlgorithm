using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dnd.Player.Input
{
    public class FloatingCameraInputController : MonoBehaviour
    {
        public struct InputData
        {
            public Vector3 movementDirection;
            public Vector3 lookDirection;
        }

        private PlayerInputActions _playerInput;

        [SerializeField]
        private InputData _inputData;

        private FloatingCameraMovementController _movementController;

        private void Awake()
        {
            _playerInput = new PlayerInputActions();
            _movementController = GetComponent<FloatingCameraMovementController>();
        }

        private void Update()
        {
            UpdateInputData();

            _movementController.Rotate(_inputData.lookDirection);

            if (_inputData.movementDirection != Vector3.zero)
            {
                _movementController.Move(_inputData.movementDirection);
            }
        }

        private void UpdateInputData()
        {
            _inputData.lookDirection = Camera.main.transform.forward;
            _inputData.movementDirection = _playerInput.FloatingCamera.Movement.ReadValue<Vector3>();
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
            _playerInput.FloatingCamera.Enable();
        }

        private void DisposeInputActions()
        {
            _playerInput.FloatingCamera.Disable();
        }
    }
}
