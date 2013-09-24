using UnityEngine;
using System.Collections;

public class RISING : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > 27)
        {
            transform.position += new Vector3(0, 5* Time.deltaTime,0);
        }
	}
}
