using UnityEngine;
using System.Collections;

public class animate : MonoBehaviour {
    public float x = 1f;
    public float y = 1f;
    public float z = 1f;

	// Use this for initialization
	void Start () {
        //if 
        //{
            //x = .5f;
            //y = .5f;
            //z = .5f;
        //}
	}
	
	// Update is called once per frame
	void Update () {
        if (Time.time > 2f && Time.time <7f)
        {
            transform.position = transform.position + new Vector3(x, y, z) * Time.deltaTime;
            transform.localScale += new Vector3(x, y, z) * Time.deltaTime;
            transform.Rotate(0f, 0f, 45f * Time.deltaTime);
        }
        if (Time.time < 2f && Time.time > 7f)
        {
            transform.position += new Vector3(x, y, z) * Time.deltaTime;
            transform.localScale += new Vector3(x, y, z) * Time.deltaTime;
            transform.Rotate(0f, 0f, 45f * Time.deltaTime);
        }

        if (Time.time >= 5f && light.enabled == false)
        {
            light.enabled = true;
        }
	}
}