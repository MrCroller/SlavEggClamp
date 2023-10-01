using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Windows;


namespace SEC.Character
{
    public class EggController : IDisposable
    {
        #region Fields

        private UnityEvent<Vector2, Vector2> _onThrow;

        private Rigidbody2D _rb;
        private SpriteRenderer _eggSprite;
        private PolygonCollider2D _polygonCollider;
        private SpriteRenderer _minimapIcon;

        private bool IsTake
        {
            set
            {
                _rb.bodyType = value ? RigidbodyType2D.Kinematic : _saveRbType;
                _rb.velocity = Vector2.zero;
                _polygonCollider.enabled = !value;
                _eggSprite.enabled = !value;
                _minimapIcon.enabled = !value;
            }
        }

        private RigidbodyType2D _saveRbType;

        #endregion


        #region ClassLifeCicle

        public EggController(EggInput input)
        {
            _rb = input.Rigidbody2D;
            _eggSprite = input.EggSprite;
            _polygonCollider = input.PolygonCollider;
            _minimapIcon = input.MinimapIcon;

            _saveRbType = _rb.bodyType;

            _onThrow = input.OnThrow;

            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            EggInput.OnTake.AddListener(Take);
            _onThrow.AddListener(Throw);
        }

        private void Unsubscribe()
        {
            EggInput.OnTake.RemoveListener(Take);
            _onThrow.RemoveListener(Throw);
        }

        #endregion


        #region Methods

        public void Take(bool value)
        {
            IsTake = value;
        }

        public void Throw(Vector2 position, Vector2 forse)
        {
            IsTake = false;
            _rb.transform.position = position;
            _rb.AddForce(forse, ForceMode2D.Impulse);
        }

        #endregion

    }
}
