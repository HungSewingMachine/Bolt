using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace Main.Scripts.Entity
{
    public class Hexagon : MonoBehaviour
    {
        // === Only Test change color
        public Color      testColor = Color.black;
        public Renderer[] renderers;
        // === Only Test change color

        public Grid grid;
        
        public Grid Coordinate { get; private set; }
        
        public float height = 0.2f / 2 + 0.1f;
        public float radius = 0.58f - 0.08f;
        
        public bool isAvailable = false;

        private const float UNIT_ANGLE = Mathf.PI / 3;
        private const float MAX_CHECK_DISTANCE = 0.2f;
        private const int HEXAGON_LAYER = 1 << 7;

        private static readonly float[] Angles = new[]
        {
            0f, UNIT_ANGLE, 2 * UNIT_ANGLE, 3 * UNIT_ANGLE, 4 * UNIT_ANGLE, 5 * UNIT_ANGLE,
        };

        // if using custom shader, replace it with appropriate name not _Color
        private static readonly int ColorField = Shader.PropertyToID("_Color"); 

        private Vector3 GetStartOriginPoint(Transform t)
        {
            return t.position;
        }

        public void CheckMovable()
        {
            Debug.Log($"RedFlag check movable!");
            if (isAvailable) return;

            foreach (var angle in Angles)
            {
                var checkPoint = GetPositionFromAngle(transform, angle - Mathf.PI / 6);
                Debug.Log($"RedFlag check point {checkPoint}!");
                if (Physics.Raycast(checkPoint, Vector3.up, 10, HEXAGON_LAYER))
                {
                    isAvailable = false;
                    LogCanNotMove();
                    return;
                }
            }

            // Turn on anim, ...
            isAvailable = true;
            ChangeColor(Color.white);
        }

        private Vector3 GetPositionFromAngle(Transform t, float angle)
        {
            var origin = GetStartOriginPoint(t);
            return new Vector3(origin.x + radius * Mathf.Cos(angle), origin.y, origin.z + radius * Mathf.Sin(angle));
        }

        protected virtual void LogCanNotMove()
        {
            Debug.Log($"RedFlag Detect False");
        }

        [Button]
        public void TestChangeColor()
        {
            ChangeColor(testColor);
        }
        public void ChangeColor(Color color)
        {
            Debug.Log($"RedFlag color change! {color}");

            var proBlock = new MaterialPropertyBlock();
            proBlock.SetColor(ColorField, color);
            foreach (var r in renderers)
            {
                r.SetPropertyBlock(proBlock);
            }
        }

        public void Initialize(Grid grid)
        {
            this.grid  = grid;
            Coordinate = grid;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (isAvailable) return;
            Gizmos.color = Color.red;
            for (int i = 0; i < Angles.Length; i++)
            {
                var checkPoint = GetPositionFromAngle(transform, Angles[i] - Mathf.PI / 6);
                Gizmos.DrawSphere(checkPoint, 0.01f);
                Gizmos.DrawLine(checkPoint, checkPoint + Vector3.up * MAX_CHECK_DISTANCE);
            }
        }
#endif
    }
}