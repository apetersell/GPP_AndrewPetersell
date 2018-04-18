using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystemAJP;

public class TitleScreen : Sceney<TransitionData> {
	
	public static List<int> takenSlots = new List<int> ();
	public List<Transform> positions = new List<Transform> ();
	public static bool gameStarting;

	// Use this for initialization
	void Start () 
	{
		gameStarting = false; 
		takenSlots.Clear ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			gameStarting = true;
			EventManager.instance.Fire (new GameStart ());
			Services.Scenes.Swap<GameScene> ();
		}

		if (takenSlots.Count <= 0 && !gameStarting) 
		{
			makeNewSet ();
		}
	}

	void makeNewSet ()
	{
		for (int i = 0; i < positions.Count; i++) 
		{
			GameObject newBot = Instantiate (Resources.Load ("Prefabs/titleBot")) as GameObject;
			newBot.transform.position = positions [i].position;
			takenSlots.Add (i);
			TitleBot tb = newBot.GetComponent<TitleBot> ();
			tb.myInt = i;
		}
	}
}
