using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fishgod : MonoBehaviour {
    public fishscript fishBlueprint;
    public fishscript fishBlueprint2;
    public int fishCount = 75;
    public List<fishscript> fishlist = new List<fishscript>();
    int randomNum = 0;
	// Use this for initialization

	void Start () {
        int currentfishcounter = 0;
        while (currentfishcounter < fishCount)
        {
            randomNum = Random.Range(0, 100);
            if (randomNum < 50)
            {
                Vector3 fishPosition = Random.insideUnitSphere * 20f;
                fishscript newFish = Instantiate(fishBlueprint, fishPosition, Quaternion.identity) as fishscript;
                fishlist.Add(newFish);
                currentfishcounter++;
            }
            else
            {
                Vector3 fishPosition = Random.insideUnitSphere * 20f;
                fishscript newFish = Instantiate(fishBlueprint2, fishPosition, Quaternion.identity) as fishscript;
                fishlist.Add(newFish);
                currentfishcounter++;
            }
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
