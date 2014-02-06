using UnityEngine;
using System.Collections;

public class ExampleLayerArea : MenuStackManager.Layer {
	public TextMesh Text;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	public override void Init(object data)
	{
		if(data != null)
			Text.text = data.ToString();
	}
}
