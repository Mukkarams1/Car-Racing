using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunCameraMovement : MonoBehaviour {
	
	public GameObject zoomPlane;//the sniper feature plane
	GameObject allScripts;//access cheats
	
	float zoomIn=5;
	float zoomOut=60;
	[HideInInspector] public bool showZoom;
	Color colorForAlpha;
	float fieldOfView;
	float lerpSpeed=20f;

	public float mouseRotationMultiplier=1f;


	GameObject car;//get the car for other stuff like speed
	//car speed text gui stuff
	GameObject speedTextObj;
    Text speedText;



	//camera movement
	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	float rotationY = 0F;
	
	void Start(){
		//current field of view

		GetComponent<Camera>().fieldOfView = zoomOut;
		fieldOfView = zoomOut;
		//the sniper feature disabled when this script is activated
		colorForAlpha = zoomPlane.GetComponent<Renderer>().material.color;
		colorForAlpha.a = 0;
		zoomPlane.GetComponent<Renderer>().material.color = colorForAlpha;
		
		//to get the cheats component
		allScripts = GameObject.Find ("_SCRIPTS");

		//car speed stuff and car find
		car = GameObject.FindGameObjectWithTag("Player");
		speedTextObj = GameObject.Find ("Car Speed");
		//speedText = speedT;
	}
	
	void Update(){
		if(Time.timeScale != 0f){
			if(allScripts.GetComponent<AllCheats> ().zoomAllowed){
				
				if(Input.GetKeyDown(KeyCode.Z)){
					if(showZoom){
						showZoom=false;
						mouseRotationMultiplier=1f;
					}
					else{
						showZoom=true;
						mouseRotationMultiplier/=11.2f;
					}
				}
				
				if(showZoom){
					fieldOfView=Mathf.Lerp(fieldOfView, zoomIn, lerpSpeed*Time.deltaTime);
					if(fieldOfView<30f) colorForAlpha.a=Mathf.Lerp(colorForAlpha.a, 1, lerpSpeed*1.5f*Time.deltaTime);
				}
				if(!showZoom){
					fieldOfView=Mathf.Lerp(fieldOfView, zoomOut, lerpSpeed*Time.deltaTime);
					colorForAlpha.a=Mathf.Lerp (colorForAlpha.a, 0, lerpSpeed*1.5f*Time.deltaTime);
				}

				GetComponent<Camera>().fieldOfView=fieldOfView;
				zoomPlane.GetComponent<Renderer>().material.color = colorForAlpha;
			}




			//camera movement

			if (axes == RotationAxes.MouseXAndY)
			{
				float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX * mouseRotationMultiplier;
				
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY * mouseRotationMultiplier;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
			}
			else if (axes == RotationAxes.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX * mouseRotationMultiplier, 0);
			}
			else
			{
				rotationY += Input.GetAxis("Mouse Y") * sensitivityY * mouseRotationMultiplier;
				rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
				
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}

		}
	}
	
	void LateUpdate(){
		if(Time.timeScale!=0){
			transform.position = GameObject.FindGameObjectWithTag ("Gun").transform.position;
			transform.Translate (-Vector3.forward * 25);
			transform.Translate(Vector3.up * 6);
		}

		int carVelocity = (int) car.GetComponent<Rigidbody>().velocity.magnitude * 2;
		string carVelocityToString = carVelocity.ToString ();
		speedText.text = carVelocityToString + " km/h";

	}
}