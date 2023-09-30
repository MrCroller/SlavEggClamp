using SEC.Architecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Rendering;

namespace SEC.Character.Players
{
    /// <summary>
    /// by Brackeys
    /// </summary>
    public class CharacterController2D : MonoBehaviour
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

        const float k_GroundedRadius = .2f;           // Радиус круга перекрытия для определения заземления
        private bool m_Grounded;                      // Заземлен ли игрок или нет.
        const float k_CeilingRadius  = .2f;           // Радиус круга перекрытия для определения того, может ли игрок встать
        const float k_EggRadius      = .2f;           // Радиус круга перекрытия для определения того, может ли игрок взять яйцо
        public bool IsEggTake { get; private set; } = false;
        private Rigidbody2D m_Rigidbody2D;            
        private bool m_FacingRight   = true;          // Для определения того, в какую сторону в данный момент обращен игрок
        private Vector3 m_Velocity   = Vector3.zero;

        [Header("Events")]
        [Space]

        public UnityEvent OnLandEvent;

        [System.Serializable]
        public class BoolEvent : UnityEvent<bool> { }

        public BoolEvent OnCrouchEvent;
        private bool m_wasCrouching = false;

        public UnityEvent OnTakeEgg;

        private void Awake()
        {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

            if (OnLandEvent == null)
                OnLandEvent = new UnityEvent();

            if (OnCrouchEvent == null)
                OnCrouchEvent = new BoolEvent();

            if (OnTakeEgg == null)
                OnTakeEgg = new UnityEvent();
        }

        private void FixedUpdate()
        {
            bool wasGrounded = m_Grounded;
            m_Grounded = false;

            // Игрок заземляется, если при передаче круга в позицию для проверки земли он попадает во что-либо, обозначенное как земля
            // Для этого можно использовать слои, но при этом Sample Assets не будет переписывать настройки проекта.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    if (!wasGrounded)
                        OnLandEvent.Invoke();
                }
            }
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // Если персонаж приседает, проверьте, может ли он встать.
            if (!crouch)
            {
                // Если у персонажа есть потолок, не позволяющий ему встать, держите его приседающим
                if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                {
                    crouch = true;
                }
            }

            //управлять персонажем только при включенном заземлении или AirControl
            if (m_Grounded || m_AirControl)
            {

                // Если приседать
                if (crouch)
                {
                    if (!m_wasCrouching)
                    {
                        m_wasCrouching = true;
                        OnCrouchEvent.Invoke(true);
                    }

                    // Уменьшить скорость на множитель crouchSpeed
                    move *= m_CrouchSpeed;

                    // Отключить один из коллайдеров при приседании
                    if (m_CrouchDisableCollider != null)
                        m_CrouchDisableCollider.enabled = false;
                }
                else
                {
                    // Включить коллайдер при отсутствии приседания
                    if (m_CrouchDisableCollider != null)
                        m_CrouchDisableCollider.enabled = true;

                    if (m_wasCrouching)
                    {
                        m_wasCrouching = false;
                        OnCrouchEvent.Invoke(false);
                    }
                }

                // Перемещение персонажа путем нахождения целевой скорости
                Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
                // And then smoothing it out and applying it to the character
                m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

                // Если входной сигнал перемещает игрока вправо, а игрок стоит лицом влево...
                if (move > 0 && !m_FacingRight)
                {
                    // ... перевернуть игрока
                    Flip();
                }
                // Иначе, если входной сигнал перемещает игрока влево, а игрок стоит лицом вправо...
                else if (move < 0 && m_FacingRight)
                {
                    // ... перевернуть игрока
                    Flip();
                }
            }
            // Если игрок должен прыгнуть...
            if (m_Grounded && jump)
            {
                // Добавить вертикальную силу к игроку
                m_Grounded = false;
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            }
        }

        public void EggTake()
        {
            if (Physics2D.OverlapCircle(m_EggCheck.position, k_EggRadius, m_WhatIsEgg))
            {
                Debug.Log("Взять яйцо");
                IsEggTake = true;
                OnTakeEgg.Invoke();
            }
        }

        public void EggThrow()
        {
            Debug.Log("Яйцо кинуто");
            IsEggTake = false;
        }

        private void Flip()
        {
            // Переключить способ обозначения игрока как стоящего перед ним
            m_FacingRight = !m_FacingRight;

            // Умножить локальный масштаб x игрока на -1
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}