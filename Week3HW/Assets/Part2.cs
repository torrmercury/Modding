using UnityEngine;
using System.Collections;

public class Part2 : MonoBehaviour {
    bool hascollided = false;
    float x = 0;
    
    // Use this for initialization
	void Start () {
	
	}

    void OnCollisionEnter()
    {
        hascollided = true;
        rigidbody.useGravity = true;
    }

	// Update is called once per frame
	void Update () {
        if (hascollided == true && x < 8)
        {
            x += 1 * Time.deltaTime;
        }
        else if (x > 8)
        {
            Destroy(gameObject);
        }
	}
}
