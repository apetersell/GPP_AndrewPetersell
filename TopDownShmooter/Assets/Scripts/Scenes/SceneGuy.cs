using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object; 

public class SceneGuy <TTransitionData>
{
	internal GameObject sceneRoot { get; private set;} 

	private readonly Dictionary<Type, GameObject> _scenes = new Dictionary<Type, GameObject>();

	public SceneGuy (GameObject root, IEnumerable<GameObject> scenePrefabs)
	{
		sceneRoot = root;
		foreach (var prefab in scenePrefabs)
		{
			var scene = prefab.GetComponent<Sceney<TTransitionData>>();
			Assert.IsNotNull(scene, "Could not find scene script in prefab used to initialize SceneManager");
			_scenes.Add(scene.GetType(), prefab);
		}
	}
	private readonly Stack<Sceney<TTransitionData>> _sceneStack = new Stack<Sceney<TTransitionData>>();

	public Sceney<TTransitionData> CurrentScene
	{
		get
		{
			return _sceneStack.Count != 0 ? _sceneStack.Peek() : null;
		}
	}

	public void PopScene(TTransitionData data = default (TTransitionData))
	{
		Sceney<TTransitionData> previousScene = null;
		Sceney<TTransitionData> nextScene = null;

		if (_sceneStack.Count != 0)
		{
			previousScene = _sceneStack.Peek();
			_sceneStack.Pop();
		}

		if (_sceneStack.Count != 0)
		{
			nextScene = _sceneStack.Peek();
		}

		if (nextScene != null)
		{
			nextScene._OnEnter(data);
		}

		if (previousScene != null)
		{
			Object.Destroy(previousScene.Root);
			previousScene._OnExit();
		}
	}
		
	public void PushScene<T>(TTransitionData data = default (TTransitionData)) where T : Sceney<TTransitionData>
	{
		var previousScene = CurrentScene;
		var nextScene = GetScene<T>();

		_sceneStack.Push(nextScene);
		nextScene._OnEnter(data);

		if (previousScene != null)
		{
			previousScene._OnExit();
			previousScene.Root.SetActive(false);
		}
	}
		
	public void Swap<T>(TTransitionData data = default (TTransitionData)) where T : Sceney<TTransitionData>
	{
		Sceney<TTransitionData> previousScene = null;
		if (_sceneStack.Count > 0)
		{
			previousScene = _sceneStack.Peek();
			_sceneStack.Pop();
		}

		var nextScene = GetScene<T>();
		_sceneStack.Push(nextScene);
		nextScene._OnEnter(data);

		if (previousScene != null)
		{
			previousScene._OnExit();
			Object.Destroy(previousScene.Root);
		}
	}
		
	private T GetScene<T>() where T : Sceney<TTransitionData>
	{
		GameObject prefab;
		_scenes.TryGetValue(typeof(T), out prefab);
		Assert.IsNotNull(prefab, "Could not find scene prefab for scene type: " + typeof(T).Name);

		var sceneObject = Object.Instantiate(prefab);
		sceneObject.name = typeof (T).Name;
		sceneObject.transform.SetParent(sceneRoot.transform, false);
		return sceneObject.GetComponent<T>();
	}
}
