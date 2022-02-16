using TMPro;
using UnityEngine;

public class User : MonoBehaviour {

    public TextMeshProUGUI welcome;
    public TextMeshProUGUI nameInput;

    void Start()
    {
        if (PlayerPrefs.GetString("username").Equals(""))
        {
            welcome.text = "Welcome User!";
        }
        else
        {
            welcome.text = "Welcome " + PlayerPrefs.GetString("username") + "!";
        }
    }

    public void setName()
    {
        PlayerPrefs.SetString("username", nameInput.text);
        welcome.text = "Welcome " + PlayerPrefs.GetString("username") + "!";
    }
}
