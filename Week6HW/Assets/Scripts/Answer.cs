using UnityEngine;
using System.Collections;

public class Answer : MonoBehaviour {
    public AudioClip Response;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseUpAsButton()
    {
        audio.Play();
        Invoke("SwitchAudio", 3.5f);
    }

    void SwitchAudio()
    {
        audio.clip = Response;
        audio.Play();
        Invoke("EndGame", 2f);
    }

    void EndGame()
    {
        Application.Quit();
    }
}
