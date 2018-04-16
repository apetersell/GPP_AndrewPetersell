using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventSystemAJP;

public class GameOver: GameEvent {}

public class GameScene : Sceney<TransitionData> {

	public int score;
	public int waveScore;
	public Text scoreDisplay;
	public Image hpMeter;
	public Text hp;
	public float maxHP;
	public float currentHP;

	// Use this for initialization
	void Start () 
	{
		currentHP = maxHP;
		hpMeter = (Image)FindObjectOfType (typeof(Image)); 
	}
	
	// Update is called once per frame
	void Update () 
	{
		scoreDisplay.text = "Robots killed: " + score;
		hp.text = "HP: " + currentHP;
		hpMeter.fillAmount = currentHP / maxHP;

		if (currentHP <= 0) 
		{
			EventManager.instance.Fire (new GameOver());
			Services.Scenes.Swap<EndScreen> (new TransitionData (score, waveScore));
		}
	}

	internal override void OnEnter (TransitionData data)
	{
		base.OnEnter (data);
		EventManager.instance.Register<EnemyDeath> (scoreUp);
		EventManager.instance.Register<NewWave> (waveUp);
	}

	internal override void OnExit ()
	{
		EventManager.instance.Unregister<EnemyDeath> (scoreUp);
		EventManager.instance.Unregister<NewWave> (waveUp);
	}

	public void scoreUp (GameEvent e)
	{
		EnemyDeath scoreUpEvent = e as EnemyDeath;
		score++;
	}

	public void waveUp (GameEvent e)
	{
		NewWave waveUpEvent = e as NewWave;
		waveScore++; 
	}

	public void takeDamage(float damage)
	{
		currentHP -= damage;
	}
}
