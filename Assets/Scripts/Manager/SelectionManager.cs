using UnityEngine;

public class SelectionManager : MonoBehaviour
{
	public static SelectionManager instance { get; private set; }
	public GameObject selectedObject = null;
	Building building;

	private void Awake()
	{
		//singleton Check
		if (instance == null)
			instance = this;
		else
			Destroy(this);
	}
	
}
