using UnityEngine;

namespace Main.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public GameObject panel;
        public GameObject winTxt;
        public GameObject loseTxt;
        
        public void ShowLoseGame()
        {
            panel.SetActive(true);
            loseTxt.SetActive(true);
        }

        public void ShowWinGame()
        {
            panel.SetActive(true);
            winTxt.SetActive(true);
        }
    }
}