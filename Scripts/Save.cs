using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Save : MonoBehaviour {
    public TextMeshProUGUI welcome;

    public void saveName()
    {
        welcome.text = "Welcome, " + PlayerPrefs.GetString("Name") + "!";
    }

    private void Start()
    {
        if(PlayerPrefs.GetString("Name") != null)
        {
            welcome.text = "Welcome, " + PlayerPrefs.GetString("Name") + "!";
        } else
        {
            welcome.text = "Welcome, guest! Set your name in the options.";
        }
    }
}
