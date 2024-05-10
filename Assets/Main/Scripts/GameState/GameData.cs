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

        public void InitializeColor(List<HexColor> list)
        {
            waitingArea = new WaitingArea();
            boxLine     = new BoxLine();
            boxLine.Initialize(list);
        }

        public Vector3 RequestLandingPosition(Hexagon hex)
        {
            var isColorMatch = hex.ElementColor == boxLine.CurrentColor;
            if (isColorMatch)
            {
                var position = boxLine.GetPositionInBox(hex);
                // if color change => move next box to it
                return position;
            }

            return waitingArea.AddWrongColorObject(hex);
        }
    }
}