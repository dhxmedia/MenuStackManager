using UnityEngine;
using System.Collections;

public class ScaleInOutPushAction : MenuStackManager.StackAction {

	public Vector3 StartScale;
	public Vector3 EndScale;
	public float Length;
	override protected IEnumerator Move()
	{
		transform.localScale = StartScale;
		float currentTime = 0;
		while(currentTime < Length)
		{
			currentTime += Time.fixedDeltaTime;
			float t = currentTime/Length;
			transform.localScale = Vector3.Lerp(StartScale, EndScale, t);
			yield return new WaitForFixedUpdate();
		}
		
		yield break;
	}

}
