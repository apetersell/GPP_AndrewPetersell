using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	EnemyManager enemyM;

	// Use this for initialization
	void Start () 
	{
		enemyM = GetComponent<EnemyManager> ();
		enemyM.commenceAttack ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		enemyM.enemyUpdates ();
	}
}
