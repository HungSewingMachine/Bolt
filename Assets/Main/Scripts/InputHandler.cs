using UnityEngine;

namespace Main.Scripts
{
    // TODO: need optimize somethings
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        
        [Range(0.1f,2)]
        public float cooldownBetweenClick = 0.5f;
        
        private float timer = 0;
        
        private void Update()
        {
            timer += Time.deltaTime;
            
            if (Input.GetMouseButton(0) && timer >= 0)
            {
                timer = -cooldownBetweenClick;
                Debug.Log($"RedFlagX btn press!");
                SelectObject();
            }
        }
        
        /// <summary>
        /// Select Character by cast a ray from screen to world position
        /// </summary>
        private void SelectObject()
        {
            var screenPos = Input.mousePosition;
            var ray = mainCamera.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out var hitInfo, 100, GameConstant.ENTITY_LAYER))
            {
                var visualPassenger = GameUtils.GetComponentFromCollider(hitInfo.collider);
                visualPassenger.FindTargetThenMove();
            }
        }
    }
}