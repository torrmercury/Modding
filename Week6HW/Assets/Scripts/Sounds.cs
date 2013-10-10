using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {
    public AudioClip respond;
	// Use this for initialization
	void Start () 
    {

	}
	
	// Update is called once per frame
	void Update () 
    {
    
	}
    void OnTriggerEnter()
    {
        audio.Play();
        Invoke("Respond", 3.5f);
        Destroy(collider);
    }

    void Respond()
    {
        audio.clip = respond;
        audio.Play();
    }
}
