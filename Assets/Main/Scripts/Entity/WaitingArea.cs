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

        public int counter = 0;

        public List<Hexagon> waitList;

        private GameManager gameManager;

        public WaitingArea(GameManager manager)
        {
            waitList = new List<Hexagon>();
            gameManager = manager;
        }

        public void MarkColorChange()
        {
            extraCounter = counter;
            isReserveMode = true;
        }

        /// <summary>
        /// Loop through the list, free hexagon that have the same color, rearrange list.
        /// </summary>
        /// <param name="c"></param>
        public void OnGameColorChanged(List<HexColor> colors, int colorCounter)
        {
            Debug.Log("XXX On Game Color Changed!");

            var c = colors[colorCounter];
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


                gameManager.FreeWaitLine(c, newList, waitPositions, waitList, ref counter);
            }

            isReserveMode = false;
        }

        public bool isReserveMode;
        public int extraCounter;
        public List<Hexagon> extraList = new List<Hexagon>();

        public void Update()
        {
            if (!isReserveMode)
            {
                if (extraList.Count != 0)
                {
                    for (int i = 0; i < extraList.Count; i++)
                    {
                        var position = gameManager.SendToBoxLine(extraList[i]);
                        extraList[i].MoveTo(position);
                    }
                    extraList.Clear();
                    extraCounter = counter;
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

            var index = isReserveMode ? extraCounter : counter;
            var currentAvailableSlot = waitPositions[index];
            if (isReserveMode)
            {
                extraList.Add(hex);
                extraCounter++;
            }
            else
            {
                waitList.Add(hex);
                counter++;
            }

            Debug.Log($"RedFlagX add hex!");

            if (extraCounter >= waitPositions.Length || counter >= waitPositions.Length)
            {
                Debug.Log($"RedFlag game lose");
                GameObject.FindObjectOfType<UIManager>().ShowLoseGame();
            }
            return currentAvailableSlot;
        }
    }
}