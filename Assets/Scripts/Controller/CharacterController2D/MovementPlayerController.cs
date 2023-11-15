using System.Linq;
using SEC.Associations;
using SEC.Character.Input;
using SEC.Controller;
using SEC.Enums;
using SEC.Helpers;
using SEC.SO;
using UnityEngine;


namespace SEC.Character.Controller
{
    /// <summary>
    /// by Brackeys
    /// </summary>
    public partial class CharacterController2D : IMovable, IExecuteLater
    {

        #region Properties

        const float k_GroundedRadius = .2f;           // Радиус круга перекрытия для определения заземления
        const float k_CeilingRadius = .2f;           // Радиус круга перекрытия для определения того, может ли игрок встать
        const float k_EggRadius = .4f;           // Радиус круга перекрытия для определения того, может ли игрок взять яйцо

        public bool IsEggTake
        {
            get => _isEggTake;
            set
            {
                Input.Animator.SetBool(AnimatorAssociations.isEggTake, value);
                Input.gameObject.layer = value ? LayerAssociations.PlayerTakeEgg : LayerAssociations.Player;
                Input.MinimapIcon.color = value ? Color.white : _saveMinimapColor;

                _isEggTake = value;
                _eggEvents.OnTake.Invoke(value);
            }
        }

        public bool IsGrounded
        {
            get => _isGrounded;
            set
            {
                _isGrounded = value;
                Input.Animator.SetBool(AnimatorAssociations.isGrounded, value);
            }
        }

        #endregion


        #region Fields

        public readonly MovementSetting MovementSetting;
        public readonly CameraShakeSetting CameraShakeSetting;

        private bool _isEggTake = false;
        private bool _isGrounded;                      // На земле ли игрок
        private OrientationLR _orientation = OrientationLR.Right;          // Для определения того, в какую сторону в данный момент обращен игрок
        private bool _isWasCrouching = false;
        private Vector3 m_Velocity = Vector3.zero;

        #endregion


        #region Methods

        public void ExecuteLater()
        {
            bool wasGrounded = IsGrounded;

            // Игрок заземляется, если при передаче круга в позицию для проверки земли он попадает во что-либо, обозначенное как земля
            // Для этого можно использовать слои, но при этом Sample Assets не будет переписывать настройки проекта.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Input.GroundCheck.position, k_GroundedRadius, Input.WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != Input.Rigidbody2D.gameObject)
                {
                    IsGrounded = true;
                    if (!wasGrounded)
                        Input.OnLandEvent.Invoke();
                }
            }
        }

