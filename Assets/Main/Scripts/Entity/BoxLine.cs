using System;
using System.Collections.Generic;
using DG.Tweening;
using Main.Scripts.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Main.Scripts.Entity
{
    [Serializable]
    public class Box
    {
        public Transform boxTransform;
        public HexColor  color;
        public int       index       = 0;
        public int       actualIndex = 0;

        private BoxLine boxLine;

        public Box(Transform t, HexColor c, BoxLine line)
        {
            index        = 0;
            boxTransform = t;
            color        = c;
            boxLine      = line;
        }

        public bool HasAvailableSlot => index < 3;

        /// <summary>
        /// Only call when 
        /// </summary>
        /// <returns></returns>
        public Vector3 GetSlotPosition()
        {
            var xPos = index + GameConstant.BOX_START_POSITION - 2 * GameConstant.X;
            const float yPos = GameConstant.BOX_Y_POSITION + GameConstant.SLOT_OFFSET_HEIGHT;
            const float zPos = GameConstant.BOX_Z_POSITION;
            var position = new Vector3(xPos, yPos, zPos);
            index++;
            return position;
        }

        /// <summary>
        /// Called when hexagon move complete to box
        /// </summary>
        public void OnMoveToBoxComplete()
        {
            actualIndex += 1;
            if (index == 3 && actualIndex >= 3)
            {
                // Change box
                actualIndex = 0;
                Debug.Log($"RedFlagXX change box");
                boxLine.PlayBoxTransition();
            }
        }
    }

    [Serializable]
    public class BoxLine
    {
        [SerializeField] private Transform boxPrefab;

        [SerializeField] private List<Box> boxList;

        private GameManager gameManager;
        private WaitingArea waitingArea;

        public int cachedColorCounter = 0;
        public int boxCounter         = 0;

        public Box CurrentBox => boxList[boxCounter];

        [field: SerializeField] public bool IsTransitionBox { get; private set; }

        public HexColor CurrentColor => CurrentBox.color;

        /// <summary>
        /// Get the list we use to set up from floor to ceil.
        /// We reverse it so that we can utilize counter variable to travel from ceil to floor.
        /// </summary>
        /// <param name="area"></param>
        /// <param name="list"></param>
        /// <param name="manager"></param>
        public void Initialize(GameManager manager, WaitingArea area, IEnumerable<HexColor> list)
        {
            IsTransitionBox    = false;
            boxCounter         = 0;
            cachedColorCounter = 0;
            gameManager        = manager;
            waitingArea        = area;
            // Set up color data
            var listColor = new List<HexColor>(list);
            listColor.Reverse();

            var numberOfBox = listColor.Count / GameConstant.BOX_CAPACITY;

            boxList = new List<Box>();
            for (int i = 0; i < numberOfBox; i++)
            {
                var boxXPosition = GameConstant.BOX_START_POSITION - i * GameConstant.DISTANCE_BETWEEN_BOX;
                var box = Object.Instantiate(boxPrefab, new Vector3(boxXPosition, GameConstant.BOX_Y_POSITION, GameConstant.BOX_Z_POSITION),
                    Quaternion.identity);
                var c = listColor[i * GameConstant.BOX_CAPACITY];
                boxList.Add(new Box(box, c, this));
                var renderer = box.GetComponent<Renderer>();
                GameUtils.ColorObject(renderer, c);
            }
        }

        public Vector3 AddToBoxLine(Hexagon hex)
        {
            var result = CurrentBox.GetSlotPosition();
            hex.RegisterBox(CurrentBox);

            return result;
        }

        /// <summary>
        /// The time delay = time travel to box + time box play animation close.
        /// Currently test for 2s
        /// </summary>
        public void PlayBoxTransition()
        {
            IsTransitionBox = true;

            gameManager.DelayExecute(() =>
            {
                UpdateLinePosition(() =>
                {
                    // OnComplete
                    boxCounter += 1;
                    if (boxCounter >= boxList.Count)
                    {
                        GameObject.FindObjectOfType<UIManager>().ShowWinGame();
                    }

                    // cachedColorCounter += 3;
                    IsTransitionBox = false;
                    Debug.Log("XXX IsTransition false!");

                    waitingArea.OnGameColorChanged(CurrentBox);
                });
            }, GameConstant.BOX_DELAY_TIME);
        }

        public void UpdateLinePosition(Action onComplete = null, Action callback = null)
        {
            // Move the line
            var sequence = DOTween.Sequence();
            foreach (var box in boxList)
            {
                var t = box.boxTransform;
                sequence.Join(t.DOMove(t.WithXShift(GameConstant.DISTANCE_BETWEEN_BOX), GameConstant.BOX_MOVE_DURATION));
            }

            sequence.AppendCallback(() => callback?.Invoke());
            //sequence.AppendInterval(0.1f);

            sequence.OnComplete(() => onComplete?.Invoke());
        }
    }
}