using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleFinger : MonoBehaviour {

	public Vector3 trajectory; 
	public Vector3 lookTarget; 
	public Vector3 currentTarget; 
	public Vector3 nextTarget;
	public float turnSpeed; 
	public float currentTime;
	public float timeBetweenShots;
	public bool hasShot;
	private FSM<TitleFinger> fsm;
	public List<Transform> positions = new List<Transform> ();

	// Use this for initialization
	void Start ()
	{
		fsm = new FSM<TitleFinger> (this); 
		int rando = Random.Range (0, positions.Count);
		currentTarget = positions [rando].position;
		fsm.TransitionTo<Searching> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		fsm.Update ();
		trajectory = (lookTarget - transform.position).normalized;
		if (trajectory != Vector3.zero) {
			float angle = Mathf.Atan2 (trajectory.y, trajectory.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
		}
	}

	public void turnHead ()
	{
		StartCoroutine (turn ());
	}

	IEnumerator turn ()
	{ 
		float currentTime = 0.0f;
		do
		{
			lookTarget = Vector3.Lerp(currentTarget, nextTarget, currentTime / turnSpeed); 
			currentTime += Time.deltaTime;
			yield return null;
		} 
		while (currentTime <= turnSpeed); 
		((TitleFingerState)fsm.CurrentState).reachTarget ();

	}

	public void shoot ()
	{
		GameObject bullet = Instantiate (Resources.Load ("Prefabs/fingerBullet")) as GameObject;
		bullet.transform.position = this.transform.position;
		BulletProperties bp = bullet.GetComponent<BulletProperties> ();
		bp.startPos = transform.position;
		bp.direction = new Vector3 (trajectory.x, trajectory.y, 0).normalized;
	}

	private class TitleFingerState : FSM<TitleFinger>.State 
	{
		public virtual void reachTarget (){}
	}

	private class Searching : TitleFingerState 
	{
		public override void OnEnter ()
		{
			int rando = Random.Range (0, TitleScreen.takenSlots.Count); 
			int index = TitleScreen.takenSlots [rando];
			Context.nextTarget = Context.positions [index].position;
			Context.turnHead ();
		}

		public override void reachTarget ()
		{
			Context.currentTarget = Context.lookTarget;
			TransitionTo<Shooting> ();
		}
	}

	private class Shooting : TitleFingerState
	{
		public override void OnEnter ()
		{
			Context.hasShot = false;
		}
		public override void Update ()
		{
			Context.currentTime += Time.deltaTime;
			if (Context.currentTime >= Context.timeBetweenShots && !Context.hasShot) 
			{
				Context.hasShot = true;
				Context.currentTime = 0;
				Context.shoot ();
			}
			if (Context.hasShot) 
			{
				if (Context.currentTime >= Context.timeBetweenShots) 
				{
					TransitionTo<Searching> ();
				}
			}
		}
	}
}
