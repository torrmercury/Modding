using UnityEngine;
using System.Collections;

public class numguess : MonoBehaviour {
    int guess = 0; //player's guess
    int secretnum = 0; //num player needs to guess
    // Use this for initialization
	void Start () {
        //gen the rng number
        secretnum = Random.Range(0, 5);
	}
	    
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            guess--;
            guiText.text = guess.ToString();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            guess++;
            guiText.text = guess.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (guess<secretnum)
            {
                guiText.text = "TOO LOW FOOL!";
            }
            else if (guess > secretnum)
            {   
                guiText.text="TOO HIGH FOOL!";
            }
            else
            {
                guiText.text="YA WIN FOO!";
            }
        }
	}
}