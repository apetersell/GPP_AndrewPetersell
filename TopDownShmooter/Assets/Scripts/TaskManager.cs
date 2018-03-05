using System.Collections.Generic;
using UnityEngine;

public class TaskManager
{
	public List <Task> TaskList = new List<Task>();

	public Task Do (Task sent)
	{
		Debug.Assert (sent != null);
		Debug.Assert (!sent.isAttached);
		TaskList.Add(sent);
		sent.SetStatus (Task.TaskStatus.Pending);
		return sent;

	}
	
	// Update is called once per frame
	public void Update () 
	{
		for (int i = TaskList.Count; i >= 0; i--) 
		{
			Task t = TaskList [i];

			if (t.isPending) 
			{
				t.SetStatus (Task.TaskStatus.Working);
			}

			if (t.isFinished) {
				HandleCompletion (t, i);
			}
			else 
			{
				t.TaskUpdate ();
			}
		}
	}

	private void HandleCompletion (Task task, int taskIndex)
	{
		if (task.NextTask != null && task.isSuccessful) 
		{
			Do (task.NextTask);
		}
		TaskList.RemoveAt (taskIndex);
		task.SetStatus (Task.TaskStatus.Detached);
	}
}
