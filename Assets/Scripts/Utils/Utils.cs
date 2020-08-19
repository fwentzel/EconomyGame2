using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static Vector3 GetMouseGroundPosition(Vector2 mouseScreenPos)
	{
		//Get Mouseposition in World coordinates on Ground Collider
		Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
		RaycastHit hitInfo;
		int layermask = LayerMask.GetMask("Ground");
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layermask))
		{
			return  hitInfo.point;
		}
		return new Vector3();
	}

    public static GameObject GetObjectAtMousePos(Vector2 mouseScreenPos)
	{
		//Get Mouseposition in World coordinates on Ground Collider
		Ray ray = Camera.main.ScreenPointToRay(mouseScreenPos);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
		{
			return  hitInfo.collider.gameObject;
		}
		return null;
	}

}
