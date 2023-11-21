using UnityEngine;
using UnityEngine.Events;


namespace SEC.Map
{
    public class TriggerInput : MonoBehaviour
    {
        public UnityEvent<Collider2D> TrigerEnter;
        public UnityEvent<Collider2D> TrigerExit;


        public void OnTriggerEnter2D(Collider2D collision)
        {
            TrigerEnter.Invoke(collision);
        }

        public void OnTriggerExit2D(Collider2D collision)
        {
            TrigerExit.Invoke(collision);
        }
    }
}