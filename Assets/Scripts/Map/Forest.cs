using UnityEngine;

public class Forest : MonoBehaviour
{

    public void OnBuild()
    {
        transform.Rotate(new Vector3(0, Random.Range(0, 360), 0));
       
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i).transform;
            t.Rotate(new Vector3(0, Random.Range(0, 360), 0));
            t.localScale += new Vector3(0, Random.Range(-.4f, .5f), 0);
            t = t.transform.GetChild(1);
             MaterialPropertyBlock _propBlock = new MaterialPropertyBlock();

        _propBlock.SetVector("Pos", new Vector4(t.position.x+Random.Range(-5,5), t.position.z+Random.Range(-5,5), 0, 0));
            // Apply the edited values to the renderer.
            t.GetComponent<Renderer>().SetPropertyBlock(_propBlock);
        }
    }
}
