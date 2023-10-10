using SEC.Associations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
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

        public Slider SliderMaster;
        public Slider SliderMusic;
        public Slider SliderEffect;
        public Slider SliderVoice;

        public Button RestartButton;
        public Button ExitButton;

        [field: SerializeField] public AudioMixer AudioMixer { get; private set; }

        private void Start()
        {
            AudioMixer.GetFloat(MIXER_MASTER, out float value);
            SliderMaster.value = Mathf.InverseLerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, value);
            SliderMaster.onValueChanged.AddListener(ChangeMasterVolume);

            AudioMixer.GetFloat(MIXER_MUSIC, out value);
            SliderMusic.value = Mathf.InverseLerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, value);
            SliderMusic.onValueChanged.AddListener(ChangeMusicVolume);

            AudioMixer.GetFloat(MIXER_EFFECT, out value);
            SliderEffect.value = Mathf.InverseLerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, value);
            SliderEffect.onValueChanged.AddListener(ChangeEffectVolume);

            AudioMixer.GetFloat(MIXER_VOICE, out value);
            SliderVoice.value = Mathf.InverseLerp(MIN_SOUND_VELOCITY, MAX_SOUND_VELOCITY, value);
            SliderVoice.onValueChanged.AddListener(ChangeVoiceVolume);

            RestartButton.onClick.AddListener(RestartButtonHandler);
            ExitButton.onClick.AddListener(ExitButtonHandler);
        }

        private void OnDestroy()
        {
            SliderMaster.onValueChanged.RemoveListener(ChangeMasterVolume);
            SliderMusic.onValueChanged.RemoveListener(ChangeMusicVolume);
            SliderVoice.onValueChanged.RemoveListener(ChangeVoiceVolume);
            SliderEffect.onValueChanged.RemoveListener(ChangeEffectVolume);

            RestartButton.onClick.RemoveListener(RestartButtonHandler);
            ExitButton.onClick.RemoveListener(ExitButtonHandler);
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

        private void RestartButtonHandler()
        {
            SceneManager.LoadScene(SceneAssociations.Game);
        }

        private void ExitButtonHandler()
        {
            Application.Quit();
        }
    }
}
