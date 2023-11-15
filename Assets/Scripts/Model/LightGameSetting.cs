using UnityEngine;

namespace SEC.SO
{
    [CreateAssetMenu(fileName = "LightSetting", menuName = "SEC/Effect/LightSetting")]
    public class LightGameSetting : ScriptableObject
    {

        [SerializeField, Tooltip("Время анимации света")] private float _timeAnimate;
        [SerializeField, Tooltip("Кривая анимации света")] private AnimationCurve _easingAnimate;
        [SerializeField, Tooltip("Множитель изменений света")] private float _easingMultiplayer;

        public float TimeAnimate => _timeAnimate;
        public AnimationCurve Easing => _easingAnimate;
        public float EasingMultiplayer => _easingMultiplayer;

    }
}