using UnityEngine;
using System.Collections;

public class SkyRotation : MonoBehaviour {
	public float scale;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate(scale * Vector3.up * Time.deltaTime );
	
	}
}
