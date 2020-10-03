using UnityEngine;

public class Buildcheck : MonoBehaviour
{
    Building building;
    bool isInCollider = false;
    private void Awake()
    {
        building = GetComponent<Building>();
    }
    private void Update()
    {
        if (isInCollider) return;
        //Only distance check
        building.CheckCanBuild(null, true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ground"))
        {
            isInCollider = true;
        }
        building.CheckCanBuild(other, true);

    }
    private void OnTriggerExit(Collider other)
    {
        isInCollider = false;
        building.CheckCanBuild(other, false);
    }
}
