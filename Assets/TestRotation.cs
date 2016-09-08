using UnityEngine;
using System.Collections;

public class TestRotation : MonoBehaviour {

    public Transform trans;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        trans.Rotate(new Vector3(0, 0, 45));

    }   
}
