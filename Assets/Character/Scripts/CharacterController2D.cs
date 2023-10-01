using SEC.Associations;
using SEC.Character.Input;
using SEC.Enum;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace SEC.Character.Controller
{
    /// <summary>
    /// by Brackeys
    /// </summary>
    public class CharacterController2D
    {
        const float k_GroundedRadius = .2f;           // Радиус круга перекрытия для определения заземления
        const float k_CeilingRadius  = .2f;           // Радиус круга перекрытия для определения того, может ли игрок встать
        const float k_EggRadius      = .2f;           // Радиус круга перекрытия для определения того, может ли игрок взять яйцо


        public bool IsEggTake 
        {
            get => _isEggTake;
            set
            {
                _input.gameObject.layer = value ? LayerAssociations.PlayerEgg : LayerAssociations.Player;
                _input.Egg.layer = value ? LayerAssociations.PlayerEgg : LayerAssociations.Egg;

                _isEggTake = value;
                _input.Egg.SetActive(value);
            }
        }

        private bool _isEggTake = false;
        private bool _isGrounded;                      // На земле ли игрок
        private OrientationLR _orientation   = OrientationLR.Right;          // Для определения того, в какую сторону в данный момент обращен игрок
        private bool _isWasCrouching  = false;
        private Vector3 m_Velocity   = Vector3.zero;

        private PlayerInput _input;

        public CharacterController2D(PlayerInput input)
        {
            _input = input;

            _input.OnLandEvent   ??= new UnityEvent();
            _input.OnCrouchEvent ??= new UnityEvent<bool>();
            _input.OnTakeEgg     ??= new UnityEvent();
            _input.OnThrowEgg    ??= new UnityEvent<Vector2, Vector2>();
        }

        public void Execute()
        {
            bool wasGrounded = _isGrounded;
            _isGrounded = false;

            // Игрок заземляется, если при передаче круга в позицию для проверки земли он попадает во что-либо, обозначенное как земля
            // Для этого можно использовать слои, но при этом Sample Assets не будет переписывать настройки проекта.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_input.GroundCheck.position, k_GroundedRadius, _input.WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != _input.Rigidbody2D.gameObject)
                {
                    _isGrounded = true;
                    if (!wasGrounded)
                        _input.OnLandEvent.Invoke();
                }
            }
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // Если персонаж приседает, проверьте, может ли он встать.
            if (!crouch)
            {
                // Если у персонажа есть потолок, не позволяющий ему встать, держите его приседающим
                if (Physics2D.OverlapCircle(_input.GroundCheck.position, k_CeilingRadius, _input.WhatIsGround))
                {
                    crouch = true;
                }
            }

            //управлять персонажем только при включенном заземлении или AirControl
            if (_isGrounded || _input.AirControl)
            {

                // Если приседать
                if (crouch)
                {
                    if (!_isWasCrouching)
                    {
                        _isWasCrouching = true;
                        _input.OnCrouchEvent.Invoke(true);
                    }

                    // Уменьшить скорость на множитель crouchSpeed
                    move *= _input.CrouchSpeed;

                    // Отключить один из коллайдеров при приседании
                    if (_input.CrouchDisableCollider != null)
                        _input.CrouchDisableCollider.enabled = false;
                }
                else
                {
                    // Включить коллайдер при отсутствии приседания
                    if (_input.CrouchDisableCollider != null)
                         _input.CrouchDisableCollider.enabled = true;

                    if (_isWasCrouching)
                    {
                        _isWasCrouching = false;
                        _input.OnCrouchEvent.Invoke(false);
                    }
                }

                // Перемещение персонажа путем нахождения целевой скорости
                Vector3 targetVelocity = new Vector2(move * 10f, _input.Rigidbody2D.velocity.y);
                // And then smoothing it out and applying it to the character
                _input.Rigidbody2D.velocity = Vector3.SmoothDamp(_input.Rigidbody2D.velocity, targetVelocity, ref m_Velocity, _input.MovementSmoothing);

                // Если входной сигнал перемещает игрока вправо, а игрок стоит лицом влево...
                if (move > 0 && _orientation == OrientationLR.Left)
                {
                    // ... перевернуть игрока
                    Flip();
                }
                // Иначе, если входной сигнал перемещает игрока влево, а игрок стоит лицом вправо...
                else if (move < 0 && _orientation == OrientationLR.Right)
                {
                    // ... перевернуть игрока
                    Flip();
                }
            }
            // Если игрок должен прыгнуть...
            if (_isGrounded && jump)
            {
                // Добавить вертикальную силу к игроку
                _isGrounded = false;
                _input.Rigidbody2D.AddForce(new Vector2(0f, _input.JumpForce));
            }
        }

        public void Hand()
        {
            if (IsEggTake)
            {
                EggThrow();
            }
            else
            {
                EggTake();
            }
        }


        public void EggTake()
        {
            if (Physics2D.OverlapCircle(_input.EggCheck.position, k_EggRadius, _input.WhatIsEgg))
            {
                IsEggTake = true;
                _input.OnTakeEgg.Invoke();
            }
        }

        public void EggThrow()
        {
            IsEggTake = false;
            _input.OnThrowEgg.Invoke(_input.EggCheck.position,
                                     new Vector2(
                                         _orientation == OrientationLR.Right ? _input.ForseThrowEgg : -_input.ForseThrowEgg,
                                         0f));
        }

        private void Flip()
        {
            // Переключить способ обозначения игрока как стоящего перед ним
            _orientation = (_orientation == OrientationLR.Right) ? OrientationLR.Left : OrientationLR.Right;

            // Умножить локальный масштаб x игрока на -1
            Vector3 theScale = _input.Rigidbody2D.transform.localScale;
            theScale.x *= -1;
            _input.Rigidbody2D.transform.localScale = theScale;
        }
    }
}