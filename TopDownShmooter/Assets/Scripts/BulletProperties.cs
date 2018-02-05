using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletProperties : MonoBehaviour 
{
	public float speed;
	public Vector3 direction; 
	public Vector3 startPos;
	float lifeSpan;
	public float maxLifeSpan;
	public float damage;
	public float hitStun;
	public bool friendly;

	// Use this for initialization
	void Start () 
	{
		lifeSpan = maxLifeSpan;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position += new Vector3 (direction.x * speed, direction.y * speed, 0);
		Vector3 trajectory = direction;
		if (trajectory != Vector3.zero) 
		{
			float angle = Mathf.Atan2 (trajectory.y, trajectory.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		}
		lifeSpan-= Time.deltaTime;
		if (lifeSpan <= 0) 
		{
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (friendly) {
			Enemy e = coll.gameObject.GetComponent<Enemy> ();
			if (e != null) {
				Destroy (this.gameObject);
				e.takeDamage (damage, hitStun);
			}
		} else {
			Player p = coll.gameObject.GetComponent<Player> (); 
			if (p != null) {
				Destroy (this.gameObject); 
				p.stunned = true;
				p.stunTimer = damage;
			}
		}
	}
}