        public void Move(float moven, bool crouch, bool jump)
        {
            var move = moven * MovementSetting.RunSpeed;
            // Если персонаж приседает, проверьте, может ли он встать.
            if (!crouch)
            {
                // Если у персонажа есть потолок, не позволяющий ему встать, держите его приседающим
                if (Physics2D.OverlapCircle(Input.GroundCheck.position, k_CeilingRadius, Input.WhatIsGround))
                {
                    crouch = true;
                }
            }

            //управлять персонажем только при включенном заземлении или AirControl
            if (IsGrounded || MovementSetting.AirControl)
            {

                // Если приседать
                if (crouch)
                {
                    if (!_isWasCrouching)
                    {
                        _isWasCrouching = true;
                        Input.OnCrouchEvent.Invoke(true);
                    }

                    // Уменьшить скорость на множитель crouchSpeed
                    move *= MovementSetting.CrouchSpeed;

                    // Отключить один из коллайдеров при приседании
                    if (Input.CrouchDisableCollider != null)
                        Input.CrouchDisableCollider.enabled = false;
                }
                else
                {
                    // Включить коллайдер при отсутствии приседания
                    if (Input.CrouchDisableCollider != null)
                        Input.CrouchDisableCollider.enabled = true;

                    if (_isWasCrouching)
                    {
                        _isWasCrouching = false;
                        Input.OnCrouchEvent.Invoke(false);
                    }
                }

                // Перемещение персонажа путем нахождения целевой скорости
                Vector3 targetVelocity = new Vector2(move * 10f, Input.Rigidbody2D.velocity.y);
                // А затем сгладить его и применить к игроку
                Input.Rigidbody2D.velocity = Vector3.SmoothDamp(Input.Rigidbody2D.velocity, targetVelocity, ref m_Velocity, MovementSetting.MovementSmoothing);

                Input.Animator.SetFloat(AnimatorAssociations.xVelocity, Mathf.Abs(Input.Rigidbody2D.velocity.x));
                Input.Animator.SetFloat(AnimatorAssociations.yVelocity, Input.Rigidbody2D.velocity.y);
                //AudioEffectPlay(_input.EffectAudioData.Move);

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
            if (IsGrounded && jump)
            {
                // Добавить вертикальную силу к игроку
                IsGrounded = false;

                Input.Animator.SetTrigger(AnimatorAssociations.Jump);
                AudioEffectPlay(Input.EffectAudioData.Jump);

                Input.Rigidbody2D.AddForce(Vector2.up * MovementSetting.JumpForce);
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

        /// <summary>
        /// Взять яйцо
        /// </summary>
        public void EggTake()
        {
            var checkTake = Physics2D.OverlapCircleAll(Input.EggCheck.position, k_EggRadius);
            if (checkTake == null) return;


            if (checkTake.Any(obj => obj.gameObject.layer == LayerAssociations.Egg))
            {
                IsEggTake = true;
                Input.OnTakeEgg.Invoke(true);
            }
            else
            {
                var obj = checkTake.FirstOrDefault(obj => obj.gameObject.layer is LayerAssociations.PlayerTakeEgg);
                if (obj != null)
                {
                    VoicePlay(Input.VoiceAudioData.Kick);
                    AudioEffectPlay(Input.EffectAudioData.Hand);
                    Input.Animator.SetTrigger(AnimatorAssociations.Kick);

                    obj.GetComponentInParent<PlayerInput>().OnKicked();
                }
            }
        }

        /// <summary>
        /// Кинуть яйцо
        /// </summary>
        public void EggThrow()
        {
            IsEggTake = false;

            AudioEffectPlay(Input.EffectAudioData.Throw);
            VoicePlay(Input.VoiceAudioData.Throw);

            Input.OnThrowEgg.Invoke(Input.EggThrowPoint.position,
                                     new Vector2(
                                         _orientation == OrientationLR.Right ? MovementSetting.ForseThrowEgg : -MovementSetting.ForseThrowEgg,
                                         0f));
        }

        /// <summary>
        /// Выбивание яйца
        /// </summary>
        public void Kicked()
        {
            IsEggTake = false;

            Input.Animator.SetTrigger(AnimatorAssociations.Bump);
            Effects.CameraShake(CameraShakeSetting.PlayerKick_Time, CameraShakeSetting.PlayerKick_Forse);

            AddImmunable(MovementSetting.ImmunityTime);
            Input.OnKick.Invoke();
            _eggEvents.OnTake.Invoke(false);

            Vector2 forsePush = new(_orientation == OrientationLR.Right ? MovementSetting.ForseKickedEgg : -MovementSetting.ForseKickedEgg, 0f);
            Input.OnThrowEgg.Invoke(Input.EggCheck.position, forsePush / 2);
            Input.Rigidbody2D.AddForce(forsePush * 4, ForceMode2D.Impulse);
        }

        /// <summary>
        /// Входящий удар
        /// </summary>
        public void Bump(float forse)
        {
            AudioEffectPlay(Input.EffectAudioData.Bump);

            if (forse > MovementSetting.ForseToDeath && !_imunable)
            {
                Effects.CameraShake(CameraShakeSetting.PlayerKill_Time, CameraShakeSetting.PlayerKill_Forse);
                Death();
            }
        }

        private void Flip()
        {
            // Переключить способ обозначения игрока как стоящего перед ним
            _orientation = (_orientation == OrientationLR.Right) ? OrientationLR.Left : OrientationLR.Right;

            // Умножить локальный масштаб x игрока на -1
            Vector3 theScale = Input.Rigidbody2D.transform.localScale;
            theScale.x *= -1;
            Input.Rigidbody2D.transform.localScale = theScale;
        }

        #endregion

    }
}