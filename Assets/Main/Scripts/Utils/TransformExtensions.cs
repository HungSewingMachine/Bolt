using UnityEngine;

namespace Main.Scripts.Utils
{
    public static class TransformExtensions
    {
        public static void SetParentAndReset(this Transform transform, Transform newParent)
        {
            transform.SetParent(null, true);      // Detach from current parent
            transform.SetParent(newParent, true); // Set new parent
        }
        
        public static Vector3 WithXShift(this Transform t, float offset)
        {
            var position = t.position;
            return new Vector3(position.x + offset, position.y, position.z);
        }
    }
}