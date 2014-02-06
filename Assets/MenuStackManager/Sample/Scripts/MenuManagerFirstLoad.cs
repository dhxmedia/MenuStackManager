using UnityEngine;
using System.Collections;
using MenuStackManager;
public class MenuManagerFirstLoad : MonoBehaviour {
	
	public GameObject testPrefab;
	private Manager _manager;

	// Use this for initialization
	
	void Start () {
		Application.targetFrameRate = 60;
		_manager = GetComponent<MenuStackManager.Manager>();
		_manager.PushMenu(testPrefab, null, null, null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
