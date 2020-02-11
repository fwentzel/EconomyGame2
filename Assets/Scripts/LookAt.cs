using UnityEngine;

public class LookAt : MonoBehaviour
{
	public Transform mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		transform.LookAt(mainCamera);
    }
}
