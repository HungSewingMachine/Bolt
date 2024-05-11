using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts.Entity
{
    /// <summary>
    /// manage list o
    /// </summary>
    [Serializable]
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

        [SerializeField] private int counter = 0;

        public List<Hexagon> waitList;

        public WaitingArea()
        {
            waitList = new List<Hexagon>();
        }

        /// <summary>
        /// Loop through the list, free hexagon that have the same color, rearrange list.
        /// </summary>
        /// <param name="c"></param>
        public void OnGameColorChanged(HexColor c)
        {
            var hasMatchColor = false;
            for (int i = 0; i < waitList.Count; i++)
            {
                if (waitList[i].ElementColor == c)
                {
                    hasMatchColor = true;
                    break;
                }
            }

            if (hasMatchColor)
            {
                var newList = new List<Hexagon>(waitList);
            
                waitList.Clear();
                counter = 0;

                for (int i = 0; i < newList.Count; i++)
                {
                    newList[i].FindTargetThenMove(true);
                }
            }
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
            if (counter >= waitPositions.Length)
            {
                Debug.Log($"RedFlag game lose");
                GameObject.FindObjectOfType<UIManager>().ShowLoseGame();
            }
            return currentAvailableSlot;
        }
    }
}