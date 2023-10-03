using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SEC.UI
{
    public class OptionManager : MonoBehaviour
    {
        public const string MIXER_MASTER = "MasterVolume";
        public const string MIXER_MUSIC = "MusicVolume";
        public const string MIXER_EFFECT = "EffectVolume";
        public const string MIXER_VOICE = "VoiceVolume";

        public const float MIN_SOUND_VELOCITY = -50f;
        public const float MAX_SOUND_VELOCITY = 0f;

        public Slider sliderMaster;
        public Slider sliderMusic;
        public Slider sliderEffect;
        public Slider sliderVoice;

        [field: SerializeField] public AudioMixer AudioMixer { get; private set; }

        private void Start()
        {
            AudioMixer.GetFloat(MIXER_MASTER, out float value);
            sliderMaster.value = Mathf.InverseLerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, value);
            sliderMaster.onValueChanged.AddListener(ChangeMasterVolume);

            AudioMixer.GetFloat(MIXER_MUSIC, out value);
            sliderMusic.value = Mathf.InverseLerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, value);
            sliderMusic.onValueChanged.AddListener(ChangeMusicVolume);

            AudioMixer.GetFloat(MIXER_EFFECT, out value);
            sliderEffect.value = Mathf.InverseLerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, value);
            sliderEffect.onValueChanged.AddListener(ChangeEffectVolume);

            AudioMixer.GetFloat(MIXER_VOICE, out value);
            sliderVoice.value = Mathf.InverseLerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, value);
            sliderVoice.onValueChanged.AddListener(ChangeVoiceVolume);
        }

        private void OnDestroy()
        {
            sliderMaster.onValueChanged.RemoveListener(ChangeMasterVolume);
            sliderMusic.onValueChanged.RemoveListener(ChangeMusicVolume);
            sliderVoice.onValueChanged.RemoveListener(ChangeVoiceVolume);
            sliderEffect.onValueChanged.RemoveListener(ChangeEffectVolume);
        }

        private void ChangeMasterVolume(float volume)
        {
            AudioMixer.SetFloat(MIXER_MASTER, Mathf.Lerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, volume));
        }

        private void ChangeMusicVolume(float volume)
        {
            AudioMixer.SetFloat(MIXER_MUSIC, Mathf.Lerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, volume));
        }

        private void ChangeEffectVolume(float volume)
        {
            AudioMixer.SetFloat(MIXER_EFFECT, Mathf.Lerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, volume));
        }

        private void ChangeVoiceVolume(float volume)
        {
            AudioMixer.SetFloat(MIXER_VOICE, Mathf.Lerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, volume));
        }
    }
}
