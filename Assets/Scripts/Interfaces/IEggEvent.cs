using UnityEngine.Events;
using UnityEngine;

namespace SEC.Character
{
    public interface IEggEvent
    {
        UnityEvent<bool> OnTake { get; }
        UnityEvent<Vector2, Vector2> OnThrow { get; }
    }
}