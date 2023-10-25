using System;
using TimersSystemUnity.Extension;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngineTimers;

namespace SEC.UI
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Image _faderImage;
        [SerializeField] private float _fadeTime;
        [SerializeField] private AnimationCurve _easing;

        private IStop _timerAnimation;

        private void Start()
        {
            _faderImage.gameObject.SetActive(true);
            _faderImage.SetAlpha(0f);
        }

        public void FadeIn()
        {
            _timerAnimation?.Stop();
            _timerAnimation = _faderImage.SetAplhaDynamic(() => _timerAnimation = null, _fadeTime, _easing, false);
        }

        /// <summary>
        /// Увеличивает НЕпрозрачность изображения меняя Альфа канал
        /// </summary>
        /// <param name="MethodEnd"></param>
        public void FadeIn(UnityAction MethodEnd)
        {
            _timerAnimation?.Stop();
            _timerAnimation = _faderImage.SetAplhaDynamic(() => { _timerAnimation = null; MethodEnd(); }, _fadeTime, _easing, false);
        }

        public void FadeIn(UnityAction MethodEnd, float fadeTime)
        {
            _timerAnimation?.Stop();
            _timerAnimation = _faderImage.SetAplhaDynamic(() => { _timerAnimation = null; MethodEnd(); }, fadeTime, _easing, false);
        }

        public void FadeOut()
        {
            _timerAnimation?.Stop();
            _timerAnimation = _faderImage.SetAplhaDynamicRevert(() => _timerAnimation = null, _fadeTime, _easing, false);
        }

        /// <summary>
        /// Увеличивает прозрачность изображения меняя Альфа канал
        /// </summary>
        /// <param name="MethodEnd"></param>
        public void FadeOut(UnityAction MethodEnd)
        {
            _timerAnimation?.Stop();
            _timerAnimation = _faderImage.SetAplhaDynamic(MethodEnd, _fadeTime, _easing, false);
        }

        public void FadeOut(UnityAction MethodEnd, float fadeTime)
        {
            _timerAnimation?.Stop();
            _timerAnimation = _faderImage.SetAplhaDynamic(MethodEnd, fadeTime, _easing, false);
        }
    }
}
