using UnityEngine;
using System.Collections;

public class DestroyBall : MonoBehaviour {
    float x = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

    void Update()
    {
        OnCollisionEnter();
    }

	void OnCollisionEnter() {
        if (x < 11)
        {
            x += 1*Time.deltaTime;
        }
        else if (x > 11)
        {
            Destroy(gameObject);
        }
	}
}
