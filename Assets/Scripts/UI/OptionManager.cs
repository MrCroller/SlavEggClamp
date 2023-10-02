using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace SEC.UI
{
    public class OptionManager : MonoBehaviour
    {
        public const string MixerMaster = "MasterVolume";
        public const string MixerMusic = "MusicVolume";
        public const string MixerEffect = "EffectVolume";
        public const string MixerVoice = "VoiceVolume";

        public Slider sliderMaster;
        public Slider sliderMusic;
        public Slider sliderEffect;
        public Slider sliderVoice;

        [field: SerializeField] public AudioMixer AudioMixer { get; private set; }

        private void Start()
        {
            float value = 0f;

            AudioMixer.GetFloat(MixerMaster, out value);
            sliderMaster.value = value;
            sliderMaster.onValueChanged.AddListener(ChangeMasterVolume);

            AudioMixer.GetFloat(MixerMusic, out value);
            sliderMusic.value = value;
            sliderMusic.onValueChanged.AddListener(ChangeMusicVolume);

            AudioMixer.GetFloat(MixerEffect, out value);
            sliderEffect.value = value;
            sliderEffect.onValueChanged.AddListener(ChangeEffectVolume);

            AudioMixer.GetFloat(MixerVoice, out value);
            sliderVoice.value = value;
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
            AudioMixer.SetFloat(MixerMaster, volume);
        }

        private void ChangeMusicVolume(float volume)
        {
            AudioMixer.SetFloat(MixerMusic, volume);
        }

        private void ChangeEffectVolume(float volume)
        {
            AudioMixer.SetFloat(MixerEffect, volume);
        }

        private void ChangeVoiceVolume(float volume)
        {
            AudioMixer.SetFloat(MixerVoice, volume);
        }

    }
}
