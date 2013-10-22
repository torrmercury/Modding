using UnityEngine;
using System.Collections;

public class 
    ballclack : MonoBehaviour {
    
    public Vector3 start, end;


	// Use this for initialization
	void Start () 
    {
        StartCoroutine( BallMove() );
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}
    IEnumerator BallMove()
    {
        while (true)
        {
            float t = Mathf.Abs(Mathf.Sin(Time.time));
            transform.position = Vector3.Lerp(start, end, t);
            if (t > .49f && t < .51f && audio.isPlaying == false)
            {
                audio.Play();
            }
        }
    }

}
