using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Player.PlayerInputModule
{
    public class PlayerInputController : MonoBehaviour
    {
        private const float MinMagnitude = 0f;
        
        private PlayerInput _playerInput;
        private Joystick _joystick;

        public Vector2 MoveDirection { get; private set; }
        public bool IsMoveInputPerformed { get; private set; }

        public event Action OnWeaponButtonPressed;
        public event Action OnMoveButtonsPressed;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
            
            _playerInput.Player.Move.performed += OnMove;
            _playerInput.Player.Move.canceled += OnMove;

            _playerInput.Player.ActivateWeapon.performed += OnActivateWeapon;
            _playerInput.Player.ActivateWeapon.canceled += OnActivateWeapon;
        }

        private void OnDisable()
        {
            _playerInput.Player.Move.performed -= OnMove;
            _playerInput.Player.Move.canceled -= OnMove;

            _playerInput.Player.ActivateWeapon.performed -= OnActivateWeapon;
            _playerInput.Player.ActivateWeapon.canceled -= OnActivateWeapon;
            
            _playerInput.Disable();
        }

        private void OnDestroy()
        {
            _joystick.OnInputHandled -= OnMoveWithJoystick;
        }

        public void GetJoystick(Joystick joystick)
        {
            _joystick = joystick;
            _joystick.OnInputHandled += OnMoveWithJoystick;
        }

        private void OnMoveWithJoystick()
        {
            MoveDirection = _joystick.Direction;
            
            IsMoveInputPerformed = MoveDirection.sqrMagnitude > MinMagnitude;
            
            if(IsMoveInputPerformed)
                OnMoveButtonsPressed?.Invoke();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            MoveDirection = context.action.ReadValue<Vector2>();

            IsMoveInputPerformed = MoveDirection.sqrMagnitude > MinMagnitude;
            
            if(IsMoveInputPerformed)
                OnMoveButtonsPressed?.Invoke();
        }

        private void OnActivateWeapon(InputAction.CallbackContext context)
        {
            OnWeaponButtonPressed?.Invoke();
        }
    }
}
