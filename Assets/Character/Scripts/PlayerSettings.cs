using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


namespace SEC.Character.Players
{
    internal class PlayerSettings : MonoBehaviour
    {
        [SerializeField] private float m_JumpForce = 400f;                          // Величина силы, добавляемой при прыжке игрока.
        [Range(0, 1)][SerializeField] private float m_CrouchSpeed = .36f;           // Величина maxSpeed, применяемая к движению приседания. 1 = 100%
        [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // Сколько нужно для сглаживания движения
        [SerializeField] private bool m_AirControl = true;                          // Может ли игрок управлять во время прыжка
        [SerializeField] private LayerMask m_WhatIsGround;                          // Маска, определяющая, что является землей для персонажа
        [SerializeField] private Transform m_GroundCheck;                           // Обозначение позиции, в которой следует проверить, заземлен ли игрок
        [SerializeField] private Transform m_CeilingCheck;                          // Позиция, обозначающая место проверки потолков
        [SerializeField] private LayerMask m_WhatIsEgg;                             // Маска, определяющая, что является яйцом для персонажа
        [SerializeField] private Transform m_EggCheck;                              // Позиция, обозначающая место проверки яйца
        [SerializeField] private Collider2D m_CrouchDisableCollider;                // Коллайдер, который отключается при приседании
        [SerializeField] private float _runSpeed = 40f;

        public void OnMove(InputAction.CallbackContext context)
        {
            //_horizontalMove = context.ReadValue<float>() * _runSpeed;
        }

        public void OnJump(InputAction.CallbackContext _)
        {
            //_jump = true;
        }

        public void OnHand(InputAction.CallbackContext _)
        {
            //if (_controller.IsEggTake)
            //{
            //    _controller.EggThrow();
            //}
            //else
            //{
            //    _controller.EggTake();
            //}
        }
    }
}
