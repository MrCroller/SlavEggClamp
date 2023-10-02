using System.Linq;
using SEC.Associations;
using SEC.Character.Input;
using SEC.Enum;
using SEC.Helpers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineTimers;


namespace SEC.Character.Controller
{
    /// <summary>
    /// by Brackeys
    /// </summary>
    public class CharacterController2D
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
                _input.Animator.SetBool(AnimatorAssociations.isEggTake, value);
                _input.gameObject.layer = value ? LayerAssociations.PlayerTakeEgg : LayerAssociations.Player;
                _input.MinimapIcon.color = value ? Color.white : _saveMinimapColor;

                _isEggTake = value;
                EggInput.OnTake.Invoke(value);
            }
        }

        public bool IsGrounded
        {
            get => _isGrounded;
            set
            {
                _isGrounded = value;
                _input.Animator.SetBool(AnimatorAssociations.isGrounded, value);
            }
        }

        private bool _isEggTake = false;
        private bool _isGrounded;                      // На земле ли игрок
        private OrientationLR _orientation = OrientationLR.Right;          // Для определения того, в какую сторону в данный момент обращен игрок
        private bool _isWasCrouching = false;
        private Vector3 m_Velocity = Vector3.zero;

        /// <summary>
        /// Иммунитет к уничтожению
        /// </summary>
        private bool _imunable = false;

        private Color _saveMinimapColor;

        private PlayerInput _input;

        #endregion


        #region ClassLifeCicle

        public CharacterController2D(PlayerInput input)
        {
            _input = input;

            _input.OnLandEvent ??= new UnityEvent();
            _input.OnCrouchEvent ??= new UnityEvent<bool>();
            _input.OnTakeEgg ??= new UnityEvent<bool>();
            _input.OnThrowEgg ??= new UnityEvent<Vector2, Vector2>();

            _saveMinimapColor = _input.MinimapIcon.color;
        }

        public void Execute()
        {
            bool wasGrounded = IsGrounded;
            //IsGrounded = false;

            // Игрок заземляется, если при передаче круга в позицию для проверки земли он попадает во что-либо, обозначенное как земля
            // Для этого можно использовать слои, но при этом Sample Assets не будет переписывать настройки проекта.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_input.GroundCheck.position, k_GroundedRadius, _input.WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != _input.Rigidbody2D.gameObject)
                {
                    IsGrounded = true;
                    if (!wasGrounded)
                        _input.OnLandEvent.Invoke();
                }
            }
        }

        #endregion


        #region Methods

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
            if (IsGrounded || _input.AirControl)
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
                // А затем сгладить его и применить к игроку
                _input.Rigidbody2D.velocity = Vector3.SmoothDamp(_input.Rigidbody2D.velocity, targetVelocity, ref m_Velocity, _input.MovementSmoothing);

                _input.Animator.SetFloat(AnimatorAssociations.xVelocity, Mathf.Abs(_input.Rigidbody2D.velocity.x));
                _input.Animator.SetFloat(AnimatorAssociations.yVelocity, _input.Rigidbody2D.velocity.y);
                AudioEffectPlay(_input.EffectAudioData.Move);

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

                _input.Animator.SetTrigger(AnimatorAssociations.Jump);
                AudioEffectPlay(_input.EffectAudioData.Jump);

                _input.Rigidbody2D.AddForce(new Vector2(0f, _input.JumpForce));
            }
        }

        public void Hand()
        {
            AudioEffectPlay(_input.EffectAudioData.Hand);

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
            var checkTake = Physics2D.OverlapCircleAll(_input.EggCheck.position, k_EggRadius);
            if (checkTake == null) return;

            if (checkTake.Any(obj => obj.gameObject.layer == LayerAssociations.Egg))
            {
                IsEggTake = true;
                _input.OnTakeEgg.Invoke(true);
            }
            else
            {
                var obj = checkTake.FirstOrDefault(obj => obj.gameObject.layer is LayerAssociations.PlayerTakeEgg);
                if (obj != null)
                {
                    VoicePlay(_input.VoiceAudioData.Kick);
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

            AudioEffectPlay(_input.EffectAudioData.Throw);
            VoicePlay(_input.VoiceAudioData.Throw);

            _input.OnThrowEgg.Invoke(_input.EggCheck.position,
                                     new Vector2(
                                         _orientation == OrientationLR.Right ? _input.ForseThrowEgg : -_input.ForseThrowEgg,
                                         0f));
            AddImmunable(_input.ImmunityTime);
        }

        /// <summary>
        /// Выбивание яйца
        /// </summary>
        public void Kicked()
        {
            IsEggTake = false;

            AddImmunable(_input.ImmunityTime);
            _input.OnKick.Invoke();
            EggInput.OnTake.Invoke(false);

            Vector2 forsePush = new(_orientation == OrientationLR.Right ? _input.ForseKickedEgg : -_input.ForseKickedEgg, 0f);
            _input.OnThrowEgg.Invoke(_input.EggCheck.position, forsePush / 2);
            _input.Rigidbody2D.AddForce(forsePush * 4, ForceMode2D.Impulse);
        }

        /// <summary>
        /// Входящий удар
        /// </summary>
        public void Bump(float forse)
        {
            //AudioEffectPlay(_input.EffectAudioData.Bump);

            if (forse > _input.ForseToDeath && !_imunable)
            {
                AudioEffectPlay(_input.EffectAudioData.Death);
                _input.OnDeath.Invoke();
            }
        }

        public void AddImmunable(float time)
        {
            _imunable = true;
            TimersPool.GetInstance().StartTimer(() => _imunable = false, time);
        }

        public void VoicePlay(AudioClip audio)
        {
            _input.AudioSourceVoice.Play(audio);
        }

        public void AudioEffectPlay(AudioClip audio)
        {
            _input.AudioSourceEffect.Play(audio);
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

        #endregion

    }
}