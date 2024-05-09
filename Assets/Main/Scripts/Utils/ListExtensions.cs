using System;
using System.Collections.Generic;

namespace Main.Scripts.Utils
{
    public static class ListExtensions
    {
        private static Random random = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
        /// <summary>
        /// Shuffle N elements, then N element, then N element until end.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="N"></param>
        /// <typeparam name="T"></typeparam>
        public static void ShuffleNElements<T>(this IList<T> list, int N)
        {
            int length = list.Count;
            int currentIndex = 0;

            while (currentIndex < length)
            {
                int remaining = length - currentIndex;
                int shuffleCount = Math.Min(N, remaining);

                // Shuffle N elements starting from currentIndex
                for (int i = currentIndex; i < currentIndex + shuffleCount; i++)
                {
                    int randomIndex = random.Next(i, length);
                    (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
                }

                currentIndex += shuffleCount;
            }
        }
    }
}