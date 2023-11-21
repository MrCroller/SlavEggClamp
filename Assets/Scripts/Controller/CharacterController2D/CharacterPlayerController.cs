using SEC.Associations;
using SEC.Character.Input;
using SEC.Helpers;
using SEC.SO;
using TimersSystemUnity.Extension;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements.Experimental;
using UnityEngineTimers;


namespace SEC.Character.Controller
{
    public partial class CharacterController2D
    {

        #region Fields

        public readonly PlayerInput Input;

        private IEggEvent _eggEvents;

        /// <summary>
        /// Иммунитет к уничтожению
        /// </summary>
        private bool _imunable = false;
        private TimersPool _timers;

        private Color _saveMinimapColor;

        #endregion


        #region ClassLifeCicle

        public CharacterController2D(PlayerInput input,
                                     MovementSetting movementSetting,
                                     IEggEvent eggEvents,
                                     CameraShakeSetting cameraShakeSetting)
        {
            Input = input;
            MovementSetting = input.MovementSetting != null ? input.MovementSetting : movementSetting;
            CameraShakeSetting = cameraShakeSetting;

            Input.OnLandEvent ??= new UnityEvent();
            Input.OnCrouchEvent ??= new UnityEvent<bool>();

            Input.OnDeath ??= new();
            Input.OnTakeEgg ??= new();
            Input.OnThrowEgg ??= new();
            Input.OnKick ??= new();
            Input.OnDeath ??= new();

            _timers = TimersPool.GetInstance();

            _saveMinimapColor = Input.MinimapIcon.color;
            _eggEvents = eggEvents;

            // Развернуть персонажа в сторону центра если его сторона справа
            if (Input.HomeSide == Enums.OrientationLR.Right)
            {
                Flip();
            }
        }

        #endregion


        #region Methods

        public void Spawn(Vector2 position)
        {
            AudioVoicePlay(Input.VoiceAudioData.Spawn);
            Input.Animator.SetTrigger(AnimatorAssociations.Alive);

            Input.gameObject.layer = LayerAssociations.Player;

            Input.transform.position = position;
            Input.IsControlable = true;
            AddImmunable(MovementSetting.ImmunityTime);
        }

        public void Death()
        {
            Input.OnDeath.Invoke(Input);
            Input.Animator.SetTrigger(AnimatorAssociations.Dead);
            AudioEffectPlay(Input.EffectAudioData.Death);
            AudioVoicePlay(Input.VoiceAudioData.Death);

            Input.gameObject.layer = LayerAssociations.Background;

            Input.IsControlable = false;
        }

        public float Win()
        {
            Input.IsControlable = false;
            return AudioVoicePlay(Input.VoiceAudioData.Win);
        }

        public void AddImmunable(float time)
        {
            _imunable = true;
            Input.MainSprite.color = MovementSetting.ImmunityColor;

            _timers.StartTimer(() =>
            {
                _imunable = false;
                Input.MainSprite.color = Color.white;
            }, (float progress) => Input.MainSprite.SetAlpha(MovementSetting.ImmunityEasing.Evaluate(progress)), time);
        }

        private float AudioVoicePlay(AudioClip[] audio) => Input.AudioSourceVoice.Play(audio);

        private float AudioEffectPlay(AudioClip audio) => Input.AudioSourceEffect.Play(audio);

        #endregion

    }
}
