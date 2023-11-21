using UnityEngine;


namespace SEC.SO
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "SEC/Audio/PlayerVoiceAudioData")]
    public class PlayerVoiceAudioData : ScriptableObject
    {
        public AudioClip[] Spawn;
        [Tooltip("Ударить")] public AudioClip[] Kick;
        [Tooltip("Кинуть")] public AudioClip[] Throw;
        public AudioClip[] Win;
        public AudioClip[] Death;
    }
}
