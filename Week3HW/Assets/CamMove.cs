using UnityEngine;
using System.Collections;

public class CamMove : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time < 10)
        {
            transform.position = new Vector3(9.8f, 6f, -9.7f);
            transform.eulerAngles = new Vector3(30, 320, 0);
        }
        else if (Time.time > 10 && Time.time < 16)
        {
            transform.position = new Vector3(19.5f, 2.8f, 21.3f);
            transform.eulerAngles = new Vector3(30, 180, 0);
        }
        else if (Time.time > 16 && Time.time < 25)
        {
            transform.position = new Vector3(1.3f, 3.15f, -17.6f);
            transform.eulerAngles = new Vector3(30, 0, 0);
        }
        else if (Time.time > 25 && Time.time < 26)
        {
            transform.position = new Vector3(15f, -23f, -15f);
        }
        else if (Time.time > 26 && Time.time < 35)
        {
            transform.Rotate(-10 * Time.deltaTime, 0, 0);
        }
    }
}