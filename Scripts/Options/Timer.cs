using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    public TextMeshProUGUI timerTitle;
    public Image timerSUPERSLOWImage;
    public Image timerSLOWImage;
    public Image timerFASTImage;
    public List<Sprite> spriteList;

    void Start()
    {
        if (PlayerPrefs.GetFloat("Timer") == 0.1f)
            timerSuperSlow();
        else if (PlayerPrefs.GetFloat("Timer") == .050f)
            timerSLOW();
        else 
            timerFAST();
    }

    public void timerSuperSlow()
    {
        PlayerPrefs.SetFloat("Timer", 0.1f);
        PlayerPrefs.SetString("TimerText", "SUPER SLOW");
        timerTitle.text = "TIMER: SUPER SLOW";
        timerSUPERSLOWImage.sprite = spriteList[0];
        timerSLOWImage.sprite = spriteList[3];
        timerFASTImage.sprite = spriteList[5];
    }

    public void timerSLOW()
    {
        PlayerPrefs.SetFloat("Timer", .050f);
        PlayerPrefs.SetString("TimerText", "SLOW");
        timerTitle.text = "TIMER: SLOW";
        timerSUPERSLOWImage.sprite = spriteList[1];
        timerSLOWImage.sprite = spriteList[2];
        timerFASTImage.sprite = spriteList[5];
    }

    public void timerFAST()
    {
        PlayerPrefs.SetFloat("Timer", .025f);
        PlayerPrefs.SetString("TimerText", "FAST");
        timerTitle.text = "TIMER: FAST";
        timerSUPERSLOWImage.sprite = spriteList[1];
        timerSLOWImage.sprite = spriteList[3];
        timerFASTImage.sprite = spriteList[4];
    }
}
