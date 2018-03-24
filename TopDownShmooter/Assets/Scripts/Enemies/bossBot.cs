using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossPhase 
{
	Growing,
	Spawning,
	Shooting,
	Chasing
}

public class bossBot : Enemy 
{

	private readonly TaskManager tm = new TaskManager();
	public Vector3 startScale;
	public Vector3 fullScale;
	List <Enemy> minions = new List<Enemy> ();
	public BossPhase currentPhase;
	float spawnTimer;
	public float spawnTimerMax;
	public int enemiesPerWave = 5;
	int enemiesInWave;
	float maxHP;
	public int phaseNum;

	void Awake ()
	{
		currentPhase = BossPhase.Growing;
		maxHP = HP;
		fullScale = this.transform.localScale;
		startScale = fullScale/100;
	}

	// Use this for initialization
	void Start () 
	{
		findPlayer ();
		spawnSettle ();
		tm.Do (new SetPos (this.gameObject, Vector3.zero))
			.Then (new ChangeScaleOverTime (this.gameObject, startScale, fullScale, 1f))
			.Then (new AdvanceBossPhase (this));
	}
	
	public override void updateFuctions ()
	{
		phaseChanger ();
		tm.Update ();
		handleHitStun ();
		handleDirection ();
		handleDeath ();
		handleGracePeriod ();
		for (int i = 0; i < minions.Count; i++) 
		{
			minions [i].updateFuctions ();
			if (minions [i].markedForDeath) 
			{
				Enemy choppingBlock = minions [i];
				minions.Remove (choppingBlock);
				Destroy (choppingBlock.gameObject);
			}
		}
		phaseStep ();
	}

	public override void fire ()
	{
		float randoX = Random.Range (-startX, startX);
		float randoY = Random.Range (-startY, startY);
		Vector3 randoPos = new Vector3 (randoX, randoY, 0);
		GameObject bullet = Instantiate (Resources.Load ("Prefabs/pellet")) as GameObject;
		bullet.transform.position = this.transform.position;
		BulletProperties bp = bullet.GetComponent<BulletProperties> ();
		bp.startPos = transform.position;
		bp.direction = randoPos - transform.position;
	}

	public override void move ()
	{
		Debug.Log ("BossMove");
		transform.position += new Vector3 (dirX, dirY, 0);
		if ((dirX < 0 && transform.position.x < player.transform.position.x) || (dirX > 0 && transform.position.x > player.transform.position.x)) 
		{
			deccerlate ("x");
		} else 
		{
			accelerate ("x");
		}
		if (dirX == 0) 
		{
			if (gracePeriod < 0) 
			{
				changeDirection ("x");
			}
		}

		if ((dirY < 0 && transform.position.y < player.transform.position.y) || (dirY > 0 && transform.position.y > player.transform.position.y)) 
		{
			deccerlate ("y");
		} else 
		{
			accelerate ("y");
		}
		if (dirY == 0) 
		{
			if (gracePeriod < 0) 
			{
				changeDirection ("y");
			}
		}
	}

	public override void die ()
	{
		for (int i = 0; i < minions.Count; i++) 
		{
			Destroy (minions [i].gameObject);
		}
		base.die ();
	}

	void phaseStep()
	{
		switch(currentPhase){
		case BossPhase.Growing:
			break;
		case BossPhase.Spawning:
			minionSpawning ();
			break;
		case BossPhase.Shooting:
			handleShotDelay ();
			break;
		case BossPhase.Chasing:
			minionSpawning ();
			move ();
			break;
		}
	}

	void spawnMinion ()
	{
		GameObject enemy = Instantiate (Resources.Load ("Prefabs/redBot")) as GameObject;
		enemy.GetComponent<Enemy> ().minion = true;
		enemy.transform.position = this.transform.position;
		minions.Add(enemy.GetComponent<Enemy> ());
	}

	void minionSpawning ()
	{
		if (enemiesInWave != enemiesPerWave)
		{
			spawnTimer += Time.deltaTime;
			if (spawnTimer > spawnTimerMax) 
			{
				spawnTimer = 0;
				spawnMinion ();
				enemiesInWave++;
			}
		}

		if (minions.Count == 0 && enemiesInWave != 0) 
		{
			enemiesInWave = 0;
		}
	}

	void phaseChanger ()
	{
		if (HP <= maxHP * 0.5f && phaseNum < 2) {
			tm.Do (new AdvanceBossPhase (this));
		}

		if (HP <= maxHP * 0.15f && phaseNum < 3) {
			tm.Do (new AdvanceBossPhase (this));
		}
		
	}
}