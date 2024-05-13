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
            new Vector3(1.6f,0,7),
            new Vector3(3.2f,0,7),
            new Vector3(4.8f,0,7),
            new Vector3(6.4f,0,7),
            new Vector3(8,0,7),
        };

        public int counter = 0;

        public List<Hexagon> waitList;

        private GameManager gameManager;

        public WaitingArea(GameManager manager)
        {
            waitList = new List<Hexagon>();
            gameManager = manager;
        }

        /// <summary>
        /// Loop through the list, free hexagon that have the same color, rearrange list.
        /// </summary>
        /// <param name="c"></param>
        public void OnGameColorChanged(Box currentBox)
        {
            Debug.Log("XXX On Game Color Changed!");

            if (!currentBox.HasAvailableSlot) return;

            // If has slot

            var c = currentBox.color;
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
                Debug.Log($"RedFlagX list length {newList.Count}!");
                waitList.Clear();
                counter = 0;


                gameManager.FreeWaitLine(currentBox, newList, waitPositions, waitList, ref counter);
            }
        }

        public bool AddWrongColorObject(Hexagon hex, out Vector3 position)
        {
            if (counter >= waitPositions.Length)
            {
                position = Vector3.zero;
                return false;
            }

            var index = counter;
            var currentAvailableSlot = waitPositions[index];
            waitList.Add(hex);
            counter++;

            Debug.Log($"RedFlagX add hex!");

            if (IsFullElement)
            {
                gameManager.CheckResult();
            }
            position = currentAvailableSlot;
            return true;
        }

        public bool IsFullElement => counter >= waitPositions.Length;
    }
}