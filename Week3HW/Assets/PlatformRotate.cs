using UnityEngine;
using System.Collections;

public class PlatformRotate : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    void OnTriggerEnter()
    {
        collider.isTrigger = false;
    }

    // Update is called once per frame
    void Update() {
        if (collider.isTrigger == false)
        {
            transform.Rotate(30 * Time.deltaTime, 0, 0);
        }
    }
}