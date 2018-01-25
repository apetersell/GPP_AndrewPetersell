using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProperties : MonoBehaviour 
{
	public float speed;
	public Vector3 direction; 
	public Vector3 startPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += new Vector3 (direction.x * speed, direction.y * speed, 0);
		Vector3 trajectory = direction;
		if (trajectory != Vector3.zero) {
			float angle = Mathf.Atan2 (trajectory.y, trajectory.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		}
	}
}
