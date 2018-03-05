using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Single discrete tasks.
public class ActionTask : Task 
{
	public Action Action { get; private set; }

	public ActionTask (Action action)
	{
		Action = action;
	}

	protected override void Init ()
	{
		Action ();
		SetStatus (TaskStatus.Success);
	}
}

//Tasks that take a time.
public abstract class TimedTask : Task
{
	public float Duration { get; private set; }
	public float StartTime { get; private set; }

	protected TimedTask (float duration)
	{
		Debug.Assert (duration > 0, "Cannot create a timed task with a duration of 0");
		Duration = duration;
	}

	protected override void Init ()
	{
		StartTime = Time.time;
	}

	internal override void TaskUpdate ()
	{
		var now = Time.time;
		var elapsed = now - StartTime;
		var t = Mathf.Clamp01 (elapsed / Duration);
		if (t >= 1) {
			OnElapsed ();
		} else {
			OnStep (t);
		}
	}

	protected virtual void OnStep (float t) {}

	protected virtual void OnElapsed ()
	{
		SetStatus (TaskStatus.Success); 
	}
}

//Make it wait.
public class Wait : TimedTask
{
	public Wait (float duration) : base(duration){}
}

public abstract class GOTask: Task
{
	protected readonly GameObject GO;

	protected GOTask (GameObject sentGO)
	{
		this.GO = sentGO; 
	}
}

public abstract class TimedGOTask: TimedTask
{
	protected readonly GameObject GO;

	protected TimedGOTask (GameObject sentGO, float duration) : base(duration)
	{
		this.GO = sentGO;
	}
}

public class SetPos : GOTask
{
	private readonly Vector3 _pos;
	public SetPos (GameObject sentGO, Vector3 pos) : base (sentGO)
	{
		_pos = pos; 
	}

	protected override void Init ()
	{
		GO.transform.position = _pos; 
		SetStatus (TaskStatus.Success);
	}
}

public class ChangeSpriteColorOverTime : TimedGOTask
{
	private readonly Color startColor;
	private readonly Color endColor;
	private SpriteRenderer sr;
	public ChangeSpriteColorOverTime (GameObject sentGO, Color start, Color end, float duration) : base(sentGO, duration) 
	{
		Debug.Assert (GO.GetComponent<SpriteRenderer> () != null, "Game object does not have SpriteRendere attached.");
		startColor = start;
		endColor = end;
		sr = GO.GetComponent<SpriteRenderer> ();
	}

	protected override void OnStep (float t)
	{
		sr.color = Color.Lerp (startColor, endColor, t);
	}
}