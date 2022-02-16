using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public GameObject pausePanel;
    public List<Button> buttonList;
       
    //Pauses and unpauses the game
    public void pauseGame()
    {
        if (Time.timeScale == 1)
        {
            foreach(Button btn in buttonList)
            {
                btn.interactable = false;
            }

            pausePanel.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            foreach (Button btn in buttonList)
            {
                btn.interactable = true;
            }
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
