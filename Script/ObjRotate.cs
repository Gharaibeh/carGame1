using UnityEngine;
using System.Collections;

public class ObjRotate : MonoBehaviour {

	public	GameObject  targetItem ;
	public Camera GUICamera  ;
 
	
	/********Rotation Variables*********/
	public float  rotationRate   = 10.0f;
	bool wasRotating;
	
	/************Scrolling inertia variables************/
	private Vector2 scrollPosition    = Vector2.zero;
	private float scrollVelocity    = 0;
	private float timeTouchPhaseEnded  ;
	private float inertiaDuration    = 0.5f;
	
	private float itemInertiaDuration   = 1.0f;
	private float itemTimeTouchPhaseEnded ;
	private float rotateVelocityX  = 0;
	private float rotateVelocityY   = 0;
	
	
	public RaycastHit hit ;
	
	private LayerMask layerMask = (1 <<  8) | (1 << 2);
	//private var layerMask = (1 <<  0);
	
	
	void Start()
	{
		layerMask =~ layerMask;    
		print(gameObject.name);
	}
	public float rot_y , rot_x;
	void Update()
	{		print(gameObject.name);

		if (Input.touchCount > 0) 
		{
			Touch theTouch   = Input.GetTouch(0); 
			if (theTouch.phase == TouchPhase.Moved) 
			{
				rot_y = theTouch.deltaPosition.y;
				rot_x =0;// theTouch.deltaPosition.x;
				targetItem.transform.Rotate(( rot_y * rotationRate * Time.deltaTime)
				                            , (-rot_x  * rotationRate * Time.deltaTime),
				                            0, Space.World);
			}
		} 

	}
	void FixedUpdateq()
	{
		
		if (Input.touchCount > 0) 
		{        //    If there are touches...
			Touch theTouch   = Input.GetTouch(0);        //    Cache Touch (0)
			
			var ray = Camera.main.ScreenPointToRay(theTouch.position);
 
			
			if(Physics.Raycast(ray,out hit,500, layerMask ))
			{    
				
				if(Input.touchCount == 1)
				{
					
					if (theTouch.phase == TouchPhase.Began) 
					{
						wasRotating = false;    
					}        
					
					if (theTouch.phase == TouchPhase.Moved) 
					{
						
						targetItem.transform.Rotate(0, theTouch.deltaPosition.x * rotationRate * Time.deltaTime ,0,Space.World);
						wasRotating = true;
					}        
					
				}
				
				
				
				
				
			}
		}
	}
}
