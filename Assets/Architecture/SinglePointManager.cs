using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SEC.Architecture
{
    public class SinglePointManager : MonoBehaviour
    {
        IMonoUpdate[] monoUpdates = null;

        private void Awake()
        {
            foreach(IMonoUpdate update in monoUpdates)
            {
                update.Initialize();
            }
        }


        void Start()
        {
            foreach (IMonoUpdate update in monoUpdates)
            {
                update.Start();
            }
        }


        void Update()
        {
            foreach (IMonoUpdate update in monoUpdates)
            {
                update.Update();
            }
        }

        private void FixedUpdate()
        {
            foreach (IMonoUpdate update in monoUpdates)
            {
                update.FixedUpdate();
            }
        }
    }
}