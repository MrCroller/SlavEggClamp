using UnityEngine;
using UnityEngine.Events;

namespace SEC.Character
{
    public class EggInput : MonoBehaviour, IEggEvent
    {
        public UnityEvent<bool> OnTake { get; private set; } = new();
        public UnityEvent<Vector2, Vector2> OnThrow { get; private set; } = new();

        public Rigidbody2D Rigidbody2D;
        public SpriteRenderer EggSprite;
        public PolygonCollider2D PolygonCollider;
        public SpriteRenderer MinimapIcon;

        public static float Velocity { get; private set; }
        private bool _velosityFlagSave = true;
        private RigidbodyType2D _saveRbType;

        private void FixedUpdate()
        {
            if (_velosityFlagSave)
            {
                Velocity = Rigidbody2D.velocity.magnitude;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _velosityFlagSave = false;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            _velosityFlagSave = true;
        }

        public void OnTakeHandler(bool value) => OnTake?.Invoke(value);

        public void OnThrowHandler(Vector2 position, Vector2 forse) => OnThrow?.Invoke(position, forse);
    }
}
