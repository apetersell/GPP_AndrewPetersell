using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class SkyBot : Enemy {

	private Tree<SkyBot> tree; 
	public float seekSpeed;
	public float attackSpeed;
	public float fleeSpeed;
	public Vector3 attackDir;
	public float sightDistance;
	public float prepTime;
	public Vector3 targetPos;
	public float strikeTimer;
	public bool prepping;

	// Use this for initialization
	void Start () 
	{
		randomSpawn ();
		findPlayer ();
		spawnSettle ();
		tree = new Tree <SkyBot> (
			new Selector<SkyBot> (
				//Flee
				new Sequence <SkyBot> (
					new IsPrepping (),
					new IsHit (),
					new Flee ()
				),
				//Attack 
				new Sequence <SkyBot> (
					new IsPrepping (),
					new FullyPrepped (),
					new Attack ()
				),
				//Prep
				new Sequence <SkyBot> (
//					new Seek (), 
					new IsInRange (),
					new Prep ()
				),
				//Seek
				new Seek ()
			));
	}

	public override void fire () {}

	public override void updateFuctions ()
	{
		tree.Update (this);
		handleHitStun ();  
		handleShotDelay (); 
		handleDirection (); 
		handleDeath (); 
		handleGracePeriod (); 
		move (); 
	}

	public override void move ()
	{

	}

	public void tickUpStrikeTimer ()
	{
		strikeTimer += Time.deltaTime;
	}

	public override void takeDamage (float damage, float hitstun)
	{
		base.takeDamage (damage, hitstun);
	}

	public void seekPlayer (Vector3 target) 
	{
		transform.position += new Vector3 (dirX, dirY, 0); 
		if ((dirX < 0 && transform.position.x < target.x) || (dirX > 0 && transform.position.x > target.x))  
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
				changeDirection("x");
			}
		}

		if ((dirY < 0 && transform.position.y < target.y) || (dirY > 0 && transform.position.y > target.y))  
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
				changeDirection("y");
			}
		}
	}

	public void moveInStraightLine (Vector3 target)
	{
		Vector3 direction = target - transform.position;
		Vector3 start = transform.position; 
		transform.position = Vector3.Lerp (start, target, 0.25f);
	}

	//Behavior Tree Conditions
	private class IsInRange : Node<SkyBot>
	{
		public override bool Update (SkyBot bot)
		{
			float distToTarget = Vector3.Distance (bot.transform.position, bot.player.transform.position); 
			return distToTarget <= bot.sightDistance;
		}
	}

	private class HasReachedTarget : Node <SkyBot>
	{
		public override bool Update (SkyBot bot)
		{
			return bot.transform.position == bot.targetPos;
		}
	}

	private class IsHit : Node <SkyBot>
	{
		public override bool Update (SkyBot bot)
		{
			return bot.hit; 
		}
	}

	private class IsPrepping : Node <SkyBot>
	{
		public override bool Update (SkyBot bot)
		{
			return bot.prepping;
		}
	}

	private class FullyPrepped : Node <SkyBot>
	{
		public override bool Update (SkyBot bot)
		{
			return bot.strikeTimer >= bot.prepTime;
		}
	}

	//Behavior Tree Actions
	private class Attack: Node<SkyBot>
	{
		public override bool Update (SkyBot bot)
		{
			bot.prepping = false;
			bot.strikeTimer = 0;
			bot.topSpeed = bot.attackSpeed;
			bot.targetPos = bot.attackDir;
			bot.moveInStraightLine (bot.attackDir);
			Debug.Log ("Attacking");
			return true;
		}
	}

	private class Prep: Node <SkyBot>
	{
		public override bool Update (SkyBot bot)
		{
			bot.prepping = true;
			bot.attackDir = bot.player.transform.position; 
			bot.tickUpStrikeTimer ();
			bot.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp (Color.white, Color.red, (bot.strikeTimer/bot.prepTime)); 
			Debug.Log ("Prepping");
			return true;
		}
	}

	private class Flee: Node <SkyBot>
	{
		Vector3 fleeDir;
		public override bool Update (SkyBot bot)
		{
			bot.prepping = false;
			bot.strikeTimer = 0;
			bot.topSpeed = bot.fleeSpeed;
			fleeDir = -bot.attackDir;
			bot.targetPos = fleeDir;
			bot.gameObject.GetComponent<SpriteRenderer> ().color = Color.blue;
			bot.moveInStraightLine (fleeDir);
			Debug.Log ("Fleeing");
			return true;
		}
	}

	private class Seek: Node <SkyBot>
	{
		float distToTarget;
		public override bool Update (SkyBot bot)
		{
			bot.prepping = false;
			bot.topSpeed = bot.seekSpeed; 
			distToTarget = Vector3.Distance (bot.transform.position, bot.player.transform.position);   
			bot.seekPlayer (bot.player.transform.position);
			Debug.Log ("Seeking");
			return true;
		}
	}

}
