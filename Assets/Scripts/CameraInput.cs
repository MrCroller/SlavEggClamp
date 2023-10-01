using UnityEngine;

namespace SEC.Input
{
    [RequireComponent(typeof(Camera))]
    public class CameraInput : MonoBehaviour
    {
        public Camera Camera;
        [Header("Settings")]
        public AnimationCurve Easing;
        public float AnimateTime;
        [Range(0.1f, 0.5f)] [Tooltip("Дальность появления игрока от края в процентах")]
        public float PlayerTranslateRange;

        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }
    }
}
