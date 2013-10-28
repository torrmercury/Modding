using UnityEngine;
using System.Collections;

public class 
    ballclack : MonoBehaviour {
    
    public Vector3 start, end;

    Vector3 baseCameraPosition;

	// Use this for initialization
	void Start () 
    {
        StartCoroutine( BallMove() );
        baseCameraPosition = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () 
    {
        
	}


    IEnumerator ScreenShake()
    {
        float t = 1;
        while (t > 0)
        {
            t -= Time.deltaTime;
            Camera.main.transform.position = baseCameraPosition + t *
                                             new Vector3(Mathf.Sin(Time.time * 10f),
                                                         Mathf.Sin(Time.time * 12.5f),
                                                         Mathf.Sin(Time.time * 7f));
            yield return 0;
        }
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
                StartCoroutine(ScreenShake());
            }
            yield return 0;
        }
    }

    //implenenting screen shake in a super hacky way


}
