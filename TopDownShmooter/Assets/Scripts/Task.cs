using System;
using System.Diagnostics;

public abstract class Task 
{
	public enum TaskStatus : byte
	{
		Detached, // Task has not been attached to a TaskManager
		Pending, // Task has not been initialized
		Working, // Task has been initialized
		Success, // Task completed successfully
		Fail, // Task completed unsuccessfully
		Aborted // Task was aborted
	}

	public TaskStatus Status {get; private set;}

	protected virtual void Init (){}
	internal virtual void TaskUpdate(){}
	protected virtual void CleanUp(){}
	protected virtual void OnAbort (){}
	protected virtual void OnSuccess (){}
	protected virtual void OnFailure (){}

	internal void SetStatus (TaskStatus newStatus)
	{
		if (Status == newStatus) return;

		Status = newStatus;

		switch (newStatus)
		{
		case TaskStatus.Working:
			Init();
			break;
		case TaskStatus.Success:
			OnSuccess();
			CleanUp();
			break;

		case TaskStatus.Aborted:
			OnAbort();
			CleanUp();
			break;

		case TaskStatus.Fail:
			OnFailure();
			CleanUp();
			break;
		case TaskStatus.Detached:
		case TaskStatus.Pending:
			break;
		default:
			throw new ArgumentOutOfRangeException(newStatus.ToString(), newStatus, null);
		}
	}

	public void Abort()
	{
		SetStatus (TaskStatus.Aborted);
	}

	public bool isDetached {get { return Status == TaskStatus.Detached;}}
	public bool isAttached { get { return Status != TaskStatus.Detached;}}
	public bool isPending {get { return Status == TaskStatus.Pending; }}
	public bool isWorking {get { return Status == TaskStatus.Working; }}
	public bool isSuccessful{get { return Status == TaskStatus.Success;}}
	public bool hasFailed {get { return Status == TaskStatus.Fail;}}
	public bool isAborted {get { return Status == TaskStatus.Aborted;}}
	public bool isFinished { get { return Status == TaskStatus.Success || Status == TaskStatus.Fail;}} 

	public Task NextTask { get; private set; }
	public Task Then(Task task)
	{
		Debug.Assert(!task.isAttached);
		NextTask = task;
		return task;
	}

}