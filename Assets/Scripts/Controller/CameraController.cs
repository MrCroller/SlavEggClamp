using System;
using SEC.Enum;
using SEC.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngineTimers;

namespace SEC.Controller
{
    public class CameraController : IDisposable
    {
        public UnityEvent<OrientationLR> OnBorderExitAnim;
        private UnityEvent<OrientationLR> _onBorderExit;

        private Camera _camera;
        private float _animationTime;
        private AnimationCurve _easing;

        /// <summary>
        /// Величина смещение по X
        /// </summary>
        private float _xTranslate;
        private float _yPosition;
        private float _zPosition;

        private bool _lookAnim = false;

        internal CameraController(CameraInput input)
        {
            _camera = input.Camera;
            _animationTime = input.AnimateTime;
            _easing = input.Easing;

            _xTranslate = _camera.orthographicSize * 4f * (1f -input.PlayerTranslateRange);
            _yPosition = _camera.transform.position.y;
            _zPosition = _camera.transform.position.z;

            OnBorderExitAnim = new();
            _onBorderExit = input.OnBorderExit;

            _onBorderExit.AddListener(Translate);
        }

        public void Dispose()
        {
            _onBorderExit.RemoveListener(Translate);
        }

        public void Translate(OrientationLR orientation)
        {
            if (_lookAnim) return;

            float saveXPosition = _camera.transform.position.x;

            _lookAnim = true;
            TimersPool.GetInstance().StartTimer(() => { _lookAnim = false; OnBorderExitAnim.Invoke(orientation); }, TranslateUp, _animationTime);

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