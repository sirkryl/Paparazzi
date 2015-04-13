using System;
using UnityEngine;
using System.Collections;

public class AheadAndBack : MonoBehaviour
{
    /* speed of orbit (in degrees/second) */
    public float speed;

    public void Update()
    {
		if(transform.position.z <= 15.7f)
			speed = +0.03f;
		else if(transform.position.z >= 18f) 
			speed = -0.03f;
		
		transform.position = new Vector3(transform.position.x,transform.position.y, transform.position.z + speed);
    }
}

