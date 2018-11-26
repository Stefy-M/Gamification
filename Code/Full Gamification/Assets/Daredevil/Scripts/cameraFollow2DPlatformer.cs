﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow2DPlatformer : MonoBehaviour {

	public Transform target;  // what the camera is following

	public float smoothing; // dampening effect

	Vector3 offset;
	float lowY;



	// Use this for initialization
	void Start () {
		offset = transform.position - target.position; // difference between target and camera
		lowY = transform.position.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 targetCamPos = target.position + offset;
		
		//slowly move camera to pos
		transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing*Time.deltaTime);

		//if (transform.position.y < lowY)
		//{
		//	transform.position = new Vector3(transform.position.x, lowY, transform.position.z);
		//}
	}
}