using UnityEngine;

namespace SEC.SO
{
    [CreateAssetMenu(fileName = "DefaultMovementSetting", menuName = "SEC/Player/MovementSetting")]
    public class MovementSetting : ScriptableObject
    {
        [Header("Movement Setting")]
        public float RunSpeed = 60f;

        [Tooltip("Величина силы, добавляемой при прыжке игрока")]
        public float JumpForce = 900f;

        [Tooltip("Может ли игрок управлять во время прыжка")]
        public bool AirControl = true;

        [Tooltip("Величина maxSpeed, применяемая к движению приседания. 1 = 100%")]
        [Range(0f,1f)] public float CrouchSpeed = .36f;

        [Tooltip("Величина сглаживания движения")]
        [Range(0f, .3f)] public float MovementSmoothing = .05f;

        [Space]

        [Tooltip("Сила броска яйца")]
        public float ForseThrowEgg;

        [Tooltip("Сила выбивания твоего яйца")]
        public float ForseKickedEgg;

        [Space]

        [Tooltip("Входящая сила достаточная что бы умереть")]
        public float ForseToDeath;

        [Tooltip("Время нахождения в посмертии")]
        public float DeathTime;

        [Tooltip("Время иммунтитета. В случаях: Выкидывания яйца, Выбивания твоего яйца, Спавна")]
        public float ImmunityTime;
    }
}
