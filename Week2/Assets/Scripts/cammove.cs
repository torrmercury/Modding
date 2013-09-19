using UnityEngine;
using System.Collections;

public class cammove : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(1f, 1f, 1f) * Time.deltaTime;
        transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime;
    }
}
