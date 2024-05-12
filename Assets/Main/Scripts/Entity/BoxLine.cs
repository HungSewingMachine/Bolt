using System;
using System.Collections.Generic;
using DG.Tweening;
using Main.Scripts.Utils;
using UnityEngine;

namespace Main.Scripts.Entity
{
    [Serializable]
    public class BoxLine
    {
        [SerializeField] private Transform       boxPrefab;
        [SerializeField] private List<Transform> boxTransforms;

        [SerializeField] private List<HexColor> colors;
        
        private GameManager gameManager;
        private WaitingArea waitingArea;

        public int cachedColorCounter = 0;

        [field : SerializeField] public bool IsTransitionBox { get; private set; }

        public HexColor CurrentColor
        {
            get
            {
                if (colors is not { Count: > 0 } || counter >= colors.Count)
                {
                    return HexColor.None;
                }

                return colors[cachedColorCounter];
            }
        }

        private const float BOX_Y_POSITION = 0f;
        private const float BOX_Z_POSITION = 10f;

        public int counter = 0;

        /// <summary>
        /// Get the list we use to set up from floor to ceil.
        /// We reverse it so that we can utilize counter variable to travel from ceil to floor.
        /// </summary>
        /// <param name="list"></param>
        public void Initialize(GameManager manager, WaitingArea area, IEnumerable<HexColor> list)
        {
            
            IsTransitionBox = false;
            counter         = 0;
            cachedColorCounter = 0;
            gameManager     = manager;
            waitingArea     = area;
            // Set up color data
            var listColor = new List<HexColor>(list);
            listColor.Reverse();
            colors = listColor;

            var numberOfBox = listColor.Count / GameConstant.BOX_CAPACITY;
            boxTransforms = new List<Transform>();
            for (var i = 0; i < numberOfBox; i++)
            {
                var box = GameObject.Instantiate(boxPrefab, new Vector3(1 - i * GameConstant.DISTANCE_BETWEEN_BOX, BOX_Y_POSITION, BOX_Z_POSITION),
                    Quaternion.identity);
                boxTransforms.Add(box);
                var renderer = box.GetComponent<Renderer>();
                GameUtils.ColorObject(renderer, colors[i * GameConstant.BOX_CAPACITY]);
            }
        }

        public Vector3 AddToBoxLine(Hexagon hex)
        {
            // Set parent and position calculation must be done before counter increase
            const int boxCapacity = GameConstant.BOX_CAPACITY;
            var result = new Vector3(counter % boxCapacity, BOX_Y_POSITION, BOX_Z_POSITION);
            var currentBox = boxTransforms[counter / boxCapacity];
            counter += 1;
            if (counter >= colors.Count)
            {
                Debug.Log($"RedFlag log game win!");
                GameObject.FindObjectOfType<UIManager>().ShowWinGame();
            }
            
            hex.RegisterParent(currentBox);
            
            // Current game color depend on counter so if we want delay color change event we delay counter
            var isColorChange = counter % boxCapacity == 0;
            if (isColorChange)
            {
                // Run Animation and done
                // Delay 2s chay animation
                PlayBoxTransition();
            }

            return result;
        }

        /// <summary>
        /// The time delay = time travel to box + time box play animation close.
        /// Currently test for 2s
        /// </summary>
        private void PlayBoxTransition()
        {
            IsTransitionBox = true;
            waitingArea.MarkColorChange();
            gameManager.DelayExecute(() =>
            {
                UpdateLinePosition(() =>
                {
                    cachedColorCounter += 3;
                    IsTransitionBox = false;
                    Debug.Log("XXX Transition false!");
                }, () =>
                {
                    waitingArea.OnGameColorChanged(colors, counter);
                    
                });
                
            },GameConstant.BOX_DELAY_TIME);
        }

        public void UpdateLinePosition(Action onComplete = null, Action callback = null)
        {
            // Move the line
            var sequence = DOTween.Sequence();
            foreach (var t in boxTransforms)
            {
                sequence.Join(t.DOMove(t.WithXShift(GameConstant.DISTANCE_BETWEEN_BOX), GameConstant.BOX_MOVE_DURATION));
            }

            sequence.AppendCallback(() => callback?.Invoke());
            //sequence.AppendInterval(0.1f);

            sequence.OnComplete(() => onComplete?.Invoke());
        }
    }
}