using System;
using System.Collections;
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
        
        public HexColor CurrentColor
        {
            get
            {
                if (colors is not { Count: > 0 } || counter >= colors.Count)
                {
                    return HexColor.None;
                }

                return colors[counter];
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
        public void Initialize(IEnumerable<HexColor> list)
        {
            counter     = 0;
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

        public Vector3 AddToBoxLine(Hexagon hex, out bool isColorChange)
        {
            // Set parent and position calculation must be done before counter increase
            const int boxCapacity = GameConstant.BOX_CAPACITY;
            var result = new Vector3(counter % boxCapacity, BOX_Y_POSITION, BOX_Z_POSITION);
            var currentBox = boxTransforms[counter / boxCapacity];
            hex.RegisterParent(currentBox);
            
            // Current game color depend on counter so if we want delay color change event we delay counter
            isColorChange = (counter + 1) % boxCapacity == 0;

            return result;
        }

        public void UpdateLinePosition()
        {
            // Move the line
            var sequence = DOTween.Sequence();
            foreach (var t in boxTransforms)
            {
                sequence.Join(t.DOMove(t.WithXShift(GameConstant.DISTANCE_BETWEEN_BOX), GameConstant.BOX_MOVE_DURATION));
            }
        }

        public void Tick()
        {
            counter += 1;
        }
    }
}