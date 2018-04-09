using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpBot : Enemy 
{
	private FSM<PurpBot> fsm;
	public float seekSpeed;
	public float attackSpeed;
	public float fleeSpeed;
	public Vector3 attackDir;
	public float sightDistance;
	public float prepTime;
	public Vector3 targetPos;

	// Use this for initialization
	void Start () 
	{
		fsm = new FSM<PurpBot>(this); 
		fsm.TransitionTo<PurpBotSeeking> ();
		randomSpawn ();
		findPlayer ();
		spawnSettle ();
	}

	public override void fire () {}

	public override void updateFuctions ()
	{
		handleHitStun ();  
		handleShotDelay (); 
		handleDirection (); 
		handleDeath (); 
		handleGracePeriod (); 
		fsm.Update ();
		move (); 

		if (transform.position == targetPos)
		{
			((PurpBotState)fsm.CurrentState).reachTarget ();
		}
	}

	public override void move ()
	{
		((PurpBotState)fsm.CurrentState).stateBasedMovement (); 
	}

	public override void takeDamage (float damage, float hitstun)
	{
		((PurpBotState)fsm.CurrentState).stateBasedTakeDamage ();
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
		

	//States
	private class PurpBotState : FSM<PurpBot>.State 
	{
		public virtual void stateBasedMovement () {}
		public virtual void stateBasedTakeDamage () {}
		public virtual void reachTarget () {}
	}

	private class PurpBotSeeking : PurpBotState
	{
		public override void OnEnter ()
		{
			Context.topSpeed = Context.seekSpeed;
		}
		public override void Update ()
		{
			float distToTarget = Vector3.Distance (Context.transform.position, Context.player.transform.position); 
			if (distToTarget <= Context.sightDistance) 
			{
				TransitionTo<PurpBotPrepping> ();
			}
		}
		public override void stateBasedMovement ()
		{
			Context.seekPlayer (Context.player.transform.position);
		}
	}

	private class PurpBotPrepping : PurpBotState
	{
		float timer;
		public override void OnEnter ()
		{
			timer = 0;
		}
		public override void Update ()
		{
			Context.attackDir = Context.player.transform.position; 
			timer += Time.deltaTime;
			Context.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp (Color.white, Color.red, (timer/Context.prepTime)); 
			if (timer > Context.prepTime) 
			{
				TransitionTo<PurpBotAttacking> ();
			}

		}
		public override void stateBasedTakeDamage ()
		{
			TransitionTo<PurpBotFleeing> (); 
		}
	}

	private class PurpBotAttacking : PurpBotState
	{
		public override void OnEnter ()
		{
			Context.topSpeed = Context.attackSpeed;
			Context.targetPos = Context.attackDir;
		}
		public override void stateBasedMovement ()
		{
			Context.moveInStraightLine (Context.attackDir);
		}

		public override void reachTarget ()
		{
			TransitionTo<PurpBotSeeking> ();
		}
	}

	private class PurpBotFleeing : PurpBotState
	{
		Vector3 fleeDir;
		public override void OnEnter ()
		{
			Context.topSpeed = Context.fleeSpeed;
			fleeDir = -Context.attackDir;
			Context.targetPos = fleeDir;
			Context.gameObject.GetComponent<SpriteRenderer> ().color = Color.blue;
		}

		public override void stateBasedMovement ()
		{
			Context.moveInStraightLine (fleeDir);
		}

		public override void reachTarget ()
		{
			TransitionTo<PurpBotSeeking> ();
		}
	}
}
