using UnityEngine;
using System.Collections;

public class DancingMan : MonoBehaviour
{

    bool start = false;
    float timetrack = 0;
    bool looper = false;
    bool hundredtick = false;

    // Use this for initialization
    void Start()
    {
        start = true;
        timetrack = 0;
        looper = false;
        hundredtick = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, 20f * Time.deltaTime, 0f);
        if (start == true)
        {
            timetrack += 1 * Time.deltaTime;
            if (timetrack > 10)
            {
                start = false;
                timetrack = 0;
            }
            transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime;
        }
        else if (start == false && looper == false && hundredtick == false)
        {
            transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime;
            timetrack += 1 * (Time.deltaTime / 10);
            if (timetrack > 1)
            {
                looper = true;
                timetrack = 0;
            }
        }
        else if (looper == true && hundredtick == false)
        {
            transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime;
            timetrack += 1 * (Time.deltaTime / 10);
            if (timetrack > 1)
            {
                looper = false;
                timetrack = 0;
            }
        }
        else if (Time.time > 100)
        {
            hundredtick = true;
        }

        //after 100 seconds have passed, switch to dividing by 100    
        else if (hundredtick == true)
        {
            if (looper == false)
            {
                transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime;
                timetrack += 1 * (Time.deltaTime / 100);
                if (timetrack > 10)
                {
                    looper = true;
                    timetrack = 0;
                }
            }
            else if (looper == true)
            {
                transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime;
                timetrack += 1 * (Time.deltaTime / 100);
                if (timetrack > 10)
                {
                    looper = false;
                    timetrack = 0;
                }
            }
        }
    }
}