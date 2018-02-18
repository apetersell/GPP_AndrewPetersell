using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCenter;

public class PlayerPoweredUp : GameEvent 
{
	public readonly GameObject Player;

	public PlayerPoweredUp(GameObject player)
	{
		Player = player;
	}
}


public class PlayerEventExample : MonoBehaviour {

	public string name = "Player";

	// Use this for initialization
	void Start () 
	{
		StartCoroutine (PowerUp());
	}

	IEnumerator PowerUp ()
	{
		yield return new WaitForSeconds (1);
		EventManager.instance.Fire (new PlayerPoweredUp (gameObject));
	}
}
