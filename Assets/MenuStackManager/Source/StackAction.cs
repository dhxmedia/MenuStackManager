using UnityEngine;
using System.Collections;
namespace MenuStackManager
{
	public class StackAction : MonoBehaviour {
		
		virtual public IEnumerator Action(IEnumerator parent)
		{
			if(parent != null)
				yield return StartCoroutine(parent);
			yield return 0;
		}
		
		public Coroutine ActionCoroutine(IEnumerator parent)
		{
			return StartCoroutine(this.Action(parent));
		}

	}
}
