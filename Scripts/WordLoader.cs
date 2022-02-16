using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class WordLoader : MonoBehaviour {
    
    // Initialising the List where words will be stored
    List<string> words;
    // Initialising the List where descriptions will be stored
    List<string> descriptions;

    bool saved = false;

    private float desiredNumber = 0;
    private float initialNumber = 0;
    private float currentNumber = 0;

    public TextMeshProUGUI qpTitle; 

    [Header("Buttons")]
    // The option buttons, the sprites, and text on the button
    public List<Sprite> btnImages;
    public List<Button> buttonList;
    public List<TextMeshProUGUI> buttonTextList;

    [Space]

    [Header("Description")]
    // The UI element where a description is displayed
    public TextMeshProUGUI descriptionTextBox;

    // Regular expression pattern for removing HTML tags
    const string HTML_TAG_PATTERN = "<.*?>";

    // Sound effect when you answer a question correctly
    public AudioSource correctSound;
    public AudioSource incorrectSound;

    [Space]

    [Header("QuestionStats")]
    //Question Stats: Number of questions (wrong, right, answered)
    public TextMeshProUGUI qsWrong;
    public TextMeshProUGUI qsRight;
    public TextMeshProUGUI qsAnswered;
    

    [Space]

    [Header("Sliders")]
    // Time sliders
    public List<Sprite> sliderImage;
    public List<Slider> pointSliders;
    //The max amount of time for 1 slider
    double multiplierMax = 0;
    //The max amount of time for all sliders
    double maxTime = 0;
    int scoreMultiplierNumber;
    public List<Sprite> cornerColors;
    public List<Image> corners;
    [Space]

    string[] prepositions;
    bool nextQuestion = true;
    string answerPrep = "";
    int answerBtn = 0;
    bool connection;

    [Header("Preposition Text Assets")]
    public TextAsset prepositionText;
    public TextAsset prepositionary;

    [Space]

    [Header("Correct/Incorrect Anims")]
    public GameObject correct;
    public GameObject incorrect;

    [Space]
    public TextMeshProUGUI timeBonus;
    public Image timeImage;
    public List<Sprite> timerSprites;
    public TextMeshProUGUI scoreText;
    public GameObject nextQuestionBtn;
    List<int> randomNums;
    QuestionPacks chosenQP = null;

    // If the player has answered, they can't gain more points for the same question
    bool hasAnswered = false;

    void Start () {
        Time.timeScale = 1;
        chosenQP = setPack();
        qpTitle.text = "Quiz Pack #" + PlayerPrefs.GetInt("QPnum");
        PlayerPrefs.SetInt("NumAnswered", 0);
        PlayerPrefs.SetFloat("Score", 0f);
        PlayerPrefs.SetInt("Incorrect", 0);
        PlayerPrefs.SetInt("Correct", 0);

        // Initialising the string array that stores each word seperately
        string[] wordDataString;

        // The text that shows the amount of questions the player has answered
        qsAnswered.text = (PlayerPrefs.GetInt("NumAnswered") + 1) + "/20";
        hasAnswered = false;

        //Checks if the player has an internet connection
        StartCoroutine(checkInternetConnection());
        
        if (connection)
        {
            // The request to the FTP server for the .JSON file containing all words and descriptions
            FtpWebRequest ftpRequest = (FtpWebRequest)WebRequest.Create("ftp://ps20961.dreamhost.com/online.prepositionary.com/app.json");
            ftpRequest.Credentials = new NetworkCredential("privatecyber", "z470747074707bStudio2014");
            ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

            // Reads the .JSON file, and splits each word into strings, and
            // stores them into the wordDataString array
            using (Stream sw = ftpRequest.GetResponse().GetResponseStream())
            {
                StreamReader sr = new StreamReader(sw);
                wordDataString = sr.ReadToEnd().Split('}');
            }
        }
        else
        {
            // If there is no internet connection, then the text file already on the device is used
            wordDataString = prepositionary.text.Split('}');
        }

        prepositions = prepositionText.text.Split(',');
        GetWords(wordDataString);
        GetDescriptions(wordDataString);
    }

    // Check if the user is connected to the internet
    IEnumerator checkInternetConnection()
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            connection = false;
        } else {
            connection = true;
        }
    }

    void Update()
    {
        // Sets the image for timer speed according to the speed selected by the player
        if (PlayerPrefs.GetString("TimerText") == "SUPER SLOW")
            timeImage.sprite = timerSprites[0];
        else if (PlayerPrefs.GetString("TimerText") == "SLOW")
            timeImage.sprite = timerSprites[1];
        else
            timeImage.sprite = timerSprites[2];

        System.Random random = new System.Random();
        updateQuestionStats();
        
        if (hasAnswered == false)
            slidersUpdate();

        if (currentNumber != desiredNumber)
        {
            if (initialNumber < desiredNumber)
            {
                currentNumber += (1.5f * Time.deltaTime) * (desiredNumber - initialNumber);
                if (currentNumber >= desiredNumber)
                    currentNumber = desiredNumber;
            }
            scoreText.text = currentNumber.ToString("0");
        }

        if (nextQuestion == true && PlayerPrefs.GetInt("NumAnswered") < 20 && !descriptionTextBox.GetComponentInChildren<Animation>().isPlaying)
        {
            hasAnswered = false;
            DisplayRandomWord();
            normalSliders();
            answerBtn = random.Next(0, 4);

            for (int i = 0; i < 4; i++)
            {
                int prepIndex = random.Next(0, prepositions.Length);

                if (i != answerBtn)
                {
                    do
                    {
                        prepIndex = random.Next(0, prepositions.Length);
                    } while (randomNums.Contains(prepIndex));

                    randomNums.Add(prepIndex);
                    buttonTextList[i].text = prepositions[prepIndex];
                }
                else if (i == answerBtn)
                {
                    buttonTextList[i].text = answerPrep;
                }
            }

            randomNums = null;
            nextQuestion = false;
        } else if (PlayerPrefs.GetInt("NumAnswered") == 20 && nextQuestion == true)
        {
            if (chosenQP.accuracy <= (((float)PlayerPrefs.GetInt("Correct")) / 20 * 100) && chosenQP.score < PlayerPrefs.GetFloat("Score") && saved == false)
            {
                chosenQP.score = PlayerPrefs.GetFloat("Score");
                chosenQP.accuracy = (((float)PlayerPrefs.GetInt("Correct")) / 20 * 100);
                chosenQP.hsTimerSpeed = PlayerPrefs.GetString("TimerText");
                chosenQP.timerSpeed = PlayerPrefs.GetString("TimerText");
                chosenQP.userName = PlayerPrefs.GetString("username");
                chosenQP.lastAcc = chosenQP.accuracy;
                chosenQP.lastScore = chosenQP.score;
                chosenQP.Save();
                saved = true;
            }
            else if (saved == false)
            {
                chosenQP.lastAcc = (((float)PlayerPrefs.GetInt("Correct")) / 20 * 100);
                chosenQP.lastScore = PlayerPrefs.GetFloat("Score");
                chosenQP.timerSpeed = PlayerPrefs.GetString("TimerText");
                chosenQP.userName = PlayerPrefs.GetString("username");
                chosenQP.date = DateTime.Now.ToString("MM-dd-yyyy");
                chosenQP.Save();
                saved = true;
            }

            SceneManager.LoadScene(0);
        }
    }

    public void buttonChoice(Button btn)
    {
        if (hasAnswered == false)
        {
            string btnText = btn.GetComponentInChildren<TextMeshProUGUI>().text;

            if (btnText == answerPrep)
            {
                correctSound.Play();
                correct.SetActive(true);
                correct.GetComponent<Animation>().Play();
                
                PlayerPrefs.SetInt("Correct", PlayerPrefs.GetInt("Correct") + 1);
                greenSliders();
                if(scoreMultiplierNumber > 0)
                {
                    float score = (2000 * (scoreMultiplierNumber - 1)) + (2000 * (pointSliders[scoreMultiplierNumber - 1].value) / (pointSliders[scoreMultiplierNumber - 1].maxValue));
                    timeBonus.text = "Time Bonus +" + Mathf.Floor(score);
                    PlayerPrefs.SetFloat("Score", PlayerPrefs.GetFloat("Score") + Mathf.Floor(score));
                    initialNumber = currentNumber;
                    desiredNumber += Mathf.Floor(score);
                }
            }
            else 
            {
                incorrectSound.Play();
                incorrect.SetActive(true);
                incorrect.GetComponent<Animation>().Play();
                PlayerPrefs.SetInt("Incorrect", PlayerPrefs.GetInt("Incorrect") + 1);
                Handheld.Vibrate();
                redSliders();
                btn.image.sprite = btnImages[1];
            }
            PlayerPrefs.SetInt("NumAnswered", PlayerPrefs.GetInt("NumAnswered") + 1);
            hasAnswered = true;
            buttonList[answerBtn].image.sprite = btnImages[0]; 
            descriptionTextBox.GetComponentInChildren<Animation>().Play();
        }
    }

    // Update the UI with the new question stats
    void updateQuestionStats()
    { 
        qsWrong.text = "" + PlayerPrefs.GetInt("Incorrect");
        qsRight.text = "" + PlayerPrefs.GetInt("Correct");
    }

    // Resets all button colors
    void resetButtonColor()
    {
        foreach(Button btn in buttonList)
        {
            btn.image.sprite = btnImages[2]; 
        }
    }

    public void nextQ()
    {
        timeBonus.text = "";
        qsAnswered.text = (PlayerPrefs.GetInt("NumAnswered") + 1) + "/20";
        correct.SetActive(false);
        incorrect.SetActive(false);
        cornerColorReset();
        resetButtonColor();
        nextQuestion = true;
        nextQuestionBtn.SetActive(false);
    }

    // Gets the word only
    string GetWordDataValue(string data)
    {
        string index = "subject";
        string value = data.Substring(data.IndexOf(index) + index.Length);
        value = value.Remove(value.IndexOf(','));
        return value;
    }

    // Gets the description only
    string GetDescriptionDataValue(string data)
    {
        string index = "message";
        string value = data.Substring(data.IndexOf(index) + index.Length);
        return value;
    }

    // Adds all words to the words List
    void GetWords(string[] wordData)
    {
        words = new List<string>();

        // Goes through the wordData array, and adds all words to the words List
        for (int i = 0; i < wordData.Length - 1; i++)
        {
            words.Add(GetWordDataValue(wordData[i]).Trim(new Char[] { '"', ':'}));
        }
    }

    // Adds all descriptions to the descriptions List
    void GetDescriptions(string[] wordData)
    {
        descriptions = new List<string>();
        for (int i = 0; i < wordData.Length - 1; i++)
        {
            // Goes through the wordData array, and adds all descriptions to the descriptions List
            string temp = GetDescriptionDataValue(wordData[i]);
            string descriptionTemp = Regex.Replace (temp, HTML_TAG_PATTERN, string.Empty);
            
            descriptions.Add(descriptionTemp.Trim(new Char[] {'"', ':', '\\'}));
        }
    }

    // Generates a random word and displays it
    void DisplayRandomWord()
    {
        int wordIndex;
        int descIndex;

        if (chosenQP == null)
        {
            wordIndex = getWordIndex();
            descIndex = getDescIndex();
        } else
        {
            wordIndex = getChosenWordIndex(chosenQP);
            descIndex = getChosenDescIndex(chosenQP);
        }

        string tempDescription = descriptions[wordIndex];
        string[] tempArray = tempDescription.Split(new string[] { "\\r" }, StringSplitOptions.None);

        // Replaces slashes and quotation marks in a description
        tempDescription = tempArray[descIndex].Replace(@"\", string.Empty);
        string tempDesc = tempDescription.Replace("\"", string.Empty);

        // Removes preposition and displays description without it
        answerPrep = removePreposition(words[wordIndex], tempDesc);
    }

    string removePreposition(string word, string description)
    {
        string prepositionInDescription = "";
        randomNums = new List<int>();
        for (int i = 0; i < prepositions.Length; i++)
        {
            prepositionInDescription = Regex.Replace(description, "\\b" + prepositions[i] + "\\b", "______");
            PlayerPrefs.SetString("Description", Regex.Replace(description, "\\b" + prepositions[i] + "\\b", "<#10D007><size=125%><u>" + prepositions[i] + "</u></size></color>"));
            if (Regex.IsMatch(description, "\\b" + prepositions[i] + "\\b"))
            {
                descriptionTextBox.text = prepositionInDescription;

                randomNums.Add(i);
                multiplierMax = Math.Round(description.Length * PlayerPrefs.GetFloat("Timer"));
                maxTime = (multiplierMax * 5);

                normalSliders();
                return prepositions[i];
            }
        }

        descriptionTextBox.text = description;
        nextQuestion = true;
        return null;
    }

    int getWordIndex()
    {
        QuestionPacks qp = new QuestionPacks();
        qp = qp.Load();
        if (qp != null)
        {
            string[] tempIndex = qp.getWordIndexes().Split(',');
            return Int32.Parse(tempIndex[PlayerPrefs.GetInt("NumAnswered")]); ;
        } 

        return 0;
    }

    int getDescIndex()
    {
        QuestionPacks qp = new QuestionPacks();
        qp = qp.Load();
        if (qp != null)
        {
            string[] tempIndex = qp.getDescriptionIndexes().Split(',');
            
            return Int32.Parse(tempIndex[PlayerPrefs.GetInt("NumAnswered")]);
        } 

        return 0;
    }

    int getChosenWordIndex(QuestionPacks qp)
    {
        if (qp != null)
        {
            string[] tempIndex = qp.getWordIndexes().Split(',');
            return Int32.Parse(tempIndex[PlayerPrefs.GetInt("NumAnswered")]); ;
        }

        return 0;
    }

    int getChosenDescIndex(QuestionPacks qp)
    {
        if (qp != null)
        {
            string[] tempIndex = qp.getDescriptionIndexes().Split(',');

            return Int32.Parse(tempIndex[PlayerPrefs.GetInt("NumAnswered")]);
        }

        return 0;
    }

    QuestionPacks setPack()
    {
        if (!PlayerPrefs.GetString("ChosenPack").Equals("None"))
        {
            return SaveLoadManager.loadChosenQP(PlayerPrefs.GetString("ChosenPack"));
        } else
        {
            return null;
        }
    }

    void slidersUpdate()
    {
        maxTime -= Time.deltaTime;
        if (pointSliders[4].value > 0)
        {
            scoreMultiplierNumber = 5;
            pointSliders[4].value -= Time.deltaTime * 2;
        }
        else if (pointSliders[3].value > 0 && pointSliders[4].value <= 0)
        {
            scoreMultiplierNumber = 4;
            pointSliders[3].value -= Time.deltaTime;
            corners[0].color = new Color(191f / 255f, 211f / 255f, 211f / 255f);
        }
        else if (pointSliders[2].value > 0 && pointSliders[3].value <= 0)
        {
            scoreMultiplierNumber = 3;
            pointSliders[2].value -= Time.deltaTime;
            corners[1].color = new Color(191f / 255f, 211f / 255f, 211f / 255f);
        }
        else if (pointSliders[1].value > 0 && pointSliders[2].value <= 0)
        {
            scoreMultiplierNumber = 2;
            pointSliders[1].value -= Time.deltaTime;
            corners[2].color = new Color(191f / 255f, 211f / 255f, 211f / 255f);
        }
        else if (pointSliders[0].value > 0 && pointSliders[1].value <= 0)
        {
            scoreMultiplierNumber = 1;
            pointSliders[0].value -= Time.deltaTime;
            corners[3].color = new Color(191f / 255f, 211f / 255f, 211f / 255f);
        }
        else
        {
            scoreMultiplierNumber = 0;
        }
    }

    void normalSliders()
    {
        foreach (Slider pSlider in pointSliders)
        {
            pSlider.maxValue = Convert.ToInt32(multiplierMax);
            pSlider.value = Convert.ToInt32(multiplierMax);
            Image[] sliderImages = pSlider.GetComponentsInChildren<Image>();
            sliderImages[0].sprite = sliderImage[2];
            sliderImages[0].color = new Color(191f / 255f, 211f / 255f, 211f / 255f);
            sliderImages[1].sprite = sliderImage[2];
        }

        foreach(Image img in corners)
        {
            img.sprite = cornerColors[2];
        }
    }

    void greenSliders()
    {
        foreach (Slider pSlider in pointSliders)
        {
            Image[] sliderImages = pSlider.GetComponentsInChildren<Image>();
            sliderImages[0].sprite = sliderImage[0];
            sliderImages[0].color = new Color(113f / 255f, 209f / 255f, 126f / 255f);
            sliderImages[1].sprite = sliderImage[0];
        }
        foreach (Image img in corners)
        {
            img.sprite = cornerColors[0];
        }
    }

    void redSliders()
    {
        foreach (Slider pSlider in pointSliders)
        {
            Image[] sliderImages = pSlider.GetComponentsInChildren<Image>();
            sliderImages[0].sprite = sliderImage[1];
            sliderImages[0].color = new Color(191f / 255f, 211f / 255f, 211f / 255f);
            sliderImages[1].sprite = sliderImage[1];
        }

        foreach (Image img in corners)
        {
            img.sprite = cornerColors[1];
        }
    }

    void cornerColorReset()
    {
        corners[0].color = Color.white;
        corners[1].color = Color.white;
        corners[2].color = Color.white;
        corners[3].color = Color.white;
    }

    public void returnToStartMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
