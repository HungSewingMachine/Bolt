using System;
using UnityEngine;

namespace Main.Scripts
{
    public class UIManager : MonoBehaviour
    {
        public GameObject panel;
        public GameObject winTxt;
        public GameObject loseTxt;

        public GameObject replayBtn;
        public GameObject nextRoundBtn;

        private void Start()
        {
            panel.SetActive(false);
            winTxt.SetActive(false);
            loseTxt.SetActive(false);
            replayBtn.SetActive(false);
            nextRoundBtn.SetActive(false);
        }

        public void ShowLoseGame()
        {
            panel.SetActive(true);
            loseTxt.SetActive(true);
            replayBtn.SetActive(true);
        }

        public void ShowWinGame()
        {
            panel.SetActive(true);
            winTxt.SetActive(true);
            nextRoundBtn.SetActive(true);
        }
    }
}