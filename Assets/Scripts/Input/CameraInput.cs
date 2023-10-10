using SEC.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace SEC.Map
{
    [RequireComponent(typeof(Camera))]
    public class CameraInput : MonoBehaviour
    {
        public Camera Camera;
        public BoxCollider2D leftCollider;
        public BoxCollider2D rightCollider;

        [Header("Settings Border")]
        public AnimationCurve Easing;
        public float AnimateTime;
        public float TranslateRange { get; private set; } = 0.15f;

        [HideInInspector] public UnityEvent<OrientationLR> OnBorderExit = new();

        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }

        public void OnLeftEnterHandler() => OnBorderExit?.Invoke(OrientationLR.Left);
        public void OnRightEnterHandler() => OnBorderExit?.Invoke(OrientationLR.Right);
    }
}
