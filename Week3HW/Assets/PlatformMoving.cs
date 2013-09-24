using UnityEngine;
using System.Collections;

public class PlatformMoving : MonoBehaviour {
    bool platformhit = false;
	// Use this for initialization
	void Start () {
	
	}

    void OnCollisionEnter()
    {
        platformhit = true;
    }

	// Update is called once per frame
	void Update () 
    {
	    if (platformhit == true && transform.position.x > -18 && transform.position.z > 0)
        {
            transform.position += new Vector3(-5, 0, -1)*Time.deltaTime;
        }
	}
}