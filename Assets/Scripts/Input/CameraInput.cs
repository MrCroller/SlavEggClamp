using SEC.Enum;
using UnityEngine;
using UnityEngine.Events;

namespace SEC.Input
{
    [RequireComponent(typeof(Camera))]
    public class CameraInput : MonoBehaviour
    {
        public Camera Camera;
        public BoxCollider2D leftCollider;
        public BoxCollider2D rightCollider;

        [Header("Settings")]
        public AnimationCurve Easing;
        public float AnimateTime;
        [Range(0.1f, 0.5f)] [Tooltip("Дальность появления игрока от края в процентах")]
        public float PlayerTranslateRange;

        [HideInInspector] public UnityEvent<OrientationLR> OnBorderExit = new();

        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }

        public void OnLeftEnterHandler() => OnBorderExit?.Invoke(OrientationLR.Left);
        public void OnRightEnterHandler() => OnBorderExit?.Invoke(OrientationLR.Right);
    }
}
