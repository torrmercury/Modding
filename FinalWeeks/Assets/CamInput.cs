using UnityEngine;
using System.Collections;

public class CamInput : MonoBehaviour {

    public Sheeeeeeep dolly;
    public TextMesh fameMeter;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit = new RaycastHit();

        if (Physics.Raycast(ray, out rayhit, 10000f))
        {
            if (rayhit.collider.tag == "sheep" && Input.GetMouseButtonDown(0))
            {
                Sheeeeeeep selectedSheep = rayhit.collider.GetComponent<Sheeeeeeep>();
                fameMeter.text = "Sheep's fame is " + selectedSheep.fame.ToString();
            }

            if (Input.GetMouseButtonDown(1))
            {
                Sheeeeeeep newSheep = Instantiate(dolly, rayhit.point, Quaternion.identity) as Sheeeeeeep;
                newSheep.fame = Random.Range(1, 100);
            }
        }
        
        
	}
}
