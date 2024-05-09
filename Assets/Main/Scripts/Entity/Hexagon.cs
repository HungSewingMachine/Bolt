using System.Collections.Generic;
using UnityEngine;

namespace Main.Scripts.Entity
{
    public class Hexagon : MonoBehaviour
    {
        public float height = 0.2f / 2 + 0.1f;
        public float radius = 0.58f - 0.08f;

        public const float UNIT_ANGLE = Mathf.PI / 3;
        
        public List<Vector3> checkVertexes;
        public float[]       angles = new[]
        {
            0f, UNIT_ANGLE, 2 * UNIT_ANGLE, 3 * UNIT_ANGLE, 4 * UNIT_ANGLE, 5 * UNIT_ANGLE,
        };

        public bool isAvailable = false;
        
        private const float MAX_CHECK_DISTANCE = 0.2f;

        private const int HEXAGON_LAYER = 1 << 7;

        private Vector3 GetStartOriginPoint(Transform t)
        {
            return t.position;
        }
        
        public void CheckMovable()
        {
            // Debug.Log($"RedFlag check movable!");
            // if (isAvailable) return;
            Debug.Log($"RedFlag check movable2!");

            foreach (var angle in angles)  
            {
                var checkPoint = GetPositionFromAngle(transform, angle - Mathf.PI / 6);
                Debug.Log($"RedFlag check point {checkPoint}!");
                if (Physics.Raycast(checkPoint, Vector3.up, 10, HEXAGON_LAYER))
                {
                    isAvailable = false;
                    LogInfo();
                    return;
                }
            }

            // Turn on anim, ...
            isAvailable = true;
        }

        private Vector3 GetPositionFromAngle(Transform t, float angle)
        {
            var origin = GetStartOriginPoint(t);
            return new Vector3(origin.x + radius * Mathf.Cos(angle), origin.y, origin.z + radius * Mathf.Sin(angle));
        }

        public virtual void LogInfo()
        {
            Debug.Log($"RedFlag Detect False");
        }

        private void OnDrawGizmos()
        {
            if (isAvailable) return;
            Gizmos.color = Color.red;
            for (int i = 0; i < angles.Length; i++)
            {
                var checkPoint = GetPositionFromAngle(transform, angles[i] - Mathf.PI / 6);
                Gizmos.DrawSphere(checkPoint,0.01f);
                Gizmos.DrawLine(checkPoint, checkPoint + Vector3.up * MAX_CHECK_DISTANCE);
            }
        }
    }
}
