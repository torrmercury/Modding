using UnityEngine;
using System.Collections;

public class RotateDisk : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > 16)
        {
            transform.Rotate (0, 0, 30* Time.deltaTime);
        }
	}
}
