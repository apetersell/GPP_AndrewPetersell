﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float maxSpeed;
	float xSpeed; 
	float ySpeed; 
	public float accerlation;
	public float decceleration;
	public Vector3 moveDir;
	public Vector3 lookTarget;
	public Transform crosshair;
	public float fireRate;
	public float currentFireRate;
	public Vector3 trajectory;
	public AudioClip pew;
	AudioSource auds;

	// Use this for initialization
	void Start () 
	{
		auds = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float xDir;
		float yDir;
		xDir = xSpeed * Input.GetAxis ("Horizontal");
		yDir = ySpeed * Input.GetAxis ("Vertical");
		lookTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
		Vector2 crosshairPlacement = new Vector2 (lookTarget.x, lookTarget.y);
		crosshair.position = crosshairPlacement;
		lookTarget.z = 0;
		moveDir = new Vector3 (xDir, yDir, 0);
		transform.position += moveDir;
		if (Input.GetAxis ("Horizontal") != 0) {
			if (xSpeed <= maxSpeed) {
				xSpeed += accerlation;
			}
		} else {
			if (xSpeed > 0) 
			{
				xSpeed -= decceleration;
			}
		}
		if (Input.GetAxis ("Vertical") != 0) {
			if (ySpeed <= maxSpeed) {
				ySpeed += accerlation;
			}
		} else {
			if (ySpeed > 0) 
			{
				ySpeed -= decceleration;
			}
		}

		trajectory = (lookTarget - transform.position).normalized;
		if (trajectory != Vector3.zero) {
			float angle = Mathf.Atan2 (trajectory.y, trajectory.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		}

		if (Input.GetMouseButton (0)) 
		{
			currentFireRate--;
			if (currentFireRate <= 0) {
				GameObject bullet = Instantiate (Resources.Load ("Prefabs/fingerBullet")) as GameObject;
				bullet.transform.position = this.transform.position;
				BulletProperties bp = bullet.GetComponent<BulletProperties> ();
				bp.startPos = transform.position;
				bp.direction = new Vector3 (trajectory.x, trajectory.y, 0).normalized;
				currentFireRate = fireRate;
				auds.PlayOneShot (pew);
			}
		}

		if (Input.GetMouseButtonUp (0)) 
		{
			currentFireRate = fireRate;
		}
	}
}
