using TMPro;
using UnityEngine;

public class Sound : MonoBehaviour {
    public TextMeshProUGUI soundTitle;
    public TextMeshProUGUI muteText;
    public TextMeshProUGUI audioOnText;

    void Start () {
        if (PlayerPrefs.GetString("Mute").Equals("True"))
            muteAudio();
        else
            audioOn();
    }

    public void muteAudio()
    {
        PlayerPrefs.SetString("Mute", "True");
        AudioListener.pause = true;
        soundTitle.text = "SOUND: OFF";
        muteText.color = Color.black;
        audioOnText.color = Color.grey;
    }

    public void audioOn()
    {
        PlayerPrefs.SetString("Mute", "False");
        AudioListener.pause = false;
        soundTitle.text = "SOUND: ON";
        audioOnText.color = Color.black;
        muteText.color = Color.grey;
    }
}
