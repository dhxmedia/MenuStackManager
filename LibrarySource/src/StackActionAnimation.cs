using UnityEngine;
using System.Collections;
namespace MenuStackManager
{
	public class StackActionAnimation : StackAction {
		public enum TypeEnum
		{
			Intro = 0,
			Outro,
			Push,
			Pop
		}
		public Animator CurrentAnimator;
		public bool Intro 
		{
			get
			{
				return (Type == TypeEnum.Intro || Type == TypeEnum.Pop);
			}
		}
		public TypeEnum Type;

		override public IEnumerator Action(IEnumerator parent)
		{
			CurrentAnimator.SetTrigger(Type.ToString());

			CurrentAnimator.Update(0);
			CurrentAnimator.speed = 0;
			if(Intro)
				yield return StartCoroutine(base.Action(parent));
			
			CurrentAnimator.speed = 1;
			CurrentAnimator.Update(0);

			while(CurrentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
			{
				yield return new WaitForFixedUpdate();
			}
			
			if(!Intro)
				yield return StartCoroutine(base.Action(parent));
			yield return 0;
		}		

	}
}