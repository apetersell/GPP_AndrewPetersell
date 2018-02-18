using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCenter;

public class EnemyBehaviorEventExample : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		EventManager.instance.Register <PlayerPoweredUp> (OnPlayerPowerUp);
	}

	private void OnDestroy ()
	{
		EventManager.instance.Unregister<PlayerPoweredUp> (OnPlayerPowerUp);
	}

	private void OnPlayerPowerUp(GameEvent e)
	{
		PlayerPoweredUp powerUpEvent = e as PlayerPoweredUp;
		Debug.Log (powerUpEvent.Player.GetComponent<PlayerEventExample>().name);
	}
}
