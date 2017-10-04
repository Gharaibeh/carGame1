using UnityEngine;
using System.Collections;

public class WoodenBoxes : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider _collider)
	{
		if(_collider.transform.root.name.Contains("Car"))
		{
 			foreach(Transform T in transform)
			{
				T.GetComponent<Rigidbody>().isKinematic = false;
				Destroy(T.gameObject,5);
			}

		}

	}
}
