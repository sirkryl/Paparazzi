using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSInputController script to the capsule
///   -> A CharacterMotor and a CharacterController component will be automatically added.

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -60F;
	public float maximumY = 60F;

	float rotationY = 0F;
	bool tr = true;
	
	private GameObject cameraOverlay; //edit

	void Update ()
	{
		// edit start
		if(cameraOverlay == null)
		{
			cameraOverlay = GameObject.Find ("cameraOverlay");
			tr = true;
		} 
		
		if(cameraOverlay != null)
		{
	        if (cameraOverlay.activeSelf)
			{
				sensitivityX = 15F + Random.Range(10F, 20F);
				sensitivityY = 15F + Random.Range(10F, 20F);
				//float minMove = Random.Range (-4F, 4F);
				float maxMove = Random.Range (-7F, 7F);
				//rigidbody.transform.Rotate(Mathf.Lerp(minMove, maxMove, Time.time), Mathf.Lerp(minMove, maxMove, Time.time), 0);
				if(tr)
				{
					//transform.Rotate(Mathf.Lerp(minMove, maxMove, Time.time), 0, 0);
					transform.Rotate( 0, maxMove, 0);
					tr = false;
				}
			}
			else
			{
				tr = true;
				sensitivityX = 15F;
				sensitivityY = 15F;
			}
		} 
		// end
		
		if (axes == RotationAxes.MouseXAndY)
		{
			float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
		}
		else if (axes == RotationAxes.MouseX)
		{
			transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
		}
		else
		{
			rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
			
			transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
		}
	}
	
	void Start ()
	{
		// Make the rigid body not change rotation
		if (rigidbody)
			rigidbody.freezeRotation = true;
		
		cameraOverlay = GameObject.Find ("cameraOverlay");
	}
}