using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : Sceney<TransitionData> {

	[SerializeField] private Text waves;
	[SerializeField] private Text score;

	// Use this for initialization
	void Start () {
		
	}

	internal override void OnEnter (TransitionData data)
	{
		waves.text = "You survived until wave " + data.wave;
		score.text = "You murdered " + data.score + " robots";
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			Services.Scenes.Swap<TitleScreen>();
		}
	}
}
