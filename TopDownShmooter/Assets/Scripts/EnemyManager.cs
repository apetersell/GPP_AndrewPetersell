﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventSystemAJP;

public class NewWave : GameEvent {};

public class EnemyManager : MonoBehaviour {

	public int waveCounter = 0;
	public List<Enemy> activeWave = new List<Enemy>();
	public List<string> waveList = new List<string> ();
	public string[] enemyTypes;
	public int numOfEnemyTypes; 
	public float minTimeBetweenSpawns;
	public float maxTimeBetweenSpawns;
	public float timeBetweenWaves;
	float waveTimer;
	float spawnTimer;
	float spawnTimerMax;
	public Text counterDisplay;
	bool playedSound;
	public AudioClip waveCompleteSound; 
	public List<int> bossWave = new List<int>();


	public void commenceAttack ()
	{
		makeNewWaveList ();
		EventManager.instance.Register<GameOver> (GameOver);
//		counterDisplay = GameObject.Find ("WaveCounter").GetComponent<Text> ();
	}
		
	public void enemyUpdates ()
	{
		if (activeWave.Count != 0) 
		{
			for (int i = 0; i < activeWave.Count; i++) 
			{
				activeWave[i].updateFuctions ();
				if (activeWave[i].markedForDeath) {
					destroyEnemy (activeWave[i]);
				}
			}
//			foreach (var enemy in activeWave) {
//				enemy.updateFuctions ();
//				if (enemy.markedForDeath) {
//					destroyEnemy (enemy);
//				}
//			}
		}

		waveOperations ();
	}
	void createEnemy(string sent)
	{
		string path = "Prefabs/" + sent;
		GameObject enemy = Instantiate (Resources.Load (path)) as GameObject;
		activeWave.Add(enemy.GetComponent<Enemy> ());
	}

	void destroyEnemy (Enemy e)
	{
		activeWave.Remove (e);
		Destroy (e.gameObject);
		EventManager.instance.Fire (new EnemyDeath()); 
	}

	void makeNewWaveList()
	{
		EventManager.instance.Fire (new NewWave());
		waveCounter++;
		playedSound = false;
		spawnTimerMax = Random.Range (minTimeBetweenSpawns, maxTimeBetweenSpawns);
		if (!bossWave.Contains (waveCounter)) {
			for (int i = 0; i < waveCounter; i++) {
				int rando = Random.Range (0, enemyTypes.Length);
				string type = enemyTypes [rando];
				waveList.Add (type);
			}
		} else {
			waveList.Add ("bossBot");
		}
	}

	void waveOperations ()
	{
		if (!bossWave.Contains (waveCounter)) {
			counterDisplay.text = "Wave: " + waveCounter;
		} else {
			counterDisplay.text = "Wave: BOSS!!!";
		}
		spawnTimer += Time.deltaTime;
		if (spawnTimer > spawnTimerMax) 
		{
			if (waveList.Count != 0) {
				int rando = Random.Range (0, waveList.Count);
				createEnemy (waveList [rando]);
				waveList.Remove (waveList [rando]);
				spawnTimerMax = Random.Range (minTimeBetweenSpawns, maxTimeBetweenSpawns);
				spawnTimer = 0;
			}
		}

		if (waveList.Count == 0 && activeWave.Count == 0) 
		{
			if (!playedSound) 
			{
				GetComponent<AudioSource> ().PlayOneShot (waveCompleteSound);
				playedSound = true;
			}
			waveTimer += Time.deltaTime;
		}

		if (waveTimer > timeBetweenWaves) 
		{
			makeNewWaveList ();
			waveTimer = 0;
		}
	}

	public void GameOver (GameEvent e)
	{
		Debug.Log ("CLEAN HOUSE");
		GameOver powerUpEvent = e as GameOver;
		for (int i = 0; i < activeWave.Count; i++) 
		{
			Destroy (activeWave [i].gameObject);
		}
		EventManager.instance.Unregister<GameOver> (GameOver);
	}
}
