using UnityEngine;
using System.Collections;

public class 
    ClickToMove : MonoBehaviour {
    float stopPos = 0;
    public Vector3 destination;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Vector3.Distance(transform.position, destination) > stopPos)
        {
            rigidbody.AddForce(Vector3.Normalize(destination - transform.position) * 10f);
        }
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit = new RaycastHit();

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(mouseRay, out rayHit, 10000f))
            {
                destination = rayHit.point;
            }
        }
	}
}
