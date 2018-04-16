using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystemAJP;

public class GameStart: GameEvent {}

public class TitleBot : MonoBehaviour {

	public int myInt;
	TitleScreen ts;

	// Use this for initialization
	void Start () 
	{
		ts = (TitleScreen)FindObjectOfType (typeof(TitleScreen)); 
		EventManager.instance.Register<GameStart> (getOut);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (transform.position.x > 0) {
			GetComponent<SpriteRenderer> ().flipX = false;
		}
	}

	public void die()
	{
		TitleScreen.takenSlots.Remove (myInt);
		Destroy (this.gameObject);
	}

	public void getOut(GameEvent e)
	{
		EnemyDeath powerUpEvent = e as EnemyDeath;
		EventManager.instance.Unregister<GameStart> (getOut);
		Destroy (this.gameObject);
	}
}
