using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Entity;
using Main.Scripts.State;
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
        None,
    }

    public class GameManager : MonoBehaviour
    {
        public HexColor[] sampleColors;

        [SerializeField] private GameData    gameData;
        [SerializeField] private MapGenerator mapGenerator;

        public string fileName = "Level3";
        public bool   allowSpawn = true;

        private Hexagon[] hexagons;

        private void Start()
        {
            var saveData = JsonHandler.GetLevelData(fileName);
            if (allowSpawn) mapGenerator.Generate(saveData.gridIndex, saveData.offset);

            hexagons = FindObjectsOfType<Hexagon>();
            CheckPossibleMove();

            ColorObject(hexagons);
        }

        public void CheckPossibleMove()
        {
            foreach (var hexagon in hexagons)
            {
                hexagon.CheckMovable();
            }
        }

        /// <summary>
        /// Use to color all Hexagons
        /// </summary>
        private void ColorObject(Hexagon[] objects)
        {
            var sampleColorLength = sampleColors.Length;
            var resetIndex = 3 * sampleColorLength;

            // Construct color array
            List<HexColor> colors = new List<HexColor>();
            for (int i = 0; i < objects.Length; i++)
            {
                var colorIndex = (i % resetIndex) / 3;
                colors.Add(GetColorFromIndex(colorIndex));
            }
            
            gameData.InitializeColor(colors);
            
            colors.ShuffleNElements(9);
            var increasingLayerList = objects.OrderBy(hex => hex.Coordinate.y).ToList();
            
            for (int i = 0; i < objects.Length; i++)
            {
                increasingLayerList[i].ChangeColor(colors[i]);
            }

            HexColor GetColorFromIndex(int index)
            {
                return index switch
                {
                    0 => HexColor.Red,
                    1 => HexColor.Blue,
                    2 => HexColor.Green,
                    3 => HexColor.Pink,
                    _ => HexColor.None,
                };
            }
        }
    }
}