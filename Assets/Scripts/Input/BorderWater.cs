using SEC.Enums;
using UnityEngine;

namespace SEC.Map
{
    [System.Obsolete("Класс для симуляции выталкивания на коллайдере - тригере(v1.0.2 не используется")]
    public class BorderWater : MonoBehaviour
    {
        public float ForceValue;
        public OrientationLR ForceOrientation;

        private Vector2 _forceVector;

        private void Awake()
        {
            switch (ForceOrientation)
            {
                case OrientationLR.Left:
                    _forceVector = Vector2.left;
                    break;
                case OrientationLR.Right:
                    _forceVector = Vector2.right;
                    break;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            collision.attachedRigidbody.AddForce(_forceVector * ForceValue);
        }
    }
}
