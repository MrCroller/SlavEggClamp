using UnityEngine;

namespace SEC.Helpers
{
    public static class AudioHelper
    {
        public static float Play(this AudioSource source, AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("Отсутствует аудиоклип по событию");
                return 0f;
            }

            source.clip = clip;
            source.Play();
            return clip.length;
        }

        public static float Play(this AudioSource source, AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0)
            {
                Debug.LogWarning("Отсутствует аудиоклип по событию");
                return 0f;
            }

            var selectClip = clips.GetRandomElement();
            source.clip = selectClip;
            source.Play();
            return selectClip.length;
        }
    }
}
