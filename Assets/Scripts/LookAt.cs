using UnityEngine;

public class LookAt : MonoBehaviour
{
	 Transform mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera=Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
		transform.LookAt(mainCamera);
    }
}
