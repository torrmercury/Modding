using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
	    if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(10f,0f,0f) * Time.deltaTime;
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(0f, 0f, 10f) * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(-10f, 0f, 0f) * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(0f, 0f, -10f) * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(0f, 0f, 10f) * Time.deltaTime;
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(10f, 0f, 0f) * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(-10f, 0f, 0f) * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(0f, 0f, -10f) * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(-10f, 0f, 0f) * Time.deltaTime;
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(10f, 0f, 0f) * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(0f, 0f, 10f) * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(0f, 0f, -10f) * Time.deltaTime;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(0f, 0f, -10f) * Time.deltaTime;
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(10f, 0f, 0f) * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(0f, 0f, 10f) * Time.deltaTime;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(-10f, 0f, 0f) * Time.deltaTime;
            }
        }
    }
}
