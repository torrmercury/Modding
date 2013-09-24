using UnityEngine;
using System.Collections;

public class Shoveball : MonoBehaviour {
    // Use this for initialization
    void Start()
    {
    }

    void OnCollisionStay()
    {
        transform.position += new Vector3(5f, 0, 0) * Time.deltaTime;
    }

	// Update is called once per frame
	void Update () {
	}
}
