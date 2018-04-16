using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using EventSystemAJP;

public class ScoreManager : MonoBehaviour {

	public int score;
	public Text scoreDisplay;

	void Start () 
	{
		EventManager.instance.Register<EnemyDeath> (scoreUp);
	}
	
	// Update is called once per frame
	void Update () 
	{
		scoreDisplay.text = "Robots killed: " + score;
	}

	public void scoreUp (GameEvent e)
	{
		EnemyDeath scoreUpEvent = e as EnemyDeath;
		score++;
	}
}
