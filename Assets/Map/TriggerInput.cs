using UnityEngine;
using UnityEngine.Events;


namespace SEC.Map
{
    public class TriggerInput : MonoBehaviour
    {
        public UnityEvent TrigerEnter;


        public void OnTriggerEnter2D(Collider2D collision)
        {
            TrigerEnter.Invoke();
        }
    }
}