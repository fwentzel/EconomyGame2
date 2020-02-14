using UnityEngine;

public class Buildcheck : MonoBehaviour
{
	Building building;
	private void Awake()
	{
		building = GetComponent<Building>();
	}
	private void OnTriggerEnter(Collider other)
	{
		building.CheckCanBuild(other, true);

	}
	private void OnTriggerExit(Collider other)
	{
		building.CheckCanBuild(other, false);
	}
}
