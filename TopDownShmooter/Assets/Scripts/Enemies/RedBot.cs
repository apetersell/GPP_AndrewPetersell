using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBot : Enemy 
{
	int dir;
	public float borderline; 

	public override void startFuctions ()
	{
		findPlayer ();
		randomSpawn ();
		spawnSettle ();
		randomDir ();
	}

	protected void randomDir()
	{
		dir = Random.Range (0, 2);
		Debug.Log (dir);
	}

	public override void updateFuctions ()
	{
		handleHitStun ();
		handleShotDelay ();
		handleDirection ();
		handleDeath ();
		handleGracePeriod ();
		move ();
	}

	public override void fire ()
	{
		GameObject bullet = Instantiate (Resources.Load ("Prefabs/pellet")) as GameObject;
		bullet.transform.position = this.transform.position;
		BulletProperties bp = bullet.GetComponent<BulletProperties> ();
		bp.startPos = transform.position;
		if (dir == 0) {
			bp.direction = new Vector3 (0, -1 * directionModX, 0);
		} else {
			bp.direction = new Vector3 (1 * directionModX, 0, 0);
		}
	}

	public override void move ()
	{
		transform.position += new Vector3 (dirX, dirY, 0);
		if (dir == 0) 
		{
			if ((dirX < 0 && transform.position.x <= -startX + borderline) || (dirX > 0 && transform.position.x >= startX - borderline)) {
				deccerlate ("x");
			} else {
				accelerate ("x");
			}

			if (dirX == 0) {
				if (gracePeriod < 0) 
				{
					changeDirection ("x");
				}
			}
		}

		if (dir == 1) 
		{
			if ((dirY < 0 && transform.position.y <= -startY + borderline) || (dirY > 0 && transform.position.y >= startY - borderline)) {
				deccerlate ("y");
			} else {
				accelerate ("y");
			}

			if (dirY == 0) {
				if (gracePeriod < 0) 
				{
					changeDirection ("y");
				}
			}

			if (player.transform.position.x > transform.position.x && directionModX < 0) 
			{
				changeDirection ("x");
			}

			if (player.transform.position.x < transform.position.x && directionModX > 0) 
			{
				changeDirection("x");
			}
		}


	}
}
