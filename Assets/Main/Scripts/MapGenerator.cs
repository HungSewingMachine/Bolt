using Main.Scripts.Entity;
using UnityEngine;

namespace Main.Scripts
{
    public class MapGenerator : MonoBehaviour
    {
        public GameObject    prefab;
        public string        fileName;

        public bool blockSpawn = false;

        private void Start()
        {
            if (!blockSpawn)
            {
                var saveData = JsonHandler.GetLevelData(fileName);
                for (int i = 0; i < saveData.gridIndex.Count; i++)
                {
                    Instantiate(prefab, GetPosition(saveData.gridIndex[i], saveData.offset), Quaternion.identity);
                }
            }
            
            // Testing

            var hexagons = FindObjectsOfType<Hexagon>();
            foreach (var hexagon in hexagons)
            {
                hexagon.CheckMovable();
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