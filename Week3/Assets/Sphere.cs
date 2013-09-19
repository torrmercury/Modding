using UnityEngine;
using System.Collections;

public class Sphere : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    rigidbody.AddForce (500f, -500f, 500f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
