using System;
using Cinemachine;
using SEC.Enums;
using SEC.Map;
using TimersSystemUnity.Extension;
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

        private Transform _parentTransform;
        private Camera _camera;
        private float _animationTime;
        private AnimationCurve _easing;

        /// <summary>
        /// Величина смещение по X
        /// </summary>
        private float _xTranslate;
        private float _yPosition;
        private float _zPosition;

        private CinemachineBasicMultiChannelPerlin _cbmcp;

        private IStop _cameraShakeTimer;

        internal CameraController(CameraInput input)
        {
            _camera                = input.Camera;
            _parentTransform       = input.transform;
            _cbmcp                 = input.VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _cbmcp.m_AmplitudeGain = 0f;
            _animationTime         = input.AnimateTime;
            _easing                = input.Easing;

            _xTranslate    = _camera.orthographicSize * 4f * (1f - input.TranslateRange);
            _yPosition     = _camera.transform.position.y;
            _zPosition     = _camera.transform.position.z;

            OnAnimationEnd = new();
            OnBorderExit   = input.OnBorderExit;

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
            LockTranslate = true;

            StopShake();
            float saveXPosition = _camera.transform.position.x;

            TimersPool.GetInstance().StartTimer(() => { LockTranslate = false; OnAnimationEnd.Invoke(); }, TranslateUp, _animationTime);

            void TranslateUp(float progress)
            {
                _parentTransform.transform.position = new Vector3(
                    x: _easing.Evaluate(progress)
                        * _xTranslate
                        * (orientation == OrientationLR.Right ? 1 : -1)
                        + saveXPosition,
                    _yPosition,
                    _zPosition);
            }
        }

        public void Shake(float time, float strength)
        {
            _cameraShakeTimer?.Stop();

            _cbmcp.m_AmplitudeGain = strength;
            _cameraShakeTimer = TimersPool.GetInstance().StartTimer(StopShake, time);
        }

        private void StopShake()
        {
            _cbmcp.m_AmplitudeGain = 0f;
            _cameraShakeTimer = null;
        }

    }
}