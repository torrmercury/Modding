using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CorrectAnswer : MonoBehaviour {
    public List<PickingAnAnswer> picks = new List<PickingAnAnswer>();
    int rnganswer = 0;
    int picknum = 0;
    public Answer blueprint;
	// Use this for initialization
	void Start () {
        rnganswer = Random.Range(0, 4);
        foreach (PickingAnAnswer pick in picks)
        {
            if (picknum == rnganswer)
            {
                Correct();
            }
            else
            {
                picknum += 1;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Correct()
    {
        Answer CorAnswer =  Instantiate(blueprint, transform.position, Quaternion.identity) as Answer;
        DestroyObject(gameObject);
    }
}
