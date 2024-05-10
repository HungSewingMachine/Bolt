using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts.Entity
{
    /// <summary>
    /// manage list o
    /// </summary>
    public class WaitingArea
    {
        public Vector3[] waitPositions = new Vector3[6]
        {
            new Vector3(0,0,7),
            new Vector3(2,0,7),
            new Vector3(4,0,7),
            new Vector3(6,0,7),
            new Vector3(8,0,7),
            new Vector3(10,0,7),
        };

        private int counter = 0;

        public List<Hexagon> waitList;

        public WaitingArea()
        {
            waitList = new List<Hexagon>();
        }

        /// <summary>
        /// Loop through the list, free hexagon that have the same color, rearrange list.
        /// Provide extra method to add hexagon to list add hexagon to list if wrong color was picked
        /// </summary>
        /// <param name="c"></param>
        public void OnGameColorChanged(HexColor c)
        {
            // Push het cac phan tu cung mau
            // Sap xep lai cac phan tu con lai
            // cap nhat currentAvailableSlot

            // var matchList = new List<Hexagon>();
            // var remainList = new List<Hexagon>();
            //
            // for (int i = 0; i < waitList.Count; i++)
            // {
            //     if (waitList[i].ElementColor == c)
            //     {
            //         matchList.Add(waitList[i]);
            //     }
            //     else
            //     {
            //         remainList.Add(waitList[i]);
            //     }
            // }

            var newList = new List<Hexagon>(waitList);
            
            waitList.Clear();
            counter = 0;

            var hasAMatchColor = false;
            for (int i = 0; i < newList.Count; i++)
            {
                // if (!hasAMatchColor)
                // {
                //     hasAMatchColor = newList[i].ElementColor == c;
                // }
                //
                // if (hasAMatchColor)
                // {
                //     newList[i].FindTargetAndMove();
                // }
                
                newList[i].FindTargetAndMove();
            }

            // var index = 0;
            // for (int i = 0; i < waitList.Count; i++)
            // {
            //     if (waitList[i] != null)
            //     {
            //         waitList[index] =  waitList[i];
            //         index           += 1;
            //     }
            // }
            
        }

        public Vector3 AddWrongColorObject(Hexagon hex)
        {
            if (counter >= waitPositions.Length)
            {
                Debug.Log($"RedFlag end game!");
                return Vector3.zero;
            }
            
            var currentAvailableSlot = waitPositions[counter];
            waitList.Add(hex);
            counter++;
            return currentAvailableSlot;
        }
    }
}