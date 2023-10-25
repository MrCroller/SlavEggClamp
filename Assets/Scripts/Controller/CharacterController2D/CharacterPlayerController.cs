using SEC.Associations;
using SEC.Character.Input;
using SEC.Helpers;
using SEC.SO;
using TimersSystemUnity.Extension;
using UnityEngine;
using UnityEngine.Events;
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
        }

        #endregion


        #region Methods

        public void Spawn(Vector2 position)
        {
            VoicePlay(Input.VoiceAudioData.Spawn);
            Input.Animator.SetTrigger(AnimatorAssociations.Alive);

            Input.gameObject.layer = LayerAssociations.Player;
            Input.MainSprite.SetAlpha(0f);

            Input.transform.position = position;
            Input.IsControlable = true;
            AddImmunable(MovementSetting.ImmunityTime);
        }

        public void Death()
        {
            Input.OnDeath.Invoke(Input);
            Input.Animator.SetTrigger(AnimatorAssociations.Dead);
            AudioEffectPlay(Input.EffectAudioData.Death);

            Input.gameObject.layer = LayerAssociations.Background;

            Input.IsControlable = false;
        }

        public float Win()
        {
            Input.IsControlable = false;
            VoicePlay(Input.VoiceAudioData.Win);
            return Input.VoiceAudioData.Win.length;
        }

        public void AddImmunable(float time)
        {
            _imunable = true;
            _timers.StartTimer(() => _imunable = false, time);
        }

        private void VoicePlay(AudioClip audio)
        {
            Input.AudioSourceVoice.Play(audio);
        }

        private void AudioEffectPlay(AudioClip audio)
        {
            Input.AudioSourceEffect.Play(audio);
        }

        #endregion

    }
}
