using System;
using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts.Entity
{
    [Serializable]
    public class BoxLine
    {
        // public Transform       boxPrefab;
        // public List<Transform> boxTransforms;
        
        public List<HexColor> colors;

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
            var result = new List<HexColor>(list);
            result.Reverse();
            colors = result;
        }

        public Vector3 GetPositionInBox(Hexagon hex)
        {
            const int boxCapacity = GameConstant.BOX_CAPACITY;
            var result = new Vector3(counter % boxCapacity, BOX_Y_POSITION, BOX_Z_POSITION);
            counter       += 1;
            //isColorChange =  counter % boxCapacity == 0;
            
            return result;
        }
    }
}