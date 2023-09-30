using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;


namespace SEC.Character.Players
{
    public class PlayerInputData : MonoBehaviour
    {
        [SerializeField] private float _runSpeed = 40f;

        private CharacterController2D _controller;
        private float _horizontalMove;
        private bool _jump;

        private void Start()
        {
            _controller = GetComponent<CharacterController2D>();
        }

        private void FixedUpdate()
        {
            _controller.Move(_horizontalMove * Time.fixedDeltaTime, false, _jump);
            _jump = false;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _horizontalMove = context.ReadValue<float>() * _runSpeed;
        }

        public void OnJump(InputAction.CallbackContext _)
        {
            _jump = true;
        }

        public void OnHand(InputAction.CallbackContext _)
        {
            if (_controller.IsEggTake)
            {
                _controller.EggThrow();
            }
            else
            {
                _controller.EggTake();
            }
        }
    }
}
