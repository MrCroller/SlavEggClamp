using UnityEngine;

namespace SEC.Helpers
{
    public static class AudioHelper
    {
        public static void Play(this AudioSource source, AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("Отсутствует аудиоклип по событию");
                return;
            }

            source.clip = clip;
            source.Play();
        }

    }
}
