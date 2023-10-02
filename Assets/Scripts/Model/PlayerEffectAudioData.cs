using UnityEngine;


namespace SEC.SO
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Audio/PlayerEffectAudioData")]
    public class PlayerEffectAudioData : ScriptableObject
    {
        public AudioClip Move;
        public AudioClip Jump;
        [Tooltip("Ударить")] public AudioClip Hand;
        [Tooltip("Кинуть")] public AudioClip Throw;
        [Tooltip("Входящий удар")] public AudioClip Bump;
        public AudioClip Death;
        public AudioClip Win;
    }
}
