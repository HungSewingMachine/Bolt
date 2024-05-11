using Main.Scripts.Entity;
using UnityEngine;

namespace Main.Scripts
{
    /// <summary>
    /// Init data for 2 Hexagon childs
    /// </summary>
    public class HexBundle : MonoBehaviour
    {
        public Hexagon top;
        public Hexagon bottom;

        public void Initialize(Grid grid, GameManager manager)
        {
            var y = grid.y * 2;
            top.Initialize(new Grid((grid.x, y + 1, grid.z)), manager);
            bottom.Initialize(new Grid((grid.x, y, grid.z)), manager);
        }
    }
}