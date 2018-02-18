using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventSystemAJP;

public class EnemyDeath: GameEvent 
{

}
	
public abstract class Enemy: MonoBehaviour
{
	public float HP;
	public float acceleration;
	public float decceleration;
	public float topSpeed;
	public float fireRate;
	public float contactDamage;
	public AudioClip deathSound;
	public float startX;
	public float startY;
	protected float shotTimer;
	protected bool hit;
	protected float hitStun;
	protected float hitTimer;
	protected float xSpeed;
	protected float ySpeed;
	protected float dirX;
	protected float dirY;
	protected int directionModX;
	protected int directionModY;
	protected float gracePeriod;
	public GameObject player;
	public bool markedForDeath;

	public abstract void updateFuctions ();
	public abstract void fire ();
	public abstract void move ();

	protected void findPlayer()
	{
		player = GameObject.Find ("Player");
	}

	protected void randomSpawn()
	{
		float initX = Random.Range (-startX, startX);
		float initY = Random.Range (-startY, startY);
		transform.position = new Vector3(initX, initY, 0);
	}

	public virtual void handleDeath()
	{
		if (HP <= 0) 
		{
			die ();
		}
	}

	protected void spawnSettle ()
	{
		if (transform.position.x >= 0) {
			directionModX = -1;
		} else {
			directionModX = 1;
		}

		if (transform.position.y >= 0) {
			directionModY = -1;
		} else {
			directionModY = 1;
		}
	}
		
	protected void handleDirection()
	{
		dirX = xSpeed * directionModX;
		dirY = ySpeed * directionModY;
		if (directionModX >= 0) {
			GetComponent<SpriteRenderer> ().flipX = true;
		} else {
			GetComponent<SpriteRenderer> ().flipX = false;
		}

		if (xSpeed <= 0) 
		{
			xSpeed = 0;
		}
		if (ySpeed <= 0) 
		{
			ySpeed = 0;
		}
	}

	protected void handleHitStun()
	{
		if (hit) {
			GetComponent<SpriteRenderer> ().color = Color.red;
			hitTimer++;
		} else {
			GetComponent<SpriteRenderer> ().color = Color.white;
		}
		if (hitTimer >= hitStun) 
		{
			hit = false;
		}
	}

	public void handleShotDelay()
	{
		if (!hit) 
		{
			shotTimer += Time.deltaTime;
		}
		if (shotTimer >= fireRate) 
		{
			shotTimer = 0;
			fire ();
		}
	}

	protected void changeDirection(string sent)
	{
		if (sent == "x") 
		{
			directionModX = directionModX * -1;
		}
		if (sent == "y") 
		{
			directionModY = directionModY * -1;
		}
	}

	protected void accelerate (string sent)
	{
		if (sent == "x") 
		{
			if (xSpeed < topSpeed) {
				xSpeed += acceleration; 
			}
		}
		if (sent == "y") {
			if (ySpeed < topSpeed) {
				ySpeed += acceleration; 
			}
		}
	}
		
	protected void deccerlate (string sent)
	{
		if (sent == "x") 
		{
			if (xSpeed > 0) {
				xSpeed -= decceleration; 
			}
		}
		if (sent == "y") {
			if (ySpeed > 0) {
				ySpeed -= decceleration; 
			}
		}
	}

	public void takeDamage (float damage, float hitstun)
	{
		HP -= damage;
		hit = true;
		hitStun = hitstun;
	}

	protected void die ()
	{
		markedForDeath = true;
	}

	protected void handleGracePeriod()
	{
		gracePeriod--;
	}

}
