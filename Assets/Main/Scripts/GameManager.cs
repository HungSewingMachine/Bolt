using System.Collections.Generic;
using System.Linq;
using Main.Scripts.Entity;
using Main.Scripts.State;
using Main.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public static int level = 0;
        
        public HexColor[] sampleColors;

        [SerializeField] private GameData    gameData;
        [SerializeField] private MapGenerator mapGenerator;

        public string fileName = "Level3";
        public bool   allowSpawn = true;

        private Hexagon[] hexagons;

        private void Start()
        {
            Application.targetFrameRate = 60;
            // var saveData = JsonHandler.GetLevelData($"Level{level}");
            var data = Resources.Load($"Level{level}") as TextAsset;
            var saveData = JsonUtility.FromJson<SavedData>(data.text);
            if (allowSpawn) mapGenerator.Generate(saveData.gridIndex, saveData.offset);

            hexagons = FindObjectsOfType<Hexagon>();
            CheckPossibleMove();

            ColorObject(hexagons);
        }

        public void CheckPossibleMove()
        {
            Debug.Log($"RedFlag check possible move!");
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
            
            InitializeColor(this,colors);

            colors.ShuffleNElements(9, 5);
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

        public void ReloadGame()
        {
            SceneManager.LoadScene(0);
        }

        public void LoadNextGame()
        {
            level += (level + 1) % 5;
            SceneManager.LoadScene(0);
        }

        // =======================================================
        [SerializeField] private BoxLine boxLine;

        [SerializeField] private WaitingArea waitingArea;

        public bool IsColorMatch(HexColor c) => c == boxLine.CurrentColor;

        public void InitializeColor(GameManager manager, IEnumerable<HexColor> list)
        {
            waitingArea = new WaitingArea(this);
            boxLine.Initialize(manager, waitingArea, list);
        }

        /// <summary>
        /// Calculate the position for hex to move. Then reupdate all posible move
        /// </summary>
        /// <param name="hex"></param>
        /// <returns>position which the hexagon move to</returns>
        public Vector3 RequestLanding(Hexagon hex)
        {
            if (!waitingArea.isProcessing && (IsBoxSlotStable && boxLine.CurrentBox.HasAvailableSlot && IsColorMatch(hex.ElementColor)))
            {
                return SendToBoxLine(hex);
            }

            return waitingArea.AddWrongColorObject(hex);
        }

        public Vector3 SendToBoxLine(Hexagon hex)
        {
            return boxLine.AddToBoxLine(hex);
        }

        public void FreeWaitLine(Box currentBox,List<Hexagon> list, Vector3[] waitPositions, List<Hexagon> waitList, ref int counter)
        {
            Debug.Log($"RedFlagX free wait line!");
            var smallestSlotIndex = int.MaxValue;
            var currentColor = currentBox.color;

            var listMove = new List<Hexagon>();
            for (int i = 0; i < list.Count; i++)
            {
                if (!currentBox.HasAvailableSlot) break;
                if (list[i].ElementColor == currentColor)
                {
                    var position = SendToBoxLine(list[i]);
                    list[i].MoveTo(position);
                    listMove.Add(list[i]);
                    Debug.Log($"RedFlag XXX {smallestSlotIndex} {i}");
                    if (smallestSlotIndex > i) smallestSlotIndex = i;
                }
            }


            for (int i = 0; i < list.Count; i++)
            {
                if (listMove.Contains(list[i])) continue;

                waitList.Add(list[i]);
                Debug.Log($"RedFlag XXX add to list {list[i].grid}");
                counter++;
                if (i > smallestSlotIndex)
                {
                    list[i].MoveTo(waitPositions[smallestSlotIndex]);
                    smallestSlotIndex++;
                    Debug.Log($"RedFlag XXX move to {list[i].grid}");
                }
            }
        }

        public bool IsBoxSlotStable => !boxLine.IsTransitionBox;

        public void CheckResult()
        {
            if (boxLine.CurrentBox.HasAvailableSlot)
            {
                ShowEndGame();
            }
            else
            {
                var nextColor = boxLine.GetNextColor();
                if (nextColor != HexColor.None)
                {
                    for (int i = 0; i < waitingArea.waitList.Count; i++)
                    {
                        if (waitingArea.waitList[i].ElementColor == nextColor)
                        {
                            return;
                        }
                    }
                }
                
                ShowEndGame();
            }
        }

        private void ShowEndGame()
        {
            Debug.Log($"RedFlag game lose");
            FindObjectOfType<UIManager>().ShowLoseGame();
        }
    }
}