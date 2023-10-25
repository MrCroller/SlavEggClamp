using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SEC.SO
{
    [CreateAssetMenu(fileName = "CameraShakeSetting", menuName = "SEC/Effect/CameraShake")]
    public class CameraShakeSetting : ScriptableObject
    {
        [Header("Убийство игрока")]
        public float PlayerKill_Time = 0.5f;
        public float PlayerKill_Forse = 1f;

        [Header("Удар по игроку")]
        public float PlayerKick_Time = 0.3f;
        public float PlayerKick_Forse = 0.8f;
    }
}
