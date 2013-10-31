using UnityEngine;
using System.Collections;

public class CharAnimation : MonoBehaviour {

    public Rigidbody myrigidbody; //using this to make walking go as fast as the player is walking

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if player is moving or holding down any buttons then play walk
        //if not, crossfade to idle
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            animation["Walk"].speed = myrigidbody.velocity.magnitude;
            //animation.CrossFade("Walk", .5f);
        }
        else
        {

            animation.CrossFade("Idle", .5f);
        }
	}
}
