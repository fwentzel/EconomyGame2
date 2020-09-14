using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionCircle : MonoBehaviour
{

	Transform selectedTransform;
	SpriteRenderer spr => GetComponent<SpriteRenderer>();
	bool isMovable = true;
	float offset = 0;
	// Update is called once per frame
	private void Awake() {
		spr.enabled = false;
	}
	private void Start()
	{
		SelectionManager.instance.OnSelectionChange += updateTransform;
	}

	void Update()
    {
		if (selectedTransform != null && isMovable)
			setPos();

	}

	void updateTransform()
	{
		GameObject obj = SelectionManager.instance.selectedObject;
		if (obj == null || obj.GetComponent<Ship>() != null)
		{
			spr.enabled = false;
			selectedTransform = null;
			return;
		}
		offset=0;
		if (obj.GetComponent<Building>() == null)
			isMovable = true ;
		else
		{
			isMovable = false;
			offset = .01f;
		}
		
		selectedTransform = obj.transform;

		setPos();
		spr.enabled = true;
	}

	void setPos()
	{
		transform.position = selectedTransform.position +new Vector3(0,offset,0);
	}
}
