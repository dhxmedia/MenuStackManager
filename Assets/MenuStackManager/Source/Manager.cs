/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace MenuStackManager
{
	public class Manager : MonoBehaviour {
		
		private List<GameObject> menuStack = new List<GameObject>();
		private List<StackAction> menuActionStack = new List<StackAction>();
		
		public delegate void OnMenuPushedDelegate(GameObject pushed, GameObject top, GameObject bottom);
		public OnMenuPushedDelegate OnMenuPushed {get; set;}
		public delegate void OnMenuPoppedDelegate(GameObject pushed, GameObject top, GameObject bottom);
		public OnMenuPoppedDelegate OnMenuPopped {get; set;}
		
		bool _busy = false;

		
		HashSet<GameObject> _requestingObjects = new HashSet<GameObject>();
		Queue<IEnumerator> _actions = new Queue<IEnumerator>();
		
		IEnumerator _PushMenu(GameObject prefab, StackAction pushAction, StackAction popAction, object data)
		{
			GameObject newMenuObject = Instantiate(prefab) as GameObject;
			newMenuObject.GetComponent<Layer>().Init(data);
			newMenuObject.GetComponent<Layer>().RequestMenuPush += RequestPush;
			newMenuObject.GetComponent<Layer>().RequestMenuPop += RequestPop;
			GameObject gObj = new GameObject();
			gObj.transform.parent = transform;
			gObj.transform.localPosition = Vector3.zero;
			gObj.name = "LayerParent";
			newMenuObject.transform.parent = gObj.transform;
			newMenuObject.name = newMenuObject.name.Remove(newMenuObject.name.Length-7,7); //get rid of (Clone) on the names
			GameObject top = null;
			GameObject bottom = null;
			if(menuStack.Count > 0)
			{
				top = menuStack[0];
				bottom = menuStack[menuStack.Count - 1];
			}	
			menuStack.Add(newMenuObject);
			if(popAction != null)
				menuActionStack.Add(popAction);
			
			
			if(top != null)
			{
				top.transform.parent.localPosition += new Vector3(0, 0, newMenuObject.GetComponent<Layer>().Bound.size.z + 1);
			}
			
			if(pushAction != null)
				yield return newMenuObject.GetComponent<Layer>().OnCreate(pushAction.Action(null));
			else	
				yield return newMenuObject.GetComponent<Layer>().OnCreate(null);
			
			if(OnMenuPushed != null)
			{
				OnMenuPushed(newMenuObject, top, bottom);
			}
			
			if(bottom != null)
			{
				gObj.transform.parent = bottom.transform;
			}
			
			_requestingObjects.Remove(bottom);
			_busy = false;
			yield return 0;
		}
		
		IEnumerator _SideLoad(GameObject prefab, StackAction action)
		{
			yield return 0;
		}
		
		// Add the menu to the current menu stack
		// Makes everything below it lower priority
		public void PushMenu(GameObject prefab, StackAction pushAction, StackAction popAction, object data)
		{

			if(menuStack.Count > 0)
			{
				GameObject bottom = menuStack[menuStack.Count - 1];
				if(_requestingObjects.Contains(bottom) == false)
				{
					_requestingObjects.Add(bottom);
					_actions.Enqueue(this._PushMenu(prefab, pushAction, popAction, data));
				}
			}	
			else
			{
				_actions.Enqueue(this._PushMenu(prefab, pushAction, popAction, data));
			}
			
		}
		
		IEnumerator _PopMenu()
		{
			if(menuStack.Count > 0)
			{
				GameObject bottom = menuStack[menuStack.Count - 1];
				menuStack.RemoveAt(menuStack.Count - 1);
				bottom.transform.parent.parent = transform;
				
				if(menuActionStack.Count > 0 && menuActionStack[menuActionStack.Count - 1] != null)
					yield return bottom.GetComponent<Layer>().OnLayerDestroy(menuActionStack[menuActionStack.Count - 1].Action(null));
				else
					yield return bottom.GetComponent<Layer>().OnLayerDestroy(null);

				if(menuActionStack.Count > 0)
					menuActionStack.RemoveAt(menuActionStack.Count - 1);
				
				GameObject top = null;
				GameObject newBottom = null;
				
				if(menuStack.Count > 0)
				{
					top = menuStack[0];
					newBottom = menuStack[menuStack.Count - 1];
					top.transform.parent.localPosition -= new Vector3(0, 0, bottom.GetComponent<Layer>().Bound.size.z + 1);
				}
				
				if(OnMenuPopped != null)
				{
					OnMenuPopped(bottom, top, newBottom);
				}
				_requestingObjects.Remove(bottom);
				
				bottom.GetComponent<Layer>().RequestMenuPush -= RequestPush;
				bottom.GetComponent<Layer>().RequestMenuPop -= RequestPop;

				Destroy(bottom.transform.parent.gameObject);
				Resources.UnloadUnusedAssets() ;
			}
			_busy = false;
			yield return 0;
		}
		
		// Restores the previous menu to the top
		public void PopMenu()
		{
			if(menuStack.Count > 0)
			{
				GameObject bottom = menuStack[menuStack.Count - 1];
				if(_requestingObjects.Contains(bottom) == false)
				{
					_requestingObjects.Add(bottom);
					_actions.Enqueue(this._PopMenu());
				}
			}	
			else
			{
				_actions.Enqueue(this._PopMenu());
			}			
			
		}
		
		// Clears menu stack and loads a prefab
		public void LoadMenu(GameObject prefab)
		{
			//clear stack
			//call prefabs push
			Clear();
			PushMenu(prefab, null, null, null);
		}

		void BusyClear(GameObject gObj01, GameObject gObj02, GameObject gObj03)
		{
			ResetMenus();
			OnMenuPushed -= BusyClear;
			OnMenuPopped -= BusyClear;
		}

		void ResetMenus()
		{
			print (menuStack.Count);

			for(int i = menuStack.Count - 1; i >= 0 ; i--)
			{
				Destroy(menuStack[i].transform.parent.gameObject);
			}
			
			menuStack.Clear();
			_actions.Clear();
			_requestingObjects.Clear();
		}

		// Clears menu stack
		[ContextMenu("Manager.Clear")]
		public void Clear()
		{
			//run activemenu's pop
			//destory stack
			ResetMenus();

			if(_busy)
			{
				OnMenuPushed += BusyClear;
				OnMenuPopped += BusyClear;
			}
		}
		
		void RequestPush(GameObject prefab, StackAction pushAction, StackAction popAction, object data)
		{
			PushMenu(prefab, pushAction, popAction, data);
		}
		
		void RequestPop()
		{
			PopMenu();
		}

		void Update()
		{
			if(_busy == false && _actions.Count > 0)
			{
				_busy = true;
				IEnumerator top = _actions.Dequeue();
				StartCoroutine(top);
			}
		}

	}
}
