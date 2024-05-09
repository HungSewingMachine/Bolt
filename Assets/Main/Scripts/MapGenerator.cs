using System.Collections.Generic;
using Main.Scripts.Entity;
using UnityEngine;

namespace Main.Scripts
{
    public class MapGenerator : MonoBehaviour
    {
        public HexBundle prefab;

        public void Generate(List<Grid> grids, Vector3 offset)
        {
            foreach (var grid in grids)
            {
                var bundle = Instantiate(prefab, GetPosition(grid, offset), Quaternion.identity);
                bundle.Initialize(grid);
            }
        }

        /// <summary>
        /// Convert from gridIndex to position
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static Vector3 GetPosition(Grid grid, Vector3 offset)
        {
            var offsetX = Mathf.Approximately(grid.z % 2, 1) ? HexagonInfo.X : 0;
            return new Vector3(grid.x * HexagonInfo.X + offsetX, grid.y * HexagonInfo.Y, grid.z * HexagonInfo.Z) + offset;
        }
    }
}