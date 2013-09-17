using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    bool jump = false;
    float i = 0;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > 0f && Time.time <20f)
        {

            

            if (jump == false)
            {
                for (float i = 0; i <= 5; i += 1 * Time.deltaTime)
                {
                    transform.position += new Vector3(0f, 3f * Time.deltaTime, 0f);
                } jump = true;

            }
            else if (jump == true)
            {
                for (float i = 0; i <= 5; i += 1 * Time.deltaTime)
                {
                    transform.position += new Vector3(0f, -3f * Time.deltaTime, 0f);
                } jump = false;
            }
        }
    }
}