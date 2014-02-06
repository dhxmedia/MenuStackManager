using UnityEngine;
using System.Collections;

public class PopScript : MonoBehaviour {
	public MenuStackManager.Layer Layer;
	public Button SampleButton;
	// Use this for initialization
	void Start () {
		SampleButton.OnPressed += OnPressed;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnPressed()
	{
		Layer.RequestPop();
	}
}
