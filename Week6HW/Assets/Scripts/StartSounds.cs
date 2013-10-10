using UnityEngine;
using System.Collections;

public class StartSounds : MonoBehaviour {
    public AudioClip Response;

	// Use this for initialization
	void Start () {
        audio.Play();
        Invoke("SwapAudio", 8.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void SwapAudio()
    {
        audio.clip = Response;
        audio.Play();
    }
}
