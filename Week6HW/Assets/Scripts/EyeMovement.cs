using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EyeMovement : MonoBehaviour {
    Vector3 destination = new Vector3 (0,0,-5);
    int rngDest = 0;
    public AudioClip ReallyBadResp;
    int destnum;

	// Use this for initialization
	void Start () 
    {
        InvokeRepeating( "SetNewDestination", 8.5f, 7f);
        Invoke("TooLong", 60f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void FixedUpdate()
    {
        transform.position = (destination - transform.position * Time.deltaTime);
    }
    
    void SetNewDestination()
    {
        rngDest = Random.Range(0, 34);
        if (rngDest < 5)
        {
            destination = GameObject.FindGameObjectWithTag("1").transform.position;
        }
        else if (rngDest > 4 && rngDest < 10)
        {
            destination = GameObject.FindGameObjectWithTag("2").transform.position;
        }
        else if (rngDest > 9 && rngDest < 15)
        {
            destination = GameObject.FindGameObjectWithTag("3").transform.position;
        }
        else if (rngDest > 14 && rngDest < 20)
        {
            destination = GameObject.FindGameObjectWithTag("4").transform.position;
        }
        else if (rngDest > 19 && rngDest < 25)
        {
            destination = GameObject.FindGameObjectWithTag("5").transform.position;
        }
        else if (rngDest > 24 && rngDest < 30)
        {
            destination = GameObject.FindGameObjectWithTag("6").transform.position;
        }
        else if (rngDest > 29 && rngDest < 35)
        {
            destination = GameObject.FindGameObjectWithTag("7").transform.position;
        }

    }

    void TooLong()
    {
        audio.Play();
        Invoke("SwitchAudio", 8.5f);
    }

    void SwitchAudio()
    {
        audio.clip = ReallyBadResp;
        audio.Play();
        Invoke("EndGame", 2.5f);
    }

    void EndGame()
    {
        Application.Quit();
    }
}
