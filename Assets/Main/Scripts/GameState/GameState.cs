using System.Collections.Generic;
using Main.Scripts.Entity;
using UnityEngine;

namespace Main.Scripts.State
{
    [CreateAssetMenu(fileName = "GameState", menuName = "Game State", order = 51)]
    public class GameState : ScriptableObject
    {
        public Vector3  boxPosition = new Vector3(0, 0, 10);
        
        public HexColor currentColor;

        public List<HexColor> colors;

        private WaitingArea waitingArea = new WaitingArea();

        public void InitializeColor(List<HexColor> list)
        {
            colors       = new List<HexColor>(list);
            currentColor = colors[^1];
        }

        public Vector3 RequestLandingPosition(Hexagon hex)
        {
            // if (colors == null || colors.Count == 0) return Vector3.zero;
            // currentColor = colors[^1];
            
            if (hex.ElementColor == currentColor)
            {
                colors.RemoveAt(colors.Count - 1);
                currentColor = colors[^1];
                return boxPosition;
            }

            return waitingArea.AddWrongColorObject(hex);
        }
    }
}