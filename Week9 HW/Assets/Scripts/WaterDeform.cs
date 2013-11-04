using UnityEngine;
using System.Collections;

public class WaterDeform : MonoBehaviour {
    MeshFilter mf;
    Vector3[] baseVertices;
    Vector3[] workingCopy;
    float waveHeight = .1f;

	// Use this for initialization
	void Start () 
    {
        mf = GetComponent<MeshFilter>();
        baseVertices = mf.mesh.vertices;
	}
	
	// Update is called once per frame
	void Update () 
    {
        //every frame starts with a fresh copy of the untouched plane
        workingCopy = baseVertices; 
        for (int i = 0; i < workingCopy.Length; i++)
        {
            workingCopy[i] = baseVertices[i] + Vector3.up * Mathf.Sin(Time.time + i) * waveHeight;
        }

        mf.mesh.vertices = workingCopy;

        //normals to see directions
        mf.mesh.RecalculateNormals();
        for (int i = 0; i<mf.mesh.vertices.Length;i++)
        {
            Debug.DrawRay(mf.mesh.vertices[i], mf.mesh.normals[i]);
        }
	}
}
