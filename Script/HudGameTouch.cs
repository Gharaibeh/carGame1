using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Prime31;


public class HudGameTouch : MonoBehaviour {

 
 
	RaycastHit hit1;
	  
	public Camera _hud;
	public Transform _pauseGroup;
	public Transform Car;


	void Start () {

		//AdMobAndroid.init( "ca-app-pub-5400898951902983/7609454157" );
		//AdMobAndroid.requestInterstital( "ca-app-pub-5400898951902983/6132720959" );

		// if Free and AI ->> remove timer
		if(PlayerPrefs.GetInt("mission_index") != 1)
		{
			_cameraPlace = 2;
			//Destroy(transform.FindChild("Timer").gameObject);
			
			// if and only if AI
			if(PlayerPrefs.GetInt("mission_index") == 2)
			{
				/*Destroy(speedmeter.gameObject);
				Destroy( transform.FindChild("speedmeter").gameObject);
				Destroy(_speedtext2.transform.parent.gameObject);*/
			}
		}
		else
			_cameraPlace = 4;

 	}
	 
	 

 
 	void Update () {

		 


		int nbTouches = Hasan.Input.touchCount;
		RaycastHit hit;
		
		for (int i = 0; i < nbTouches; i++)
		{
			Hasan.Touch touch = Hasan.Input.GetTouch(i);
			
			 

			if(touch.phase  == TouchPhase.Ended )
			{	
			 
				Ray screenRay1 = _hud.ScreenPointToRay(touch.position);
				
				if (Physics.Raycast(screenRay1, out hit1 ))
				{
 					IPressed(hit1.transform.name);
 				}

			}

		}
	
	}

	bool buttonPause;
	public void IPressed(string str)
	{

		switch (str)
		{

		case "camera":
			StartCoroutine("ChangeCamera");
		
		break;
 		 
		case "home":
			Time.timeScale = 1;
			Application.LoadLevel(0);
		break;
		case "pause":
		case "resume":
			print("you touched : "+str);
			buttonPause = !buttonPause;
			if(buttonPause) // once your pressed pause:
			{
				//pause 
				Time.timeScale = 0;
				_pauseGroup.FindChild("pause").gameObject.SetActive(false);
				_pauseGroup.FindChild("resume").gameObject.SetActive(true);
				_pauseGroup.FindChild("home").gameObject.SetActive(true);
				//transform.FindChild("camera").gameObject.SetActive(false);

 
			}
			else
			{
				// resume 
				Time.timeScale = 1;
				_pauseGroup.FindChild("pause").gameObject.SetActive(true);
				_pauseGroup.FindChild("resume").gameObject.SetActive(false);
				_pauseGroup.FindChild("home").gameObject.SetActive(false);
				//transform.FindChild("camera").gameObject.SetActive(true);
				//AdMobAndroid.createBanner( AdMobAndroidAd.smartBanner, AdMobAdPlacement.BottomCenter );

			} 
		break;
		case"RightArrow_level":
				//MoveLevel(1);
		break;
		case "LeftArrow_level":
				//MoveLevel(-1);
			break;
		  
		default:
			// do nothing ..

			break;
 		} 
	}



	int _cameraPlace;
	public int _camIndex=0;
	bool _cameraChanged = true;

	IEnumerator  ChangeCamera()
	{
		print (_cameraPlace);
		if(_cameraPlace==2)
		{
			if(_camIndex%_cameraPlace == 0)
			{
				Car.FindChild("Camera").gameObject.SetActive(true);
			}
			else
				if(_camIndex%_cameraPlace == 1)
			{
				Car.FindChild("Camera").gameObject.SetActive(false);
			}
			
		}
		
		else
			if(_cameraPlace == 4)
		{
			if(_camIndex%_cameraPlace == 0) // infront of car
			{
				Car.FindChild("Camera").gameObject.SetActive(true);
				GameObject.Find("Cone(Clone)").transform.FindChild("Camera").gameObject.SetActive(false);
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = false;
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow_Target>().enabled = false;
				
				
				
			}
			else
				if(_camIndex%_cameraPlace == 1) // look at car with cone target
			{
				Car.FindChild("Camera").gameObject.SetActive(false);
				GameObject.Find("Cone(Clone)").transform.FindChild("Camera").gameObject.SetActive(false);
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = false;
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow_Target>().enabled = true;
				
				
				
			}
			else
				if(_camIndex%_cameraPlace == 2) // on cone
			{
				Car.FindChild("Camera").gameObject.SetActive(false);
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = false;
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow_Target>().enabled = true;
				GameObject.Find("Cone(Clone)").transform.FindChild("Camera").gameObject.SetActive(true);
				GameObject.Find("Cone(Clone)").transform.FindChild("Camera").GetComponent<CameraLookat>()._target = Car; 
			}
			else
				if(_camIndex%_cameraPlace == 3) // follow car
			{
				Car.FindChild("Camera").gameObject.SetActive(false);
				
				GameObject.Find("Cone(Clone)").transform.FindChild("Camera").gameObject.SetActive(false);
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow_Target>().enabled = false;
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>().enabled = true;
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>().target = Car.FindChild("lookat");
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>().distance = 10.6f;
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>().height = 1.0f;
				GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>().rotationDamping = .5f;
				
			}
		}
		
		
		_camIndex++;
		yield return new WaitForSeconds(.5f);
		_cameraChanged = true;
		
	}


	public void DisableForResultMenu()
	{
		_pauseGroup.gameObject.SetActive(false);


	}

	void OnApplicationFocus(bool focusStatus) {

 		//AdMobAndroid.displayInterstital();

		print ( "focusStatus   "+ focusStatus);
		if(focusStatus && !buttonPause)
			IPressed("pause");

	}
	 

	 
	 
 
}



















