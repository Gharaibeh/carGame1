using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour {

	public List<GameObject> GameCars;
	public int mission_index,level_index,car_index;

	public GameObject _targetCar1,_targetCar2  ;
	public GameObject []_targetAI;
	public GameObject _camera;
	public checkpoint _coneTarget;
	public Timer _timer;
	public GameObject _hud;
 	// privates obj 
	public GameObject _currentCar;
	public Transform _cars_Inst_Pos;
	public bool _GameHasFinished;
	// Use this for initialization
 	void Start () {
	
		_GameHasFinished = true;

		StartCoroutine( StartTheGame());


	}

	IEnumerator StartTheGame()
	{


		yield return new WaitForSeconds(.1f);
		print ("children are : "+transform.childCount);
		GameCars = new List<GameObject>(4);
		mission_index = PlayerPrefs.GetInt("mission_index");
		level_index = PlayerPrefs.GetInt("level_index");
		car_index = PlayerPrefs.GetInt("car_index");
		
		
		// save camera scripts to enable modes
		
		// Set up player car
		SetUpCar();
		// set up camera

 		// if free mode
		if(mission_index == 0)
		{
			// delete timer
			Destroy(_currentCar.transform.FindChild("CarSheild").gameObject);
			Destroy(_currentCar.transform.FindChild("CarHealth").gameObject);
			Destroy(_timer.gameObject);
		}
		else // if time Attach 
			if(mission_index == 1)
		{ 
			Destroy(_currentCar.transform.FindChild("CarSheild").gameObject);
			Destroy(_currentCar.transform.FindChild("CarHealth").gameObject);
			SetUpTarget ();
			SetUpAI_Attack(1);
			
		}
		else // if AI
		{
			Destroy(_timer.gameObject);
			StartCoroutine("SponingTargets");
			SetUpAI(3);
		}
		SetUpCameraTarget(mission_index);
		_hud.GetComponent<test2>().SetupCounter();
		yield return new WaitForSeconds(4);
		foreach(GameObject car in GameCars)
			car.GetComponent<CarController>().Reset();

		_GameHasFinished = false;
		

	}
	GameObject _cone;
	void SetUpTarget()
	{
		_cone = Instantiate(_coneTarget.gameObject , new Vector3(100,100,100) , Quaternion.identity) as GameObject 	;
		_cone.GetComponent<checkpoint>().SetUpGameManager(gameObject);
	}
 	void OnGUI()
	{


 
		/*
		if (GUI.Button(new Rect(0,10, 100, 100), "+"))
			hSbarValue+= .1f;

		if (GUI.Button(new Rect(150, 10, 100, 100), "-"))
			hSbarValue-= .1f;

		GUI.Label(new Rect(100, 10, 100, 20), hSbarValue.ToString());


		if (GUI.Button(new Rect(300, 10, 100, 100), "AI_3"))
			SetUpCameraTarget1(3);
*/
		 //if (GUI.Button(new Rect(450, 10, 70, 30), "Thunder"))
		 	// PlayThunder();



	}
	bool skythunder;
	public void PlayThunder()
	{
		 
 			GameObject.Find("Sky").GetComponent<Renderer>().material.shader = Shader.Find("Mobile/Bumped Diffuse")  ;
			_hud.transform.Find("smashes").GetComponent<Renderer>().enabled = true;
			int posX = Random.Range(0,2);
			if(posX == 0)
 				GameObject.Find("smashes").transform.localScale = new Vector3(17, 9.9f , 10);
			else
				GameObject.Find("smashes").transform.localScale = new Vector3(-17, 9.9f , 10);

 		 
			Invoke("SkyBoxToNormal",2);



	}
	void SkyBoxToNormal()
	{
		_hud.transform.Find("smashes").GetComponent<Renderer>().enabled = false;
		GameObject.Find("Sky").GetComponent<Renderer>().material.shader = Shader.Find("Mobile/Unlit (Supports Lightmap)")  ;
 	}
	void SetUpCameraTarget1(int i)
	{
		if(GameCars[i]!=null)
		_camera.GetComponent<SmoothFollow>().target = GameCars[i].transform; 

	}
	public void cameraShake()
	{
		//_camera.GetComponent<camerashake>().Shake1(); 
	}
	void SetUpCar()
	{
		 print(car_index);
		_currentCar = Instantiate(_targetAI[car_index] , new Vector3(0,0,0), _targetAI[car_index].transform.rotation)as GameObject;
		_currentCar.GetComponent<CarAIControl>().enabled = false;
		_currentCar.GetComponent<CarController>()._AI = false;
		_currentCar.GetComponent<CarController>().Immobilize();
		/*
		if(car_index<5)
		{
			// for closed circte 
			 _currentCar = Instantiate(_targetCar1 , new Vector3(0,0,0), _targetCar1.transform.rotation)as GameObject;

			// for Ahmad Track
			// _currentCar = Instantiate(_targetCar1 , new Vector3(585,-24,1952), _targetCar1.transform.rotation)as GameObject;
		}
		else
		{
			// for closed circte
			_currentCar = Instantiate(_targetCar2 , new Vector3(0,0,0), _targetCar2.transform.rotation)as GameObject;

			//  for Ahmad Track
			//_currentCar = Instantiate(_targetCar2 , new Vector3(585,-24,1952), _targetCar2.transform.rotation)as GameObject;
 


		}*/
		_hud.GetComponent<HudGameTouch>().Car = _currentCar.transform;
		_hud.GetComponent<test2>().Car = _currentCar.transform;
		//_currentCar.GetComponent<CarController>()._manager = gameObject as GameManager  ;
		GameCars.Add(_currentCar);
		 


	}
	void SetUpAI_Attack(int number_cars)
	{
		int car_ai_index = car_index;
		while(car_ai_index == car_index )
		{
			car_ai_index = Random.Range(0,15);
		}


		GameObject _carAI = Instantiate(_targetAI[car_ai_index] , _cars_Inst_Pos.GetChild(Random.Range(0,3)).position , _targetAI[car_ai_index].transform.rotation) as GameObject ;
		_carAI.GetComponent<CarController>().Immobilize();


		/*for(int i=0 ; i<number_cars    ; i++)
		{
			if( i!=car_index)
 				GameObject _carAI = Instantiate(_targetAI[i] , _cars_Inst_Pos.GetChild(Random.Range(0,3)).position , _targetAI[i].transform.rotation) as GameObject ;
 				_carAI.GetComponent<CarController>().Immobilize();
  		}*/

 	}
	public Transform GetCone()
	{
		return _cone.transform;

	}
	void SetUpAI(int number_cars)
	{
		int pos_index = 0;
		for(int i=0 ; i<number_cars ; i++)
		{
			if(i!=car_index)
			{
	 			GameObject _carAI = Instantiate(_targetAI[i] , _cars_Inst_Pos.GetChild(pos_index).position , _targetAI[i].transform.rotation) as GameObject ;
				pos_index += 1;
				//_carAI.GetComponent<CarAIControl>().SetTarget(_checkPoint.transform.GetChild(0).GetChild(0).transform);
				//_currentCar.GetComponent<CarController>()._manager = gameObject as GameManager;
				GameCars.Add(_carAI);
				_carAI.GetComponent<CarController>().Immobilize();

			}
		}
	}
	void SetUpCameraTarget(int index)
	{
		if(index == 0 || index == 2) // free or AI
		{

			_camera.GetComponent<SmoothFollow>().enabled = true;
			_camera.GetComponent<SmoothFollow>().target = _currentCar.transform.FindChild("lookat");

			_camera.GetComponent<SmoothFollow>().distance = 10.6f;
			_camera.GetComponent<SmoothFollow>().height = 1.0f;
			_camera.GetComponent<SmoothFollow>().rotationDamping = 1;//.5f;

		}
		else // if Timer
		{
			_camera.GetComponent<CameraFollow_Target>().enabled = true;
			_camera.GetComponent<CameraFollow_Target>().Target = _cone.transform;
			_camera.GetComponent<CameraFollow_Target>().Car = _currentCar.transform ;
		}
		print ("this is camera ..." + _currentCar.transform.name);
		/*_camera.transform.GetComponent<CameraFollow_Target>().enabled = true;
		_camera.GetComponent<CameraFollow_Target>().Car = _currentCar.transform;
		_checkPoint.ChangeMyPosition();
		_camera.GetComponent<CameraFollow_Target>().Target = _checkPoint.transform;
		*/
	}


	public void TimerExtra()
	{
 		if(level_index == 0)
		{
			_timer.ExtraTime(7);
			_hud.GetComponent<test2>().TimerEffect(7);
		}
		else
			if(level_index == 1)
		{
			_timer.ExtraTime(5);
			_hud.GetComponent<test2>().TimerEffect(5);

		}
		else
			if(level_index == 2)
		{
			_timer.ExtraTime(3);
			_hud.GetComponent<test2>().TimerEffect(3);

		}
	}
	public void TimerFinished()
	{
		ResultSecreen(false);
	}
 
 	
	// for sponing points targets and cubes 
	public  GameObject _target_box;
	public GameObject _target_pos;
	public GameObject _particle_effect , dustParticle , _particle_lightStrike;
	public List <GameObject>_allBoxes;
	IEnumerator SponingTargets () {
	
		_allBoxes = new List<GameObject>(_target_pos.transform.childCount);
		_myboxes = new List<GameObject>(_target_pos.transform.childCount);
 		_allboxes = 9;
		/*foreach(Transform tra in _target_pos.transform)
		{
			Instantiate(_particle_effect , tra.position , Quaternion.identity);
		}*/
		yield return new WaitForSeconds(3);

		foreach(Transform tra in _target_pos.transform)
		{
			tra.GetComponent<targetsFun>().ShowTargets();
 
			_allBoxes.Add(tra.gameObject);
			_myboxes.Add(tra.gameObject);
		}



		// assign values : sheilds 2 , super weapon 1, fuel 3 , null 3.
 
 		SetBoxValue("sheild",3);
		SetBoxValue("super_weapon",3);
		SetBoxValue("nitrus",3);
 
		 

		 
 
 	}
	public List <GameObject> _myboxes =  new List<GameObject>(9);
	void SetBoxValue(string _value , int _howMany)
	{
		for(int i=0 ; i<_howMany ; i++)
		{
			int box_random = Random.Range(0,_myboxes.Count);
			_myboxes[box_random].GetComponent<targetsFun>().SetValue(_value);
 			_myboxes.RemoveAt(box_random);
		}
		
		
	}

	// check if boxxes == 0 then respown again with random values 
	public int _allboxes ;
	public void oneBoxHasBeenDetected(GameObject targetpos)
	{

		_allboxes -= 1;
		_allBoxes.Remove(targetpos);
		// amount of particle
		//for(int g=0 ; g<3 ; g++)
			//Instantiate(dustParticle , targetpos.transform.position , dustParticle.transform.rotation);
		 
	}
	public void OneCarHasBeenDead(GameObject car)
	{
		GameCars.Remove(car);
		if(GameCars.Count == 1)
			if(GameCars[0] == _currentCar)
				ResultSecreen(true);
	}


	// get shield and super weapon only if player car not AI
	public void getSheildWeapon(string btn)
	{
		_hud.GetComponent<test2>().ShowBtn(btn);
 	}
	public void StartSuperWeapon(GameObject exeptCar)
	{
 			
		//StartCoroutine(EffectForLightStike(exeptCar));

		for(int i=0 ; i<GameCars.Count ; i++)
		{
			if(GameCars[i] != exeptCar)
			{
				if(GameCars[i])
				{
					GameCars[i].SendMessage("IGotHitted");//.GetComponent<CarController>().IGotHitted();
					Instantiate(_particle_lightStrike , GameCars[i].transform.position , Quaternion.identity);

 				}

 			}
		}
		Invoke("unFreezCars",5);
	}
	IEnumerator EffectForLightStike(GameObject exeptCar)
	{

		for(int j=0;j<3;j++)
		for(int i=0 ; i<GameCars.Count && GameCars[i]!=exeptCar ; i++)
		{
			yield return new WaitForSeconds(.5f);
 				
			Instantiate(_particle_lightStrike , GameCars[i].transform.position , Quaternion.identity);
				
 		}

	}
	void unFreezCars()
	{
		foreach(GameObject _car in GameCars)
		{
			_car.SendMessage("Reset");//.GetComponent<CarController>().Reset();
		}
	}
	public void gotFuel(int i)
	{
		if(i==1) // the player is not AI
		{
			_hud.GetComponent<test2>().setFullFuel();
		}
	}
	public void gotNitrus()
	{
		//_camera.GetComponent<camerashake>().Shake1();
		_camera.GetComponent<MotionBlur>().enabled = true;
		Invoke("CancelBluer",5);
	}

	void CancelBluer()
	{
		CancelInvoke("CancelBluer");
		_camera.GetComponent<MotionBlur>().enabled = false;
	}


	// Result menu:
	void ResultSecreen(bool status) 
	{
		if(_timer)
			_timer.gameObject.SetActive(false);
		_hud.GetComponent<test2>().DisableForResultMenu(status);
		_hud.GetComponent<HudGameTouch>().DisableForResultMenu();
		_GameHasFinished = true;
	}
	
}
