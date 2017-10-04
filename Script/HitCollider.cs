using UnityEngine;
using System.Collections;

public class HitCollider : MonoBehaviour {

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
			print("*****  "+transform.name);
			// flicker if level = 3 ( kick out )
			if(PlayerPrefs.GetInt("mission_index") == 2)
				transform.root.SendMessage("IGotHitted");
			GameObject.FindWithTag("MainCamera").GetComponent<camerashake>().Shake_OnCollision();
 		}

		//if(_collider.transform.name.Contains("wall") || _collider.transform.name.Contains("ops"))
			//GameObject.FindWithTag("MainCamera").GetComponent<camerashake>().Shake_OnCollision();
 	}
 	 

}
