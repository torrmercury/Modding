using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickingAnAnswer : MonoBehaviour {
    public AudioClip ReallyBad;
    public AudioClip KindaBad;
    public AudioClip ReallyBadResp;
    public AudioClip KindaBadResp;
    public AudioClip EasterEgg;
    public AudioClip Okay;
    public AudioClip OkayResp;
    int rngnonans = 0;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        rngnonans = Random.Range(0, 20);
	}

    void OnMouseUpAsButton()
    {
        if (rngnonans < 3)
        {
            audio.clip = ReallyBad;
            audio.Play();
            Invoke("SwitchAudio", 8.5f);
        }
        else if (rngnonans > 2 && rngnonans < 15)
        {
            audio.clip = KindaBad;
            audio.Play();
            Invoke("SwitchAudio2", 2.5f);
        }
        else if (rngnonans > 14 && rngnonans < 20)
        {
            audio.clip = Okay;
            audio.Play();
            Invoke("SwitchAudio3", 2.5f);
        }
        else if (rngnonans == 20)
        {
            audio.clip = EasterEgg;
            audio.Play();
        }
    }
    void SwitchAudio()
    {
        audio.clip = ReallyBadResp;
        audio.Play();
        Invoke("EndGame", 2.5f);
    }
    void SwitchAudio2()
    {
        audio.clip = KindaBadResp;
        audio.Play();
        Invoke("EndGame", 3.5f);
    }
    void SwitchAudio3()
    {
        audio.clip = OkayResp;
        audio.Play();
        Invoke("EndGame", 4f);
    }
    void EndGame()
    {
        Application.Quit();
    }
}
