using UnityEngine;
using System.Collections;

public class targetsFun : MonoBehaviour {

	bool _trun;
	public GameManager _gameManager;
	// Use this for initialization
	void Start()
	{
		// Invoke("HideTargets" , Random.Range(20,70));
	}
	void LetsRotate()
	{
		_trun = true;
 	}
	void Update () {

		if(_trun)
			transform.Rotate(Vector3.up *50* Time.deltaTime);
		 
 	}

	public void ShowTargets ()
	{
		transform.Rotate(0,Random.Range(0,180),0);
		iTween.MoveAdd(gameObject, iTween.Hash( "y", 6.5f ,"time",3,"oncompletetarget", gameObject, "oncomplete", "LetsRotate",  "easeType", "easeInOutExpo"  ));
		
		
	}
	public void HideTargets()
	{
		_trun = false;
		transform.GetChild(0).gameObject.SetActive(false);
		iTween.MoveAdd(gameObject, iTween.Hash( "y", -6 ,"time",1,"oncompletetarget", gameObject, "oncomplete", "",  "easeType",iTween.EaseType.linear  ));
 	}

	void OnTriggerEnter(Collider _collider)
	{
		if(_collider.transform.root.name.Contains("Car"))
		{
			transform.GetComponent<BoxCollider>().enabled = false;
			_collider.transform.root.SendMessage("BoxContains",MyValue);
			_gameManager.oneBoxHasBeenDetected(this.gameObject);
			HideTargets();
		}
		//Destroy(gameObject);
	}
	public string MyValue;
	public void SetValue(string str)
	{
		MyValue = str;
		SetUpEffect(MyValue);


	}
	public GameObject []effects;
	void SetUpEffect(string value)
	{
		GameObject _effect = null;
		switch (value)
		{
		case "sheild": //1
			_effect = Instantiate(effects[1],transform.position+Vector3.up * -1 , Quaternion.identity) as GameObject;
			break;
		case "super_weapon": // 2
			_effect = Instantiate(effects[2],transform.position+Vector3.up * -1 , Quaternion.identity) as GameObject;
			break;
		case "nitrus": // 0
			_effect = Instantiate(effects[0],transform.position+Vector3.up * -1 , Quaternion.identity) as GameObject;
			break;

		}
		_effect.transform.parent = transform;



	}
}
