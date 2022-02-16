using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GetQP : MonoBehaviour
{
    // Initialising the List where words will be stored
    List<string> words;
    // Initialising the List where descriptions will be stored
    List<string> descriptions;

    // Regular expression pattern for removing HTML tags
    const string HTML_TAG_PATTERN = "<.*?>";
    public Button qpBtn;
    public TextMeshProUGUI btnText;
    public TextAsset prepositionary;
    public List<Sprite> freeQP_sprites;
    public List<GameObject> freeQP_GO;
    private ulong lastQP;
    //86400000f
    const float MS_TO_WAIT = 86400000f;
    public GameObject qpReady;
    
    Color32 green = new Color32(8, 198, 0, 255);
    Color32 red = new Color32(167, 20, 0, 255);

    void Start()
    {
        lastQP = ulong.Parse(PlayerPrefs.GetString("LastQP"));
        
        if (!isQPReady())
        {
            qpBtn.interactable = false;
        }
        else
        {
            freeQP_GO[0].GetComponent<Image>().sprite = freeQP_sprites[0];
            freeQP_GO[1].GetComponent<Image>().sprite = freeQP_sprites[1];
            freeQP_GO[2].GetComponent<TextMeshProUGUI>().color = green;
        }
    }

    void Update()
    {
        if (!qpBtn.IsInteractable())
        {
            if (isQPReady())
            {
                qpBtn.interactable = true;
                qpReady.SetActive(true);
                btnText.text = "Get Free QP!";
                freeQP_GO[0].GetComponent<Image>().sprite = freeQP_sprites[0];
                freeQP_GO[1].GetComponent<Image>().sprite = freeQP_sprites[1];
                freeQP_GO[2].GetComponent<TextMeshProUGUI>().color = green;
                return;
            }

            qpReady.SetActive(false);
            freeQP_GO[0].GetComponent<Image>().sprite = freeQP_sprites[2];
            freeQP_GO[1].GetComponent<Image>().sprite = freeQP_sprites[3];
            freeQP_GO[2].GetComponent<TextMeshProUGUI>().color = red;

            ulong differenceInTicks = ((ulong)DateTime.Now.Ticks - lastQP);
            ulong milliseconds = differenceInTicks / TimeSpan.TicksPerMillisecond;
            float secondsLeft = (MS_TO_WAIT - milliseconds) / 1000.0f;

            string timer = "";
            // Hours Left
            timer += ((int)secondsLeft / 3600).ToString() + "h ";
            secondsLeft -= ((int)secondsLeft / 3600) * 3600;

            // Minutes Left
            timer += ((int)secondsLeft / 60).ToString("00") + "m ";

            // Seconds Left
            timer += (secondsLeft % 60).ToString("00") + "s";

            btnText.text = timer;
        }
    }

    private bool isQPReady()
    {
        ulong differenceInTicks = ((ulong)DateTime.Now.Ticks - lastQP);
        ulong milliseconds = differenceInTicks / TimeSpan.TicksPerMillisecond;
        float secondsLeft = (MS_TO_WAIT - milliseconds) / 1000.0f;

        if (secondsLeft < 0 || PlayerPrefs.GetString("QuestionPack").Equals("True") || PlayerPrefs.GetString("Unlimited").Equals("True"))
        {
            PlayerPrefs.SetString("QuestionPack", "False");
            return true;
        }

        return false;
    }

    public void btnPushed()
    {
        Scene scene = SceneManager.GetActiveScene();
        string[] wordDataString = prepositionary.text.Split('}');

        GetWords(wordDataString);
        GetDescriptions(wordDataString);

        createQP();
        qpBtn.interactable = false;
        PlayerPrefs.SetString("LastQP", DateTime.Now.Ticks.ToString());
        lastQP = (ulong)DateTime.Now.Ticks;
        if(scene.name.Equals("StartMenu"))
            SceneManager.LoadSceneAsync(0);
    }

    void GetWords(string[] wordData)
    {
        words = new List<string>();

        // Goes through the wordData array, and adds all words to the words List
        for (int i = 0; i < wordData.Length - 1; i++)
        {
            words.Add(GetWordDataValue(wordData[i]).Trim(new Char[] { '"', ':' }));
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
            string descriptionTemp = Regex.Replace(temp, HTML_TAG_PATTERN, string.Empty);

            descriptions.Add(descriptionTemp.Trim(new Char[] { '"', ':', '\\' }));
        }
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

    void createQP()
    {
        List<int> rand = new List<int>();
        QuestionPacks qp = new QuestionPacks();
        System.Random random = new System.Random();
        string wordIndexTemp = "";
        string descIndexTemp = "";
        int index = 0;

        for (int i = 0; i <= 20; i++)
        {
            do
            {
                index = random.Next(0, words.Count);
            } while (rand.Contains(index));

            //Splits all descriptions for a word
            string tempDescription = descriptions[index];
            string[] tempArray = tempDescription.Split(new string[] { "\\r" }, StringSplitOptions.None);

            // Generates a random index based on the number of descriptions a word has
            int descIndex = 0;
            do
            {
                descIndex = random.Next(0, tempArray.Length); 
                if (tempArray[descIndex].Equals(""))
                    descIndex = random.Next(0, tempArray.Length);
            } while (Regex.IsMatch(tempArray[descIndex], "Note:"));
            rand.Add(index);
            if (i != 20)
            {
                wordIndexTemp += index + ",";
                descIndexTemp += descIndex + ",";
            }
            else
            {
                wordIndexTemp += index;
                descIndexTemp += descIndex;
            }

        }

        qp.setDate(DateTime.Now.ToString("MM-dd-yyyy"));
        qp.setWordIndexes(wordIndexTemp);
        qp.setDescriptionIndexes(descIndexTemp);
        qp.SaveGenerated();
    }

    public void buyQP()
    {
        IAPManager.Instance.Buy20Questions();
    }

    public void firstTime()
    {
        QuestionPacks temp = new QuestionPacks();
        string[] fileArray = temp.FileNames();
        if (fileArray.Length == 0)
        {
            btnPushed();
        }
    }
}
