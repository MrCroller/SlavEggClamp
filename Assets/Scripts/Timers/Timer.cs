using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


namespace UnityEngineTimers
{
    public sealed class Timer : IStop
    {

        #region Fields

        public UnityEvent OnEndTime;
        /// <summary>
        /// Return a floating point number from 0.0f to 1.0f
        /// </summary>
        public UnityEvent<float> OnProgressTick;

        /// <summary>
        /// Is the timer running?
        /// </summary>
        public bool IsRunning { get => _coroutine != null; }

        private Coroutine _coroutine = null;

        #endregion


        #region CodeLifeCycles

        public Timer()
        {
            OnEndTime = new UnityEvent();
            OnProgressTick = new UnityEvent<float>();
        }

        #endregion


        #region Methods

        /// <summary>
        /// Starts a timer
        /// </summary>
        /// <param name="endMethod">Method on the end of the timer to be run</param>
        /// <param name="time">Timer running time</param>
        public IStop Start(UnityAction endMethod, float time)
        {
            if (IsRunning)
            {
                throw new Exception("Timer is running");
            }

            OnEndTime.AddListener(endMethod);
            _coroutine = Coroutines.StartRoutine(Ticker(time));
            return this;
        }

        /// <summary>
        /// Starts a timer
        /// </summary>
        /// <param name="endMethod">Method on the end of the timer to be run</param>
        /// <param name="timeTickMethod">Method returning progress values from 0.0f to 1.0f</param>
        /// <param name="time">Timer running time</param>
        public IStop Start(UnityAction endMethod, UnityAction<float> timeTickMethod, float time)
        {
            if (IsRunning)
            {
                throw new Exception("Timer is running");
            }

            OnEndTime.AddListener(endMethod);
            OnProgressTick.AddListener(timeTickMethod);
            _coroutine = Coroutines.StartRoutine(Ticker(time));
            return this;
        }

        /// <summary>
        /// Starts a timer
        /// </summary>
        /// <param name="timeTickMethod">Method returning progress values from 0.0f to 1.0f</param>
        /// <param name="time">Timer running time</param>
        public IStop Start(UnityAction<float> timeTickMethod, float time)
        {
            if (IsRunning)
            {
                throw new Exception("Timer is running");
            }

            OnProgressTick.AddListener(timeTickMethod);
            _coroutine = Coroutines.StartRoutine(Ticker(time));
            return this;
        }

        public void Stop()
        {
            Coroutines.StopRoutine(_coroutine);
        }

        private IEnumerator Ticker(float maxTime)
        {
            float time = maxTime;
            do
            {
                time -= Time.deltaTime;
                OnProgressTick?.Invoke(Mathf.Lerp(0.0f, 1.0f, (maxTime - time) / maxTime));
                yield return null;
            } while (time > 0.0f);

            OnEndTime?.Invoke();

            OnProgressTick?.RemoveAllListeners();
            OnEndTime?.RemoveAllListeners();
            _coroutine = null;


        }

        #endregion

    }
}