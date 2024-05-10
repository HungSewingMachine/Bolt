using System.Collections.Generic;
using Main.Scripts.Entity;
using UnityEngine;

namespace Main.Scripts.State
{
    [CreateAssetMenu(fileName = "GameState", menuName = "Game State", order = 51)]
    public class GameData : ScriptableObject
    {
        [SerializeField] private BoxLine boxLine;

        private WaitingArea waitingArea;
        

        public void InitializeColor(GameManager manager, IEnumerable<HexColor> list)
        {
            waitingArea = new WaitingArea();
            boxLine.Initialize(manager, waitingArea, list);
        }

        /// <summary>
        /// Process A hexagon moving
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public Vector3 RequestLanding(Hexagon hex, bool fromWaitLine)
        {
            var isColorMatch = hex.ElementColor == boxLine.CurrentColor;
            
            if (isColorMatch && (fromWaitLine || !boxLine.IsTransitionBox))
            {
                var position = boxLine.AddToBoxLine(hex);

                return position;
            }

            return waitingArea.AddWrongColorObject(hex);
        }
    }
}