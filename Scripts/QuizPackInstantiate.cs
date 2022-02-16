using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Displays all the quiz packs unlocked by the player
public class QuizPackInstantiate : MonoBehaviour {
    //The quiz pack prefab
    public GameObject quizPack;
    public GameObject gridThatStoresTheItems;
    QuestionPacks qp;
    List<GameObject> quizPackList;
    List<QuestionPacks> quizPackDataList;
    public List<Sprite> timerSpeed;
    public GameObject loadingPanel;
    public Slider loadSlider;
    public TextMeshProUGUI loadText;

    void Start () {
        instantiateQPs();
    }

    void createList()
    {
        qp = new QuestionPacks();
        quizPackList = new List<GameObject>();
        quizPackDataList = new List<QuestionPacks>(qp.LoadAll());
        PlayerPrefs.SetInt("QPs", quizPackDataList.Capacity);
        for (int i = 0; i < quizPackDataList.Capacity; i++)
        {
            GameObject temp = Instantiate(quizPack) as GameObject;
            temp.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Quiz Pack #" + (i + 1);
            
            temp.transform.Find("HS_Accuracy%").GetComponent<TextMeshProUGUI>().text = quizPackDataList[i].getAccuracy() + "%";
            temp.transform.Find("LG_Accuracy%").GetComponent<TextMeshProUGUI>().text = quizPackDataList[i].getLastAccuracy() + "%";
            temp.transform.Find("HS_TimerScore").GetComponent<TextMeshProUGUI>().text = quizPackDataList[i].getScore() + "";
            temp.transform.Find("LG_TimerScore").GetComponent<TextMeshProUGUI>().text = quizPackDataList[i].getLastScore() + "";

            //Sets the highscore accuracy
            if (quizPackDataList[i].getAccuracy() == 0)
            {
                temp.transform.Find("HS_Accuracy%").GetComponent<TextMeshProUGUI>().text = "NA";    
            }

            //Sets the last game accuracy
            if (quizPackDataList[i].getLastAccuracy() == 0)
            {
                temp.transform.Find("LG_Accuracy%").GetComponent<TextMeshProUGUI>().text = "NA";
            }

            //Sets the highscore 
            if (quizPackDataList[i].getScore() == 0)
            {
                temp.transform.Find("HS_TimerScore").GetComponent<TextMeshProUGUI>().text = "NA";
            }

            //Sets the last game's score 
            if (quizPackDataList[i].getLastScore() == 0)
            {
                temp.transform.Find("LG_TimerScore").GetComponent<TextMeshProUGUI>().text = "NA";
            }

            //Shows highscore badge
            if (quizPackDataList[i].getScore() == quizPackDataList[i].getLastScore() && quizPackDataList[i].getScore() > 0)
            {
                temp.transform.Find("HScore").GetComponent<Image>().gameObject.SetActive(true);
            }
            else
            {
                temp.transform.Find("HScore").GetComponent<Image>().gameObject.SetActive(false);
            }

            // Indicates which Quiz Pack was most recently played
            if (PlayerPrefs.GetInt("QPnum") - 1 == i)
            {
                temp.transform.Find("most recent").GetComponent<TextMeshProUGUI>().gameObject.SetActive(true);
            } 

            //Sets the user name
            temp.transform.Find("UserName").GetComponent<TextMeshProUGUI>().text = quizPackDataList[i].getUserName() + "";

            //Sets the date last played or when it was unlocked
            temp.transform.Find("Date").GetComponent<TextMeshProUGUI>().text = quizPackDataList[i].getDate();

            temp.transform.Find("PlayBtn").GetComponent<Button>().onClick.AddListener(playGame);
            temp.transform.Find("PlayBtn").name = i + "";

            //Sets the timer speed picture for last game
            if(quizPackDataList[i].getTimerSpeed() == "SUPER SLOW")
                temp.transform.Find("LG_Timer").GetComponent<Image>().sprite = timerSpeed[0];
            else if (quizPackDataList[i].getTimerSpeed() == "SLOW")
                temp.transform.Find("LG_Timer").GetComponent<Image>().sprite = timerSpeed[1];
            else if (quizPackDataList[i].getTimerSpeed() == "FAST")
                temp.transform.Find("LG_Timer").GetComponent<Image>().sprite = timerSpeed[2];

            //Sets the timer speed picture for highscore
            if (quizPackDataList[i].getHSTimerSpeed() == "SUPER SLOW")
                temp.transform.Find("HS_Timer").GetComponent<Image>().sprite = timerSpeed[0];
            else if (quizPackDataList[i].getHSTimerSpeed() == "SLOW")
                temp.transform.Find("HS_Timer").GetComponent<Image>().sprite = timerSpeed[1];
            else if (quizPackDataList[i].getHSTimerSpeed() == "FAST")
                temp.transform.Find("HS_Timer").GetComponent<Image>().sprite = timerSpeed[2];

            quizPackList.Add(temp);
        }
    }
	
    void instantiateQPs()
    {
        createList();
        foreach (GameObject qpInstance in quizPackList)
        {
            qpInstance.transform.localScale = Vector3.one;
            qpInstance.transform.SetParent(gridThatStoresTheItems.transform);
            qpInstance.transform.localScale = Vector3.one;
        }
    }

    public void playGame()
    {
        QuestionPacks temp = new QuestionPacks();
        string[] fileArray = temp.FileNames();
        int index = Int32.Parse(EventSystem.current.currentSelectedGameObject.name);
        PlayerPrefs.SetInt("QPnum", index + 1);
        PlayerPrefs.SetString("ChosenPack", fileArray[index]);
        LoadLevel(1);
    }

    void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    // A coroutine that loads a scene
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            // Set the slider fill percent to "progress"
            loadSlider.value = progress;

            // The percentage text for "progress"
            loadText.text = progress * 100 + "%";

            yield return null;
        }
    }
}
