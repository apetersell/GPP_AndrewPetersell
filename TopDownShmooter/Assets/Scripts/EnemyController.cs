using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		GetComponent<Enemy> ().startFuctions ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetComponent<Enemy> ().updateFuctions ();
	}
}
