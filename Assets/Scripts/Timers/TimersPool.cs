using System.Collections.Generic;
using UnityEngine.Events;


namespace UnityEngineTimers
{
    public sealed class TimersPool
    {

        #region Fields

        private static TimersPool _instance;

        private List<Timer> _timers;
        private const int TIMERS_PRE_GENERATE = 6;

        #endregion


        #region Properties

        private Timer Timer
        {
            get
            {
                for (int i = _timers.Count - 1; i >= 0; i--)
                {
                    if (!_timers[i].IsRunning)
                    {
                        return _timers[i];
                    }
                }
                _timers.Add(new Timer());
                return Timer;
            }
        }

        #endregion


        #region ClassLifeCicle

        private TimersPool()
        {
            _timers = new List<Timer>(TIMERS_PRE_GENERATE);
            for (int i = 0; i < TIMERS_PRE_GENERATE; i++)
            {
                _timers.Add(new Timer());
            }
        }

        public static TimersPool GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TimersPool();
            }
            return _instance;
        }

        #endregion


        #region Methods

        public IStop StartTimer(UnityAction endMethod, float time) => Timer.Start(endMethod, time);
        public IStop StartTimer(UnityAction<float> timeTickMethod, float time) => Timer.Start(timeTickMethod, time);
        public IStop StartTimer(UnityAction endMethod, UnityAction<float> timeTickMethod, float time) => Timer.Start(endMethod, timeTickMethod, time);


        public void Reset()
        {
            _timers.Clear();
        }

        #endregion

    }
}