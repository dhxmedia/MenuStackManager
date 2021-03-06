﻿/* This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. */
 
using UnityEngine;
using System.Collections;
namespace MenuStackManager
{
	public class StackActionAnimation : StackAction {
		public Animator CurrentAnimator;
			

		override public IEnumerator Action(IEnumerator parent)
		{
			CurrentAnimator.SetTrigger(Type.ToString());
			CurrentAnimator.Update(0);
			CurrentAnimator.speed = 0;
			CurrentAnimator.playbackTime = 0;

			yield return StartCoroutine(base.Action (parent));
		}		

		override protected IEnumerator Move()
		{
			CurrentAnimator.speed = 1;
			CurrentAnimator.Update(0);
			yield return new WaitForFixedUpdate();

			float start = CurrentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
			float diff = (CurrentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime - start);
			while(diff < 1.0f)
			{
				diff = (CurrentAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime - start);
				yield return new WaitForFixedUpdate();
			}

			yield break;
		}
	}
}