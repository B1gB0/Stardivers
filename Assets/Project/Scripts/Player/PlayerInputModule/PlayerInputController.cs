using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Player.PlayerInputModule
{
    public class PlayerInputController : MonoBehaviour
    {
        private PlayerInput _playerInput;

        public Vector2 MoveDirection { get; private set; }

        public event Action OnWeaponButtonPressed;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void Update()
        {
            _playerInput.Player.Move.performed += OnMove;
            _playerInput.Player.Move.canceled += OnMove;
            
            _playerInput.Player.ActivateWeapon.performed += OnActivateWeapon;
            _playerInput.Player.ActivateWeapon.canceled += OnActivateWeapon;
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            MoveDirection = context.action.ReadValue<Vector2>();
        }

        private void OnActivateWeapon(InputAction.CallbackContext context)
        {
            OnWeaponButtonPressed?.Invoke();
        }
    }
}
