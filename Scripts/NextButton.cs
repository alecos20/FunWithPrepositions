using TMPro;
using UnityEngine;

public class NextButton : MonoBehaviour {
    public GameObject nextQuestionBtn;
    public TextMeshProUGUI descriptionTextBox;

    public void next()
    {
        nextQuestionBtn.SetActive(true);
    }

    public void setAnswer()
    {
        descriptionTextBox.text = PlayerPrefs.GetString("Description");
    }
}
