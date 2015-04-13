using System;
using UnityEngine;
using System.Collections;
 
public class CameraShake : MonoBehaviour {
        public CharacterController playerController;
        public Animation anim; //Empty GameObject's animation component
		private GameObject cameraOverlay;
        private bool isMoving;
		private CharacterMotor characterMotor;
		private float stabilizingCounter = 1.0f;
       
        private bool left;
        private bool right;
       
	
		void Start() {
		cameraOverlay = GameObject.Find ("cameraOverlay");
		characterMotor = GameObject.Find ("First Person Controller").GetComponent<CharacterMotor>();
		}
        void CameraAnimations(){
			if(cameraOverlay == null)
			{
				cameraOverlay = GameObject.Find ("cameraOverlay");
			}
			else
			{
	            if((characterMotor.inputMoveDirection == Vector3.zero) && cameraOverlay.activeSelf && stabilizingCounter > 0){
	            	anim.Play ();                
	            }
				else anim.Stop ();
        	}
		}
	
	
		
        void Update () {
		
            if(cameraOverlay == null)
			{
				cameraOverlay = GameObject.Find ("cameraOverlay");
			}
			else if((characterMotor.inputMoveDirection == Vector3.zero) && cameraOverlay.activeSelf && Input.GetKey (KeyCode.H) && stabilizingCounter >= 0.0f)
			{
				stabilizingCounter -= 0.01f;
				GameObject.Find ("First Person Controller").GetComponent<Screenshot>().infoText = "Stabilizing.."+stabilizingCounter.ToString ("F2");
				if(stabilizingCounter <= 0.0f) 
				{
				GameObject.Find ("First Person Controller").GetComponent<Screenshot>().stabilized = true;
				GameObject.Find ("First Person Controller").GetComponent<Screenshot>().infoText = "Take the picture now ('Mouse1')!";
				}
			}
			else if((characterMotor.inputMoveDirection != Vector3.zero) && cameraOverlay.activeSelf && Input.GetKey (KeyCode.H))
			{
				GameObject.Find ("First Person Controller").GetComponent<Screenshot>().infoText = "Stand still!";
			}
			else if (!Input.GetKey (KeyCode.H)) 
			{
				stabilizingCounter = 1.0f;
				GameObject.Find ("First Person Controller").GetComponent<Screenshot>().stabilized = false;
			}
			else if (!cameraOverlay.activeSelf && Input.GetKey (KeyCode.H))
			{
				GameObject.Find ("First Person Controller").GetComponent<Screenshot>().infoText = "Take out the cam ('C')!";
			}
			CameraAnimations();
       
        }
}

