using UnityEngine;
using System.Collections;

public class fordCarControl : MonoBehaviour {



	public WheelCollider FrontLeftWheel  ;
	public WheelCollider FrontRightWheel  ;
	
	// These variables are for the gears, the array is the list of ratios. The script
	// uses the defined gear ratios to determine how much torque to apply to the wheels.
	public float[] GearRatio   ;
	public AudioClip[] GearSound   ;
	public AudioClip TessSound   ;
	public int CurrentGear =  0;
	public int LastGear = 0;
	private int AppropriateGear   ;
	
	// These variables are just for applying torque to the wheels and shifting gears.
	// using the defined Max and Min Engine RPM, the script can determine what gear the
	// car needs to be in.
	public float EngineTorque  = 600.0f;
	public float MaxEngineRPM   = 3000.0f;
	public  float MinEngineRPM   = 1000.0f;
	private float EngineRPM   = 0.0f;
	
	private float Horizantal   ;
	private float Vertical   ;
	public bool Tes   = false;
	public bool immobilized;                                                    
	public bool _AI;
	public GameManager _manager;


	// Use this for initialization
	void Start () {
		_manager = GameObject.FindGameObjectWithTag("manager").GetComponent<GameManager>();

		// I usually alter the center of mass to make the car more stable. I'ts less likely to flip this way.
		GetComponent<Rigidbody>().centerOfMass  = new Vector3(0, -1.5f, 0);
		LastGear = CurrentGear;

	}

	float _globalAcc;
	float _globalTurn;
	public void SetInput(float i)
	{
		_globalAcc = i;
	}
	public void SetTurn(float i)
	{
		_globalTurn = i;
	}


	// Update is called once per frame
	void Update () {
	


		if (Application.platform != RuntimePlatform.WindowsEditor)  
		{
 			Vertical = _globalAcc;
			Horizantal = _globalTurn;
				
		}
		else 
		{
			Vertical = Input.GetAxis("Vertical");
			Horizantal =  Input.GetAxis("Horizontal");
 
		}


		// This is to limith the maximum speed of the car, adjusting the drag probably isn't the best way of doing it,
		// but it's easy, and it doesn't interfere with the physics processing.
		GetComponent<Rigidbody>().drag = GetComponent<Rigidbody>().velocity.magnitude / 250;
		
		// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
		EngineRPM = (FrontLeftWheel.rpm + FrontRightWheel.rpm)/2 * GearRatio[CurrentGear];
		ShiftGears();
		
		// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
		// up to twice it's pitch, where it will suddenly drop when it switches gears.
		GetComponent<AudioSource>().pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0f ;


		if ( GetComponent<AudioSource>().pitch > 2.0f ) {
			GetComponent<AudioSource>().pitch = 2.0f;

		}
		if(  GetComponent<AudioSource>().clip != GearSound[CurrentGear] || !GetComponent<AudioSource>().isPlaying )
		{
			GetComponent<AudioSource>().clip = TessSound;
			GetComponent<AudioSource>().Play();
			
			
			if(!GetComponent<AudioSource>().isPlaying || GetComponent<AudioSource>().clip != GearSound[CurrentGear])
			{
				GetComponent<AudioSource>().clip  = GearSound[CurrentGear];
				GetComponent<AudioSource>().Play();
				LastGear = CurrentGear;
				
			}
			
		}
 

		// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
		// multiplied by the user input variable.
		FrontLeftWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Vertical;
		FrontRightWheel.motorTorque = EngineTorque / GearRatio[CurrentGear] * Vertical;
		
		// the steer angle is an arbitrary value multiplied by the user input.
		FrontLeftWheel.steerAngle = 30 * Horizantal;
		FrontRightWheel.steerAngle = 30 * Horizantal;
		
		
		
		
		// for left and right 
		var clampX = -Horizantal;
		clampX = Mathf.Clamp(clampX ,-.8f ,.8f);
		
		GetComponent<Rigidbody>().centerOfMass = new Vector3(clampX,-1.5f,0);





	}


	void ShiftGears() {
		
		// this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
		// the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
		if ( EngineRPM >= MaxEngineRPM ) {
			AppropriateGear   = CurrentGear;
			
			for ( int i = 0; i < GearRatio.Length; i ++ ) {
				if ( FrontLeftWheel.rpm * GearRatio[i] < MaxEngineRPM ) {
					AppropriateGear = i;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}

		if ( EngineRPM <= MinEngineRPM ) {
			AppropriateGear = CurrentGear;
			
			for ( var j = GearRatio.Length-1; j >= 0; j -- ) {
				if ( FrontLeftWheel.rpm * GearRatio[j] > MinEngineRPM ) {
					AppropriateGear = j;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
	}


	// Immobilize can be called from other objects, if the car needs to be made uncontrollable
	// (eg, from asplosion!)
	public void Immobilize ()
	{
		immobilized = true;
	}
	
	// Reset is called via the ObjectResetter script, if present.
	public void Reset()
	{
		immobilized = false;
	}


	// car statistics 
	public float _fuel;
	public float _sheild , _super_weapon  , _hits;
	public Transform life;
	private int _sheild_timer, _super_weapon_timer, _fuel_timer;
	
	void BoxContains(string _value)
	{
		switch(_value)
		{
		case "sheild":
		case "super_weapon":
			if(!_AI)
			{
				_manager.getSheildWeapon(_value);
			}
			 
			
			
			break;
		case "fuel":
			if(!_AI)
			{
				_fuel = 100;
				_manager.gotFuel(1);
			}
			break;
			
		}
	}
	// inst a shield 
	public GameObject sheild;
	private GameObject mySheild;
	void SetSheildOn()
	{
		mySheild =  Instantiate(sheild , transform.position , Quaternion.identity ) as GameObject;
		mySheild.transform.parent = transform;
		
	}
	void SetSheildOff()
	{
		Destroy (mySheild);
	}
	void SetWeaponOn()
	{
		_manager.StartSuperWeapon(this.gameObject);
	}
	void guiWeapon()
	{
		
	}
	void ReduceFuelPercentage()
	{
		if(_fuel > .02f)
		{
			_fuel -= .01f;
		}
		else
		{
			Immobilize();
			if(_manager)
				_manager.OneCarHasBeenDead(gameObject);
		}
		
	}
	
	public void IGotHitted()
	{
		_hits -= 1;
		
		if(_hits==0)
		{
			Immobilize();
			_manager.OneCarHasBeenDead(gameObject);
			if(!_AI)
				Invoke("GameisOver" , 3);
		}
		else
		{
			if(life.childCount>0)
				Destroy(life.GetChild(0).gameObject);
			else
				print("there are no more health to this car...");
		}

	}

	void GameisOver()
	{
		print("game is over ...!");
	}



}
