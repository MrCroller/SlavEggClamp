using SEC.Associations;
using SEC.Character.Controller;
using SEC.Enums;
using SEC.SO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace SEC.Character.Input
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerInput : MonoBehaviour
    {

        #region Properties

        [Tooltip("Если не заполненно, то берется стандартный параметр из GameManager")]
        public MovementSetting MovementSetting;
        
        [Tooltip("С какой стороны спавнится игрок")]
        public OrientationLR HomeSide;

        [Header("Input Setting")]
        public InputAction Move;
        public InputAction Jump;
        public InputAction Hand;

        #region Links
            
            [Header("Links")]
            [Tooltip("Маска, определяющая, что является землей для персонажа")] public LayerMask WhatIsGround;
            
            /// <summary>
            /// Обозначение позиции, в которой следует проверить, заземлен ли игрок
            /// </summary>
            [field: SerializeField] public Transform GroundCheck { get; private set; }
            
            /// <summary>
            /// Позиция, обозначающая место проверки потолков
            /// </summary>
            [field: SerializeField] public Transform CeilingCheck { get; private set; }
            
            /// <summary>
            /// Коллайдер, который отключается при приседании
            /// </summary>
            [field: SerializeField] public Collider2D CrouchDisableCollider { get; private set; }
            
            /// <summary>
            /// Позиция, обозначающая место проверки яйца
            /// </summary>
            [field: SerializeField] public Transform EggCheck { get; private set; }
            
            /// <summary>
            /// Позиция из которой вылетает яйцо
            /// </summary>
            [field: SerializeField] public Transform EggThrowPoint { get; private set; }
            [field: SerializeField] public SpriteRenderer MainSprite { get; private set; }
            [field: SerializeField] public SpriteRenderer MinimapIcon { get; private set; }
            [field: SerializeField] public Animator Animator { get; private set; }
            [field: SerializeField] public AudioSource AudioSourceEffect { get; private set; }
            [field: SerializeField] public PlayerEffectAudioData EffectAudioData { get; private set; }
            [field: SerializeField] public AudioSource AudioSourceVoice { get; private set; }
            [field: SerializeField] public PlayerVoiceAudioData VoiceAudioData { get; private set; }
            
            [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
            
        #endregion

        [Header("Events")]
        [Space]

        public UnityEvent OnLandEvent;
        public UnityEvent<bool> OnCrouchEvent;
        public UnityEvent<bool> OnTakeEgg;
        public UnityEvent<Vector2, Vector2> OnThrowEgg;
        public UnityEvent OnKick;
        public UnityEvent<PlayerInput> OnDeath;

        [HideInInspector] public bool IsControlable = true;
        [HideInInspector] public IMovable Controller;
        private float _horizontalMove;
        private bool _jump;
        private float _linearDragSave;

        #endregion


        #region MONO

        private void Awake()
        {
            Rigidbody2D = Rigidbody2D != null ? Rigidbody2D : GetComponent<Rigidbody2D>();
            _linearDragSave = Rigidbody2D.drag;

            Jump.started += OnJump;
            Hand.started += OnHand;
        }

        private void OnEnable()
        {
            IsControlable = true;
            Rigidbody2D.drag = _linearDragSave;

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
            _horizontalMove = Move.ReadValue<float>();
        }

        private void FixedUpdate()
        {
            if (!IsControlable)
            {
                Controller.Move(0f, false, false);
                return;
            }

            Controller.Move(_horizontalMove * Time.fixedDeltaTime, false, _jump);
            _jump = false;
        }

        private void OnDestroy()
        {
            Jump.started -= OnJump;
            Hand.started -= OnHand;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerAssociations.Egg)
            {
                Controller.Bump(EggInput.Velocity);
            }
            else if (collision.gameObject.layer == LayerAssociations.DeadZone)
            {
                OnDeath.Invoke(this);
            }
        }

        #endregion


        #region EventHandlers

        public void OnJump(InputAction.CallbackContext _)
        {
            if (!IsControlable) return;
            _jump = true;
        }

        public void OnHand(InputAction.CallbackContext _)
        {
            if (!IsControlable) return;
            Controller.Hand();
        }

        public void OnKicked() => Controller.Kicked();

        #endregion

    }
}
