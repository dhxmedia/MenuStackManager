using UnityEngine;
using System.Collections;
namespace MenuStackManager
{
	public class Layer : MonoBehaviour {
		public StackAction OnCreateAction;
		public StackAction OnDestroyAction;
		
		//public delegate void RequestMenuPushDelegate<T>(GameObject prefab, StackAction action, T data);
		public System.Action<GameObject, StackAction, StackAction, object> RequestMenuPush;
		public System.Action RequestMenuPop;

		public Bounds Bound
		{
			get
			{
				return _Bound;
			}
		}
		Bounds _Bound;
		// Use this for initialization
		void Awake () {
			Renderer[] r = GetComponentsInChildren<Renderer>();
			
			Bounds size = new Bounds(Vector3.zero, Vector3.zero);
			
			for(int i = 0; i < r.Length; i++)
			{
				size.Encapsulate(r[i].bounds);	
			}
			_Bound = size;

			if(OnCreateAction == null)
				OnCreateAction = gameObject.AddComponent<StackAction>();
			if(OnDestroyAction == null)
				OnDestroyAction = gameObject.AddComponent<StackAction>();
		}

		
		public Coroutine OnCreate(IEnumerator parent)
		{
			return StartCoroutine(OnCreateAction.Action(parent));
		}
		
		public Coroutine OnLayerDestroy(IEnumerator parent)
		{
			return StartCoroutine(OnDestroyAction.Action(parent));
		}
		
		public void RequestPush(GameObject obj, StackAction pushAction, StackAction popAction, object data)
		{
			if(RequestMenuPush != null)
			{
				RequestMenuPush(obj, pushAction, popAction, data);
			}
		}

		public void RequestPop()
		{
			if(RequestMenuPop != null)
			{
				RequestMenuPop();
			}
		}
	}
}