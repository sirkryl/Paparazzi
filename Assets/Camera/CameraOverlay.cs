using System;
using UnityEngine;
using System.Collections;


public class CameraOverlay: MonoBehaviour
{
	private GameObject cameraOverlay;
	private GameObject intro1;
	private GameObject intro2;
	public AudioSource soundtrack;
	
	public void Start()
	{
		cameraOverlay = GameObject.Find ("cameraOverlay");
		intro1 = GameObject.Find ("intro1");
		intro2 = GameObject.Find ("intro2");
		cameraOverlay.SetActive(false);
		intro1.SetActive(false);
		intro2.SetActive(false);
	}
	
	public void OnEnable ()
	{
	}
	public void OnDisable ()
	{
	}
	
	public void Update()
	{
		if (Time.time <= 5)
		{
			if(!soundtrack.isPlaying)
			{
				soundtrack.Play ();
			}
			intro1.SetActive (true);
		}
		else if (Time.time <= 10)
		{
			intro1.SetActive (false);
			intro2.SetActive (true);
		}
		else 
		{
			intro2.SetActive (false);
			
		}
		if(Input.GetKeyDown (KeyCode.C))
		{
			cameraOverlay.SetActive(!cameraOverlay.activeSelf);
		}
	}
}
		


