using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fishgod : MonoBehaviour {
    public fishscript fishBlueprint;
    public int fishCount = 100;
    public List<fishscript> fishlist = new List<fishscript>();

	// Use this for initialization
	void Start () {
        int currentfishcounter = 0;
        while (currentfishcounter < fishCount)
        {
            Vector3 fishPosition = Random.insideUnitSphere * 100f;
            fishscript newFish = Instantiate(fishBlueprint, fishPosition, Quaternion.identity) as fishscript;
            fishlist.Add(newFish);
            currentfishcounter++;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (fishscript currentFish in fishlist)
            {
                currentFish.destination = Vector3.zero;
            }
	    }
    }
}
