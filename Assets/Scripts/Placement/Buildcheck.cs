using UnityEngine;

public class Buildcheck : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag.Equals("Placeable"))
		{
			PlacementController.instance.SetCanBuild(false);
		}
			
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.tag.Equals("Placeable"))
		{
			PlacementController.instance.SetCanBuild (true);
		}

	}
}
