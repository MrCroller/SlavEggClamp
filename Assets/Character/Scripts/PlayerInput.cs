using SEC.Character.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace SEC.Character.Input
{
    public class PlayerInput : MonoBehaviour
    {
        #region Properties

        [Header("Movement Setting")]
        [SerializeField] public float RunSpeed = 40f;

        /// <summary>
        /// Величина силы, добавляемой при прыжке игрока
        /// </summary>
        [field: SerializeField] public float JumpForce = 400f;

        /// <summary>
        /// Величина maxSpeed, применяемая к движению приседания. 1 = 100%
        /// </summary>
        [Tooltip("Range = 0 <> 1")][field: SerializeField] public float CrouchSpeed { get; private set; } = .36f;

        /// <summary>
        /// Сколько нужно для сглаживания движения
        /// </summary>
        [Tooltip("Range = 0 <> 0.3")][field: SerializeField] public float MovementSmoothing { get; private set; } = .05f;
        [field: SerializeField] public float ForseThrowEgg { get; private set; }
        [Tooltip("Может ли игрок управлять во время прыжка")][field: SerializeField] public bool AirControl { get; private set; } = true;

        [Header("Input Setting")]
        public InputAction Move;
        public InputAction Jump;
        public InputAction Hand;

        [Header("Links")]
        [field: SerializeField] public LayerMask WhatIsGround;              // Маска, определяющая, что является землей для персонажа
        [field: SerializeField] public Transform GroundCheck { get; private set; }               // Обозначение позиции, в которой следует проверить, заземлен ли игрок
        [field: SerializeField] public Transform CeilingCheck { get; private set; }              // Позиция, обозначающая место проверки потолков
        [field: SerializeField] public Collider2D CrouchDisableCollider { get; private set; }    // Коллайдер, который отключается при приседании
        [field: SerializeField] public LayerMask WhatIsEgg { get; private set; }                 // Маска, определяющая, что является яйцом для персонажа
        [field: SerializeField] public GameObject Egg { get; private set; }
        [field: SerializeField] public Transform EggCheck { get; private set; }                  // Позиция, обозначающая место проверки яйца

        public Rigidbody2D Rigidbody2D { get; private set; }

        [Header("Events")]
        [Space]

        public UnityEvent OnLandEvent;
        public UnityEvent<bool> OnCrouchEvent;
        public UnityEvent OnTakeEgg;
        public UnityEvent<Vector2, Vector2> OnThrowEgg;

        private CharacterController2D _controller;
        private float _horizontalMove;
        private bool _jump;

        #endregion


        #region MONO

        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
            _controller = new CharacterController2D(this);

            Jump.started += OnJump;
            Hand.started += OnHand;
        }

        private void OnEnable()
        {
            Move.Enable();
            Jump.Enable();
            Hand.Enable();
        }

        private void OnDisable()
        {
            Move.Disable();
            Jump.Disable();
            Hand.Disable();
        }
        private void Update()
        {
            _horizontalMove = Move.ReadValue<float>() * RunSpeed;
        }

        private void FixedUpdate()
        {
            _controller.Execute();
            _controller.Move(_horizontalMove * Time.fixedDeltaTime, false, _jump);
            _jump = false;
        }

        private void OnDestroy()
        {
            Jump.started -= OnJump;
            Hand.started -= OnHand;
        }

        #endregion


        #region EventHandlers

        public void OnJump(InputAction.CallbackContext _)
        {
            _jump = true;
        }


        public void OnHand(InputAction.CallbackContext _)
        {
            _controller.Hand();
        }

        public void OnDeath()
        {
            Debug.Log("СМЭРТЬ");
        }

        #endregion

    }
}
