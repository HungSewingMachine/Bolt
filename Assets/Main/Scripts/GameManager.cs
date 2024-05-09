using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Entity;
using Main.Scripts.Utils;
using UnityEngine;

namespace Main.Scripts
{
    public enum HexColor
    {
        Red,
        Green,
        Blue,
        Pink,
    }

    public class GameManager : MonoBehaviour
    {
        public HexColor[] sampleColors;

        [SerializeField] private MapGenerator mapGenerator;

        public string fileName = "Level3";
        public bool   allowSpawn = true;

        private Hexagon[] hexagons;

        private void Start()
        {
            var saveData = JsonHandler.GetLevelData(fileName);
            if (allowSpawn) mapGenerator.Generate(saveData.gridIndex, saveData.offset);

            hexagons = FindObjectsOfType<Hexagon>();
            foreach (var hexagon in hexagons)
            {
                hexagon.CheckMovable();
            }

            ColorObject(hexagons);
        }

        /// <summary>
        /// Use to color all Hexagons
        /// </summary>
        private void ColorObject(Hexagon[] objects)
        {
            var sampleColorLength = sampleColors.Length;
            var resetIndex = 3 * sampleColorLength;

            // Construct color array
            List<Color> colors = new List<Color>();
            for (int i = 0; i < objects.Length; i++)
            {
                var colorIndex = (i % resetIndex) / 3;
                colors.Add(GetColorFromIndex(colorIndex));
            }
            
            colors.ShuffleNElements(9);
            var increasingLayerList = objects.OrderBy(hex => hex.Coordinate.y).ToList();
            
            for (int i = 0; i < objects.Length; i++)
            {
                increasingLayerList[i].ChangeColor(colors[i]);
            }

            Color GetColorFromIndex(int index)
            {
                return index switch
                {
                    0 => Color.red,
                    1 => Color.blue,
                    2 => Color.green,
                    3 => Color.magenta,
                    _ => Color.white,
                };
            }
        }
    }
}