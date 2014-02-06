using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	public System.Action OnPressed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( Input.GetMouseButtonDown(0)){
			RaycastHit hit = new RaycastHit();
			Ray ray  = Camera.main.ScreenPointToRay (Input.mousePosition);
			if(collider.Raycast(ray, out hit, Mathf.Infinity))
			{
				if(OnPressed != null)
				{
					OnPressed();
				}
			}
		}
	}
}
