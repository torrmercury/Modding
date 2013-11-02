using UnityEngine;
using System.Collections;

public class WaterScrolling : MonoBehaviour {
    float scrollSpeed = 0.2F;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
    void Update() 
    {
        float offset = Time.time * scrollSpeed;
        renderer.material.mainTextureOffset = new Vector2(Mathf.Sin(offset), offset);
	}
}
