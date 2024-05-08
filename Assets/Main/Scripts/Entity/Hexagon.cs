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

        private Vector3 GetStartOriginPoint(Transform t)
        {
            return t.position + height * Vector3.up;
        }
        
        public void CheckMovable()
        {
            if (isAvailable) return;

            foreach (var vertex in checkVertexes)
            {
                if (Physics.Raycast(vertex, Vector3.up, MAX_CHECK_DISTANCE, 1 << 7))
                {
                    isAvailable = true;
                    LogInfo();
                    // Turn on anim, ...
                    return;
                }
            }
        }

        private Vector3 GetPositionFromAngle(Transform t, float angle)
        {
            var origin = GetStartOriginPoint(t);
            return new Vector3(origin.x + radius * Mathf.Cos(angle), origin.y, origin.z + radius * Mathf.Sin(angle));
        }

        public virtual void LogInfo()
        {
            Debug.Log($"RedFlag Detect Available");
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < angles.Length; i++)
            {
                Gizmos.DrawSphere(GetPositionFromAngle(transform, angles[i] - Mathf.PI / 6),0.01f);
            }
        }
    }
}
