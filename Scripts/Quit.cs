using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour {

    public GameObject pausePanel;
    public TextMeshProUGUI playPauseText;

    public void quitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void resume()
    {
        Time.timeScale = 1;
        playPauseText.text = "\U0000f04b";
        pausePanel.SetActive(false);
    }
}
