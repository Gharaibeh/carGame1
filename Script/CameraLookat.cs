using UnityEngine;
using System.Collections;

public class CameraLookat : MonoBehaviour {

	public Transform _target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(!_target)
			return ;
		transform.LookAt(_target);
	
	}
}
