using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueBot : Enemy 
{
	public override void startFuctions ()
	{
		randomSpawn ();
		findPlayer ();
		spawnSettle ();
	}

	public override void updateFuctions ()
	{
		handleHitStun ();
		handleShotDelay ();
		handleDirection ();
		handleDeath ();
		move ();
		handleGracePeriod ();
	}

	public override void fire ()
	{
		GameObject bullet = Instantiate (Resources.Load ("Prefabs/pellet")) as GameObject;
		bullet.transform.position = this.transform.position;
		BulletProperties bp = bullet.GetComponent<BulletProperties> ();
		bp.startPos = transform.position;
		bp.direction = player.transform.position - transform.position;
	}
		
	public override void move ()
	{
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
}
