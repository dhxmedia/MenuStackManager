using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class SimpleAnchor : MonoBehaviour {

	public enum Anchor
	{
		TopLeft,
		TopMiddle,
		TopRight,
		MiddleLeft,
		MiddleMiddle,
		MiddleRight,
		BottomLeft,
		BottomMiddle,
		BottomRight,
		None
	}

	public Anchor AnchorType = Anchor.TopLeft;
	Anchor _LastAnchor = Anchor.None;

	Dictionary<Anchor, Vector3> _AnchorLookup = new Dictionary<Anchor, Vector3>();
	bool _Inited = false;
	public Vector3 Offset;
	void Reset()
	{
		Init ();
	}

	void Awake()
	{
		
		Init ();
	}
	// Use this for initialization
	void Init () {
		Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
		Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
		Vector3 diff = topRight - bottomLeft;
		Vector3 diffX = diff;
		diffX.y = 0;
		diffX.z = 0;
		Vector3 diffY = diff;
		diffY.x = 0;
		diffY.z = 0;

		Vector3 middleMiddle = (topRight + bottomLeft)/2.0f;
		middleMiddle.z = 0;
		_AnchorLookup = new Dictionary<Anchor, Vector3>();
		_AnchorLookup[Anchor.None] = Vector3.zero;

		_AnchorLookup[Anchor.TopLeft] = middleMiddle + diffY/2.0f - diffX/2.0f;
		_AnchorLookup[Anchor.TopMiddle] = middleMiddle + diffY/2.0f;
		_AnchorLookup[Anchor.TopRight] = middleMiddle + diffY/2.0f + diffX/2.0f;
		
		_AnchorLookup[Anchor.MiddleLeft] = middleMiddle - diffX/2.0f;
		_AnchorLookup[Anchor.MiddleMiddle] = middleMiddle;
		_AnchorLookup[Anchor.MiddleRight] = middleMiddle + diffX/2.0f;
		
		_AnchorLookup[Anchor.BottomLeft] = middleMiddle - diffY/2.0f - diffX/2.0f;
		_AnchorLookup[Anchor.BottomMiddle] = middleMiddle - diffY/2.0f;
		_AnchorLookup[Anchor.BottomRight] = middleMiddle - diffY/2.0f + diffX/2.0f;

		_Inited = true;
	}

	// Update is called once per frame
	void Update () {
		if(_Inited == false)
		{
			_Inited = true;
			Init();
		}
		transform.localPosition = _AnchorLookup[AnchorType] + Offset;
	}
}
