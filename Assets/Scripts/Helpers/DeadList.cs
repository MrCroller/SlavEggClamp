using System;
using System.Collections.Generic;
using SEC.Character.Input;
using UnityEngine.Events;
using UnityEngineTimers;


namespace SEC.Helpers
{
    public class DeadList<T> : List<T>, IDisposable where T : PlayerInput
    {
        private Dictionary<T, IStop> dictionary;

        public DeadList() : base()
        {
            dictionary = new Dictionary<T, IStop>();
        }
        public DeadList(int length) : base(length)
        {
            dictionary = new Dictionary<T, IStop>();
        }

        public new void Add(T player, UnityAction EndMethod, float time)
        {
            player.IsControlable = false;

            if (!base.Contains(player))
            {
                base.Add(player);

                var timer = TimersPool.GetInstance().StartTimer(() =>
                {
                    EndMethod();
                    base.Remove(player);
                    dictionary.Remove(player);
                }, time);

                dictionary.Add(player, timer);
            }
            else
            {
                dictionary[player].Stop();

                dictionary[player] = TimersPool.GetInstance().StartTimer(() =>
                {
                    EndMethod();
                    base.Remove(player);
                    dictionary.Remove(player);
                }, time);
            }
        }

        public void StopAll()
        {
            foreach (IStop timer in dictionary.Values)
            {
                timer.Stop();
            }
        }

        public new void Remove(T player)
        {
            dictionary[player]?.Stop();
            dictionary.Remove(player);
            base.Remove(player);
        }

        public new void Clear()
        {
            StopAll();
            base.Clear();
            dictionary.Clear();
        }

        public void Dispose()
        {
            StopAll();
            base.Clear();
            dictionary.Clear();
        }
    }
}
