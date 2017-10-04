using UnityEngine;
using System.Collections;

public class particleInst : MonoBehaviour {

	public GameObject _particle;
	public GameObject _particles_des;
	public GameObject _particle_lightStrike;
	public GameObject _cube;
	// Use this for initialization
	void Start () {
	  
		//Invoke("Inst_part",5);
		StartCoroutine("Inst_part");
	}
	void Update()
	{
		 
	}
	// Update is called once per frame
	IEnumerator Inst_part () {
		for(int i=0 ; i<5 ; i++)
		{
			yield return new WaitForSeconds(.5f);
			foreach(Transform tra in transform)
		{

				Instantiate(_particle_lightStrike , tra.position , Quaternion.identity);
			
		}
		}
		
	}
	void Inst_cube () {
		foreach(Transform tra in transform)
		{
			GameObject target =  Instantiate(_cube ,new Vector3( tra.position.x, -.5f,tra.position.z) , Quaternion.identity) as GameObject;

		}
		
	}

	void Inst_Des()
	{
		foreach(Transform tra in transform)
		{
			Instantiate(_particles_des , tra.position , Quaternion.identity);
		}
		 
	}
}
