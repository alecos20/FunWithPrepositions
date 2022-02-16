using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animCheck : MonoBehaviour {

    public GameObject descriptionBox;
    public List<Button> navBarList;

    // If an animation is playing, the nav bar is uninteractable 
    void Update () {
        if (descriptionBox.GetComponentInChildren<Animation>().isPlaying)
        {
            foreach (Button btn in navBarList)
            {
                btn.interactable = false;
            }
        }
        else
        {
            foreach(Button btn in navBarList)
            {
                btn.interactable = true;
            }
        }
	}
}
