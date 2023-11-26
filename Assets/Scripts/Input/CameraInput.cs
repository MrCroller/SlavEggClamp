using Cinemachine;
using SEC.Associations;
using SEC.Character;
using SEC.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace SEC.Map
{
    public class CameraInput : MonoBehaviour
    {
        public Camera Camera;
        public CinemachineVirtualCamera VirtualCamera;
        public BoxCollider2D leftCollider;
        public BoxCollider2D rightCollider;

        [Header("Settings Border")]
        public AnimationCurve Easing;
        public float AnimateTime;
        [field: SerializeField] public float TranslateRange { get; private set; } = 0.15f;

        [HideInInspector] public UnityEvent<OrientationLR> OnBorderExit;

        private void Start()
        {
            OnBorderExit ??= new();
        }

        public void OnLeftEnterHandler(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerAssociations.PlayerTakeEgg)
            {
                OnBorderExit?.Invoke(OrientationLR.Left);
            }
        }

        public void OnRightEnterHandler(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerAssociations.PlayerTakeEgg)
            {
                OnBorderExit?.Invoke(OrientationLR.Right);
            }
        }
    }
}
