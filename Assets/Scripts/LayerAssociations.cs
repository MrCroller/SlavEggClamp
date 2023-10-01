using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEC.Associations
{
    public static class LayerAssociations
    {
        public const string Nothing = "Nothing";

        public const int Player = 3;
        public const int Egg = 7;
        /// <summary>
        /// Игрок с яйцом
        /// </summary>
        public const int PlayerTakeEgg = 8;
        /// <summary>
        /// Яйцо у игрока
        /// </summary>
        public const int PlayerEgg = 10;
    }
}
