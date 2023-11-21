using System;

namespace SEC.Helpers
{
    public static class EnumerableHelper
    {
        public static T GetRandomElement<T>(this T[] array)
        {
            if (array == null || array.Length == 0)
            {
                throw new Exception("Array is empty or null");
            }

            int randomIndex = UnityEngine.Random.Range(0, array.Length);
            return array[randomIndex];
        }
    }
}
