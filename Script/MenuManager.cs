using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Prime31;


public class MenuManager : MonoBehaviour {

 
 
	RaycastHit hit1;
	 
	public GameObject Car_Selection_Group,Mission_Selection_Group,Levels_Selection_Group;
	public GameObject []cars_images;
	public Texture2D []mission_images;
	public Texture2D []levels_images;
	public GameObject CarBG,MissionBG,LevelsBG; 
	public int car_index;
	public int mission_index;
	public int level_index;
	public GameObject CarParent;
	void Awake()
	{
		//AdMobAndroid.requestInterstital( "ca-app-pub-5400898951902983/6132720959" );

	}
	void Start () {

		car_index=2;
		// Android FullScreen 
		Invoke("ViewInterstital",3);
		//Invoke("ViewBanner",6);
		rot = CarParent.transform.GetChild(car_index).transform;
	}
	 
	public float rot_y , rot_x;
	Vector3 _value;
 	Transform rot ;

	bool animatedPlay;

	void ViewInterstital()
 	{
		//AdMobAndroid.displayInterstital();


	}

	void ViewBanner()
	{
		//Android banner
		 //AdMobAndroid.init( "ca-app-pub-5400898951902983/7609454157" );
		Invoke("ViewBannerSmart" , 3);

	}

	void ViewBannerSmart()
	{
		//AdMobAndroid.createBanner( //AdMobAndroidAd.smartBanner, AdMobAdPlacement.BottomCenter );

	}
	void letanimatedPlay()
	{
		animatedPlay = true;
		iTween.ScaleTo(splash_img.transform.GetChild(0).gameObject , iTween.Hash("x",.1 ,"y",.2f, "time",1,"oncompletetarget", gameObject , "oncomplete", "ScallOut",  "easeType",iTween.EaseType.easeInBounce  ));


	}


	void ScallOut()
	{
		iTween.ScaleTo(splash_img.transform.GetChild(0).gameObject , iTween.Hash("x",0.15484 ,"y",0.277082, "time",1,"oncompletetarget", gameObject , "oncomplete", "letanimatedPlay",  "easeType",iTween.EaseType.easeOutBounce  ));
	}
 	void Update () {

		if(splash_img.activeSelf && !animatedPlay)
			letanimatedPlay();


		int nbTouches = Hasan.Input.touchCount;
		RaycastHit hit;
		
		for (int i = 0; i < nbTouches; i++)
		{
			Hasan.Touch touch = Hasan.Input.GetTouch(i);
			
			 if (Car_Selection_Group.activeInHierarchy &&  touch.phase == TouchPhase.Moved) 
			{
				rot_y = touch.deltaPosition.y;
				rot_x = touch.deltaPosition.x;
				  
 
 				_value = new Vector3(0 , -rot_x*10*Time.deltaTime , 0);
 
				rot.Rotate(_value , Space.Self);

				 
  
 			}

			if(touch.phase  == TouchPhase.Ended )
			{	
			 
				Ray screenRay1 = Camera.main.ScreenPointToRay(touch.position);
				
				if (Physics.Raycast(screenRay1, out hit1 ))
				{
 					IPressed(hit1.transform.name);
 				}

			}

		}
	
	}

