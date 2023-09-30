using UnityEngine;


namespace SEC.Character
{
    public class EggController : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private SpriteRenderer _spriteRenderer;
        private PolygonCollider2D _polygonCollider;

        private bool IsTake
        {
            set
            {
                _rb.bodyType = value ? RigidbodyType2D.Kinematic : _saveRbType;
                _polygonCollider.enabled = !value;
                _spriteRenderer.enabled = !value;
            }
        }

        private RigidbodyType2D _saveRbType;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _polygonCollider = GetComponent<PolygonCollider2D>();

            _saveRbType = _rb.bodyType;
        }

        public void OnTake()
        {
            IsTake = true;
        }

        public void OnThrow(Vector2 position, Vector2 forse)
        {
            IsTake = false;
            this.transform.position = position;
            _rb.AddForce(forse, ForceMode2D.Impulse);
        }
    }
}
