using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NavBar : MonoBehaviour {
    public TextMeshProUGUI playPauseText;
    public Sprite greenBar;
    public Sprite blackBar;
    public List<GameObject> panelList;
    public List<GameObject> bannerList;
    public List<Button> btnList;
    public List<TextMeshProUGUI> textMeshList;
    Color32 gray = new Color32(202, 202, 202, 255);

    private void Start()
    {
        QuestionPacks temp = new QuestionPacks();
        string[] fileArray = temp.FileNames();
        if (fileArray.Length == 0)
        {
            viewHelp();
        }
    }

    public void viewPlay()
    {
        playPause();

        foreach (GameObject panel in panelList)
        {
            panel.SetActive(false);
        }

        foreach (GameObject panel in bannerList)
        {
            panel.SetActive(false);
        }

        foreach (Button btn in btnList)
        {
            btn.GetComponent<Image>().sprite = blackBar;
        }

        foreach (TextMeshProUGUI text in textMeshList)
        {
            text.color = gray;
        }

        textMeshList[0].color = Color.black;
        btnList[0].GetComponent<Image>().sprite = greenBar;
        panelList[0].SetActive(true);
        bannerList[0].SetActive(true);
        if (Time.timeScale == 0)
        {
            panelList[5].SetActive(true);
        }
    }

    public void viewStore()
    {
        foreach (GameObject panel in panelList)
        {
            panel.SetActive(false);
        }

        foreach (GameObject panel in bannerList)
        {
            panel.SetActive(false);
        }

        foreach (Button btn in btnList)
        {
            btn.GetComponent<Image>().sprite = blackBar;
        }

        foreach (TextMeshProUGUI text in textMeshList)
        {
            text.color = gray;
        }

        textMeshList[1].color = Color.black;

        btnList[1].GetComponent<Image>().sprite = greenBar;

        playPauseText.text = "\U0000f04b";

        Time.timeScale = 0;
        panelList[1].SetActive(true);
        bannerList[1].SetActive(true);
    }

    public void viewAccount()
    {
        foreach (GameObject panel in panelList)
        {
            panel.SetActive(false);
        }

        foreach (GameObject panel in bannerList)
        {
            panel.SetActive(false);
        }

        foreach (Button btn in btnList)
        {
            btn.GetComponent<Image>().sprite = blackBar;
        }

        foreach (TextMeshProUGUI text in textMeshList)
        {
            text.color = gray;
        }

        textMeshList[2].color = Color.black;
        btnList[2].GetComponent<Image>().sprite = greenBar;

        playPauseText.text = "\U0000f04b";

        Time.timeScale = 0;
        panelList[2].SetActive(true);
        bannerList[2].SetActive(true);
    }

    public void viewSettings()
    {
        foreach (GameObject panel in panelList)
        {
            panel.SetActive(false);
        }

        foreach (GameObject panel in bannerList)
        {
            panel.SetActive(false);
        }

        foreach (Button btn in btnList)
        {
            btn.GetComponent<Image>().sprite = blackBar;
        }

        foreach (TextMeshProUGUI text in textMeshList)
        {
            text.color = gray;
        }

        textMeshList[3].color = Color.black;

        btnList[3].GetComponent<Image>().sprite = greenBar;

        playPauseText.text = "\U0000f04b";

        Time.timeScale = 0;
        panelList[3].SetActive(true);
        bannerList[3].SetActive(true);
    }

    public void viewHelp()
    { 
        foreach (GameObject panel in panelList)
        {
            panel.SetActive(false);
        }

        foreach (GameObject panel in bannerList)
        {
            panel.SetActive(false);
        }

        foreach (Button btn in btnList)
        {
            btn.GetComponent<Image>().sprite = blackBar;
        }

        foreach (TextMeshProUGUI text in textMeshList)
        {
            text.color = gray;
        }

        textMeshList[4].color = Color.black;

        btnList[4].GetComponent<Image>().sprite = greenBar;

        playPauseText.text = "\U0000f04b";

        Time.timeScale = 0;
        panelList[4].SetActive(true);
        bannerList[4].SetActive(true);
    }

    void playPause()
    {
        if(Time.timeScale == 1)
        {
            playPauseText.text = "\U0000f04b";
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
            playPauseText.text = "\U0000f04c";
        }   
    }
}
