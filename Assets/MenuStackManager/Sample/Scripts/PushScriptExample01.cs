using UnityEngine;
using System.Collections;

public class PushScriptExample01 : MonoBehaviour {
	public MenuStackManager.Layer Layer;
	public Button SampleButton;
	public GameObject Prefab;
	public MenuStackManager.StackAction PushAction;
	public MenuStackManager.StackAction PopAction;
	// Use this for initialization
	void Start () {
		SampleButton.OnPressed += OnPressed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnPressed()
	{
		Layer.RequestPush(Prefab, PushAction, PopAction, name);
	}
}
