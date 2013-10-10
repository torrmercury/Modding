using UnityEngine;
using System.Collections;

public class Response : MonoBehaviour {
    public AudioClip Respond;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter()
    {
        audio.clip = Respond;
        audio.PlayDelayed(5.5f);
    }
}
