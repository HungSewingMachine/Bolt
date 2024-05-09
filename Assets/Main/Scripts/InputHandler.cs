using Main.Scripts.Entity;
using UnityEngine;

namespace Main.Scripts
{
    // TODO: need optimize somethings
    public class InputHandler : MonoBehaviour
    {
        [Range(0.1f,2)]
        public float cooldownBetweenClick = 0.5f;
        
        private float timer = 0;

        [SerializeField] private Transform movePosition;
        
        private void Update()
        {
            timer += Time.deltaTime;
            
            if (Input.GetMouseButton(0) && timer >= 0)
            {
                SelectObject();
                timer = -cooldownBetweenClick;
            }
        }
        
        /// <summary>
        /// Select Character by cast a ray from screen to world position
        /// </summary>
        private static void SelectObject()
        {
            var screenPos = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out var hitInfo, 100, HexagonInfo.ENTITY_LAYER))
            {
                var visualPassenger = hitInfo.collider.GetComponentInParent<Hexagon>();
                visualPassenger.FindTargetAndMove();
            }
        }
    }
}