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
        private GameManager gameManager;

        public void InitializeColor(GameManager manager, IEnumerable<HexColor> list)
        {
            gameManager = manager;
            waitingArea = new WaitingArea();
            boxLine.Initialize(list);
        }

        /// <summary>
        /// Process A hexagon moving
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public Vector3 RequestLanding(Hexagon hex)
        {
            var isColorMatch = hex.ElementColor == boxLine.CurrentColor;
            if (isColorMatch)
            {
                var position = boxLine.AddToBoxLine(hex, out var isColorChange);
                if (isColorChange)
                {
                    // Wait
                    gameManager.DelaySec(() =>
                    {
                        boxLine.UpdateLinePosition();

                        boxLine.Tick();

                        // Handle wait line

                    }, 2f);
                }
                else
                {
                    boxLine.Tick();
                }

                return position;
            }

            return waitingArea.AddWrongColorObject(hex);
        }
    }
}