using System.Collections.Generic;
using Main.Scripts.Entity;
using UnityEngine;

namespace Main.Scripts
{
    public class MapGenerator : MonoBehaviour
    {
        public HexBundle prefab;
        public GameManager gameManager;

        public void Generate(List<Grid> grids, Vector3 offset)
        {
            foreach (var grid in grids)
            {
                var bundle = Instantiate(prefab, GetPosition(grid, offset), Quaternion.identity);
                bundle.Initialize(grid, gameManager);
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
            var offsetX = Mathf.Approximately(grid.z % 2, 1) ? GameConstant.X : 0;
            return new Vector3(grid.x * GameConstant.X + offsetX, grid.y * GameConstant.Y_BUILD, grid.z * GameConstant.Z) + offset;
        }
    }
}