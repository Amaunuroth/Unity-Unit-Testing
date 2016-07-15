using UnityEngine;
using System.Collections;

public class DelauneyTreeTester : MonoBehaviour {

	// Use this for initialization
    void Start () {
        UnityEngine.Rect rect = new UnityEngine.Rect(0f, 0f, 100f, 100f);
        DelauneyTriangles dt = new DelauneyTriangles(100, rect, -.5f, 2f);
        dt.MakeMesh();
	}
}
