using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using UnityEngineTimers;


namespace SEC.Helpers
{
    public sealed class DeadList<T> : IEnumerable, IDisposable
    {
        public IEnumerator GetEnumerator() => dictionary.Keys.GetEnumerator();

        private readonly Dictionary<T, IStop> dictionary;

        public DeadList()
        {
            dictionary = new Dictionary<T, IStop>();
        }

        public DeadList(int length)
        {
            dictionary = new Dictionary<T, IStop>(length);
        }

        public void Add(T item)
        {
            if (!dictionary.ContainsKey(item))
            {
                dictionary.Add(item, default);
            }
            else
            {
                dictionary[item].Stop();
            }
        }

        /// <summary>
        /// Adds an object to the list and deletes it after a specified time.
        /// If the object is already in the collection, the timer is restarted
        /// </summary>
        /// <param name="item"></param>
        /// <param name="EndMethod">Method that will be called after deletion from the list</param>
        /// <param name="time">Time after which the object will be deleted (in seconds)</param>
        public void Add(T item, UnityAction EndMethod, float time)
        {
            if (!dictionary.Keys.Contains(item))
            {
                var timer = TimersPool.GetInstance().StartTimer(() =>
                {
                    EndMethod();
                    dictionary.Remove(item);
                }, time);

                dictionary.Add(item, timer);
            }
            else
            {
                dictionary[item].Stop();

                dictionary[item] = TimersPool.GetInstance().StartTimer(() =>
                {
                    EndMethod();
                    dictionary.Remove(item);
                }, time);
            }
        }

        /// <summary>
        /// Adds an object to the list and deletes it after a specified time. 
        /// If the object is already in the collection, the timer is restarted.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="StartMethod">Method to be called before adding (some actions with the object)</param>
        /// <param name="EndMethod">Method that will be called after deletion from the list</param>
        /// <param name="time">Time after which the object will be deleted (in seconds)</param>
        public void Add(T item, UnityAction StartMethod, UnityAction EndMethod, float time)
        {
            StartMethod();

            if (!dictionary.Keys.Contains(item))
            {
                var timer = TimersPool.GetInstance().StartTimer(() =>
                {
                    EndMethod();
                    dictionary.Remove(item);
                }, time);

                dictionary.Add(item, timer);
            }
            else
            {
                dictionary[item].Stop();

                dictionary[item] = TimersPool.GetInstance().StartTimer(() =>
                {
                    EndMethod();
                    dictionary.Remove(item);
                }, time);
            }
        }

        public void Stop(T item)
        {
            if (dictionary.ContainsKey(item))
            {
                dictionary[item].Stop();
            }
        }

        /// <summary>
        /// Stops all timers in the list.
        /// If you want to stop and then clear the list, just use the Clear() method
        /// </summary>
        public void StopAll()
        {
            foreach (IStop timer in dictionary.Values)
            {
                timer.Stop();
            }
        }

        /// <summary>
        /// Stops the timer on an object and removes it from the list
        /// </summary>
        /// <param name="item"></param>
        public void Remove(T item)
        {
            dictionary[item]?.Stop();
            dictionary.Remove(item);
        }

        /// <summary>
        /// Stops all timers and clears the list
        /// </summary>
        public void Dispose()
        {
            StopAll();
            dictionary.Clear();
        }
    }
}