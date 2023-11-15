using System;
using System.Collections.Generic;
using System.Linq;
using SEC.SO;
using UnityEditor;
using UnityEngine.Rendering.Universal;
using UnityEngineTimers;

namespace SEC.Controller
{
    public class LightController : IDisposable
    {
        private Dictionary<Light2D, float> _light2Ds;

        private readonly LightGameSetting _settings;
        private IStop _timer;

        public LightController(LightGameSetting settings, Light2D[] backLights)
        {
            _settings = settings;

            _light2Ds = new(backLights.Select(light => new KeyValuePair<Light2D, float>(light, light.intensity)));
        }

        public void PlayAnimate()
        {
            _timer = TimersPool.GetInstance().StartTimer(PlayAnimate, (float progress) =>
            {
                foreach (var light in _light2Ds)
                {
                    light.Key.intensity = light.Value + _settings.Easing.Evaluate(progress) * _settings.EasingMultiplayer;
                }
            }, _settings.TimeAnimate);
        }

        public void StopAnimate()
        {
            _timer.Stop();

            foreach (var light in _light2Ds)
            {
                light.Key.intensity += light.Value;
            }
        }

        public void Dispose()
        {
            StopAnimate();
            _light2Ds.Clear();
        }
    }
}
