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
			//_busy = true;
			GameObject newMenuObject = Instantiate(prefab) as GameObject;
			newMenuObject.GetComponent<Layer>().RequestMenuPush += RequestPush;
			newMenuObject.GetComponent<Layer>().RequestMenuPop += RequestPop;
			newMenuObject.transform.parent = transform;
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
				top.transform.localPosition += new Vector3(0, 0, newMenuObject.GetComponent<Layer>().Bound.size.z);
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
				//newMenuObject.transform.parent = bottom.transform;
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
			//_busy = true;
			if(menuStack.Count > 0)
			{
				GameObject bottom = menuStack[menuStack.Count - 1];
				menuStack.RemoveAt(menuStack.Count - 1);
				bottom.transform.parent = transform;
				
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
					menuStack[0].transform.localPosition -= new Vector3(0, 0, bottom.GetComponent<Layer>().Bound.size.z);
				}
				
				if(OnMenuPopped != null)
				{
					OnMenuPopped(bottom, top, newBottom);
				}
				_requestingObjects.Remove(bottom);
				Destroy(bottom);
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
		}
		
		// Clears menu stack
		public void Clear()
		{
			//run activemenu's pop
			//destory stack
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
