using UnityEngine;
using UnityEngine.Events;

namespace SEC.Character
{
    public class EggInput : MonoBehaviour
    {
        [HideInInspector] public static UnityEvent<bool> OnTake = new();
        [HideInInspector] public UnityEvent<Vector2, Vector2> OnThrow = new();

        public Rigidbody2D Rigidbody2D;
        public SpriteRenderer EggSprite;
        public PolygonCollider2D PolygonCollider;
        public SpriteRenderer MinimapIcon;

        private RigidbodyType2D _saveRbType;

        public void OnTakeHandler(bool value) => OnTake?.Invoke(value);

        public void OnThrowHandler(Vector2 position, Vector2 forse) => OnThrow?.Invoke(position, forse);
    }
}
