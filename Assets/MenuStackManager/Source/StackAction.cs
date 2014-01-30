using UnityEngine;
using System.Collections;
namespace MenuStackManager
{
	public class StackAction : MonoBehaviour {
		
		public enum TypeEnum
		{
			Intro = 0,
			Outro,
			Push,
			Pop
		}
		public bool Intro 
		{
			get
			{
				return (Type == TypeEnum.Intro || Type == TypeEnum.Pop);
			}
		}
		public TypeEnum Type;
		public bool Blocking = true;
		virtual public IEnumerator Action(IEnumerator parent)
		{

			Coroutine parentCoroutine = null;

			if(Intro && parent != null)
			{
				parentCoroutine = StartCoroutine(parent);
				if(Blocking)
					yield return parentCoroutine;
			}
			
			if(!Intro && !Blocking && parent != null)
			{
				parentCoroutine = StartCoroutine(parent);
			}

			yield return StartCoroutine(Move());

			if(!Intro && Blocking && parent != null)
			{
				parentCoroutine = StartCoroutine(parent);
			}
			
			if(parentCoroutine != null)
				yield return parentCoroutine;
			yield return 0;
		}

		virtual protected IEnumerator Move()
		{
			yield break;
		}
		
		public Coroutine ActionCoroutine(IEnumerator parent)
		{
			return StartCoroutine(this.Action(parent));
		}

	}
}
