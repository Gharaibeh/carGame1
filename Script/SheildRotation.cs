using UnityEngine;
using System.Collections;

public class SheildRotation : MonoBehaviour {


	bool letsRotate;
	// Use this for initialization
	void Start () {
		letsRotate = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(letsRotate)
			transform.Rotate(0,0,Time.deltaTime*12);
	}

	void OnEnable() {
		letsRotate = true;
	}
	void OnDisable() {
		letsRotate = false;
	}
}
