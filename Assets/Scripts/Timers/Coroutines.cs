using System;
using System.Collections;
using UnityEngine;


namespace UnityEngineTimers
{
    public sealed class Coroutines : MonoBehaviour
    {

        #region Fields

        public const string NAME_GAMEOBJECT = "[COROUTINE_MANAGER]";

        private static Coroutines _instance;
        public static Coroutines Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject(NAME_GAMEOBJECT);
                    _instance = go.AddComponent<Coroutines>();
                    DontDestroyOnLoad(go);
                }

                return _instance;
            }
        }

        #endregion


        #region Methods

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            if (enumerator == null)
            {
                throw new Exception($"{enumerator} (enumerator) null references");
            }
            return Instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            if (routine == null)
            {
                throw new Exception($"{routine} (routine) null references");
            }
            Instance.StopCoroutine(routine);
        }

        #endregion

    }
}