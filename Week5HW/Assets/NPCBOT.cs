using UnityEngine;
using System.Collections;

public class 
    NPCBOT : MonoBehaviour {
    int randomnumber = 0;

	// Use this for initialization
    void Start()
    {

    }
	
	// Update is called once per frame
	void Update () 
    {
        if (Physics.Raycast(transform.position, transform.forward))
        {
            randomnumber = Random.Range(0, 10);
            if (randomnumber < 5)
            {
                transform.eulerAngles = new Vector3(0, 90, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, -90, 0);
            }
        }
	}

    void FixedUpdate()
    {
        rigidbody.AddForce(transform.forward);
        //could also use "rigidbody.AddRelativeForce(Vector3.forward);"
        

    }
}