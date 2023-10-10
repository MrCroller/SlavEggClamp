using UnityEngine;
using UnityEngine.Events;


namespace SEC.Map
{
    public class TriggerInput : MonoBehaviour
    {
        public UnityEvent<Collider2D> TrigerEnter;


        public void OnTriggerEnter2D(Collider2D collision)
        {
            TrigerEnter.Invoke(collision);
        }
    }
}