	public void IPressed(string str)
	{
		print ("eeee"+str);
		switch (str)
		{

		case "PlayBtn":
			GoToMenue();
		break;
 		case "LeftArrow_car":
			MoveCar(-1);
 		break;
		case "RightArrow_car":
			MoveCar(1);
		break;
  		case "LeftArrow_mission":
			MoveMission(-1);
		break;
		case "RightArrow_mission":
			MoveMission(1);
        break;
		case"RightArrow_level":
				MoveLevel(1);
		break;
		case "LeftArrow_level":
				MoveLevel(-1);
			break;
		case "M_Next":
			if(mission_index == 0 || mission_index == 2)
				Car_Selection_Group.transform.parent.gameObject.SetActive(true);
			else
				Levels_Selection_Group.transform.parent.gameObject.SetActive(true);
			playAudio(AudClick);

			Mission_Selection_Group.transform.parent.gameObject.SetActive(false);
			break;
		 
		case "L_Next":
			Levels_Selection_Group.transform.parent.gameObject.SetActive(false);
			Car_Selection_Group.transform.parent.gameObject.SetActive(true);
			playAudio(AudClick);

			break;
		case "play":
			Car_Selection_Group.transform.parent.gameObject.SetActive(false);
 
			PlayerPrefs.SetInt("mission_index",mission_index);
			PlayerPrefs.SetInt("level_index",level_index);
			PlayerPrefs.SetInt("car_index",car_index);
			playAudio(AudLunchGame);

			Application.LoadLevel(1);
			break;
		case "L_Back":
			Levels_Selection_Group.transform.parent.gameObject.SetActive(false);
			Mission_Selection_Group.transform.parent.gameObject.SetActive(true);
			playAudio(AudClick);
			break;
		case "C_Back":
			Car_Selection_Group.transform.parent.gameObject.SetActive(false);
			Mission_Selection_Group.transform.parent.gameObject.SetActive(true);
			playAudio(AudClick);

			break;
		  
		default:
			// do nothing ..

			break;
 		}
	}
	void MoveCar(int i)
	{
		if((i==-1 && car_index>0) || (i==1 && car_index<14))
		{
			cars_images[car_index].SetActive(false);
			car_index += i;
			cars_images[car_index].SetActive(true);

			rot = CarParent.transform.GetChild(car_index).transform;

			Car_Selection_Group.transform.gameObject.transform.FindChild("RightArrow_car").gameObject.SetActive(true);
			Car_Selection_Group.transform.gameObject.transform.FindChild("LeftArrow_car").gameObject.SetActive(true);

			playAudio(AudClick);


			if(car_index == 14)
				Car_Selection_Group.transform.gameObject.transform.FindChild("RightArrow_car").gameObject.SetActive(false);
			
			
			if(car_index == 0)
				Car_Selection_Group.transform.gameObject.transform.FindChild("LeftArrow_car").gameObject.SetActive(false);
		}
	}

	public GameObject splash_img;
	public GameObject game_menu;
	public AudioClip AudStartGame;
	public AudioClip AudClick;
	public AudioClip AudLunchGame;
	void GoToMenue()
	{
		ViewBanner();

		splash_img.SetActive(false);
		game_menu.SetActive(true);
		Mission_Selection_Group.SetActive(true);

		playAudio(AudStartGame);
	}
	void MoveMission(int i)
	{
		if((i==-1 && mission_index>0) || (i==1 && mission_index<2))
		{

			mission_index += i;
			MissionBG.GetComponent<Renderer>().material.mainTexture = mission_images[mission_index];

			Mission_Selection_Group.transform.FindChild("RightArrow_mission").gameObject.SetActive(true);
			Mission_Selection_Group.transform.FindChild("LeftArrow_mission").gameObject.SetActive(true);

			playAudio(AudClick);

			if(mission_index == 2)
				Mission_Selection_Group.transform.FindChild("RightArrow_mission").gameObject.SetActive(false);


			if(mission_index == 0)
				Mission_Selection_Group.transform.FindChild("LeftArrow_mission").gameObject.SetActive(false);

		}
	}



	void MoveLevel(int i)
	{
		if((i==-1 && level_index>0) || (i==1 && level_index<2))
		{
			level_index += i;
			LevelsBG.GetComponent<Renderer>().material.mainTexture = levels_images[level_index];

			Levels_Selection_Group.transform.FindChild("RightArrow_level").gameObject.SetActive(true);
			Levels_Selection_Group.transform.FindChild("LeftArrow_level").gameObject.SetActive(true);
			
			if(level_index == 2)
				Levels_Selection_Group.transform.FindChild("RightArrow_level").gameObject.SetActive(false);
			
			
			if(level_index == 0)
				Levels_Selection_Group.transform.FindChild("LeftArrow_level").gameObject.SetActive(false);
		}
	}

	void playAudio(AudioClip audio)
	{
		AudioSource.PlayClipAtPoint(audio,transform.position);
	}

}



















