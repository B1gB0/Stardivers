using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Player.PlayerInputModule
{
    public class PlayerInputController : MonoBehaviour
    {
        private PlayerInput _playerInput;

        public Vector2 MoveDirection { get; private set; }

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        private void Update()
        {
            _playerInput.Player.Move.performed += OnMove;
            _playerInput.Player.Move.canceled += OnMove;
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
    }
}
