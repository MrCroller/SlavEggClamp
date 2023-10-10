using System;
using SEC.Enums;
using SEC.Map;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineTimers;


namespace SEC.Controller
{
    public sealed class CameraController : IDisposable
    {
        public readonly UnityEvent<OrientationLR> OnBorderExit;
        public readonly UnityEvent OnAnimationEnd;

        public bool LockTranslate = false;

        private Camera _camera;
        private float _animationTime;
        private AnimationCurve _easing;

        /// <summary>
        /// Величина смещение по X
        /// </summary>
        private float _xTranslate;
        private float _yPosition;
        private float _zPosition;

        internal CameraController(CameraInput input)
        {
            _camera = input.Camera;
            _animationTime = input.AnimateTime;
            _easing = input.Easing;

            _xTranslate = _camera.orthographicSize * 4f * (1f - input.TranslateRange);
            _yPosition = _camera.transform.position.y;
            _zPosition = _camera.transform.position.z;

            OnAnimationEnd = new();
            OnBorderExit = input.OnBorderExit;

            OnBorderExit.AddListener(Translate);
        }

        public void Dispose()
        {
            OnBorderExit.RemoveListener(Translate);
        }

        /// <summary>
        /// Смещение камеры в сторону
        /// </summary>
        /// <param name="orientation"></param>
        public void Translate(OrientationLR orientation)
        {
            if (LockTranslate) return;

            float saveXPosition = _camera.transform.position.x;

            LockTranslate = true;
            TimersPool.GetInstance().StartTimer(() => { LockTranslate = false; OnAnimationEnd.Invoke(); }, TranslateUp, _animationTime);

            void TranslateUp(float progress)
            {
                _camera.transform.position = new Vector3(
                    x: _easing.Evaluate(progress)
                        * _xTranslate
                        * (orientation == OrientationLR.Right ? 1 : -1)
                        + saveXPosition,
                    _yPosition,
                    _zPosition);
            }
        }
    }
}